using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Hangfire;
using Hangfire.Server;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Users;
using ImperaPlus.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Normal)]
    [DisableConcurrentExecution(60)]
    public class UserCleanupJob : AsyncJob
    {
        public const string JobId = "UserCleanup";

        private readonly UserManager<User> userManager;
        private readonly IEventAggregator eventAggregator;
        private IUnitOfWork unitOfWork;

        public UserCleanupJob(ILifetimeScope scope)
            : base(scope)
        {
            unitOfWork = LifetimeScope.Resolve<IUnitOfWork>();
            userManager = LifetimeScope.Resolve<UserManager<User>>();
            eventAggregator = LifetimeScope.Resolve<IEventAggregator>();
        }

        [AutomaticRetry(Attempts = 0)]
        public override async Task Handle(PerformContext performContex)
        {
            await base.Handle(performContex);

            await TraceContext.TraceAsync("Processing user cleanup", async () =>
            {
                var days = -30;
                if (TestSupport.RunningUnderTest)
                {
                    days = 1;
                }

                var users = unitOfWork.Users.FindUsersToDelete(days).ToArray();
                foreach (var user in users)
                {
                    if (new string[] { "Ghost", "Bot", "System" }.Contains(user.UserName))
                    {
                        continue;
                    }

                    // Skip admin users
                    var userRoles = await userManager.GetRolesAsync(user);
                    if (userRoles.Any(role => role.Contains("admin")))
                    {
                        Log.Log(LogLevel.Info, "Skipping admin user {0} '{1}' from cleanup", user.Id, user.UserName);
                        continue;
                    }

                    try
                    {
                        Log.Log(LogLevel.Info, "Deleting user {0} '{1}'", user.Id, user.UserName);

                        // Ensure sub-systems know about this
                        eventAggregator.Raise(new AccountDeleted(user, true));

                        await userManager.DeleteAsync(user);
                        unitOfWork.Commit();

                        Log.Log(LogLevel.Info, "Deleted user {0} '{1}'", user.Id, user.UserName);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        Log.Log(LogLevel.Error, "DbUpdateConcurrencyException while deleting users");
                    }
                    catch (Exception ex)
                    {
                        Log.Log(LogLevel.Error, "Error while deleting user {0}", ex);
                    }
                }
            });
        }
    }
}
