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
            this.unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>();
            this.userManager = this.LifetimeScope.Resolve<UserManager<User>>();
            this.eventAggregator = this.LifetimeScope.Resolve<IEventAggregator>();
        }

        [AutomaticRetry(Attempts = 0)]
        public override async Task Handle(PerformContext performContex)
        {
            await base.Handle(performContex);

            await TraceContext.TraceAsync("Processing user cleanup", async () =>
            {
                int days = -30;
                if (TestSupport.RunningUnderTest)
                {
                    days = 1;
                }

                var users = this.unitOfWork.Users.FindUsersToDelete(days).ToArray();
                foreach (var user in users)
                {
                    if (new string[] { "Ghost", "Bot", "System" }.Contains(user.UserName))
                    {
                        continue;
                    }

                    try
                    {
                        this.Log.Log(LogLevel.Info, "Deleting user {0} '{1}'", user.Id, user.UserName);

                        // Ensure sub-systems know about this
                        this.eventAggregator.Raise(new AccountDeleted(user, true));

                        await this.userManager.DeleteAsync(user);
                        this.unitOfWork.Commit();

                        this.Log.Log(LogLevel.Info, "Deleted user {0} '{1}'", user.Id, user.UserName);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        this.Log.Log(LogLevel.Error, "DbUpdateConcurrencyException while deleting users");
                    }
                    catch (Exception ex)
                    {
                        this.Log.Log(LogLevel.Error, "Error while deleting user {0}", ex);
                    }
                }
            });
        }
    }
}
