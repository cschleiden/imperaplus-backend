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

        public UserCleanupJob(ILifetimeScope scope)
            : base(scope)
        {
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

                string[] userIds;

                using (var unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>())
                {
                    userIds = unitOfWork.Users.FindUsersToDelete(days).ToArray().Select(x => x.Id).ToArray();
                }

                foreach (var userId in userIds)
                {
                    using (var unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>())
                    using (var userManager = this.LifetimeScope.Resolve<UserManager<User>>())
                    {
                        var eventAggregator = this.LifetimeScope.Resolve<IEventAggregator>();

                        try
                        {
                            var user = unitOfWork.Users.FindById(userId);

                            this.Log.Log(LogLevel.Info, "Deleting user {0} '{1}'", user.Id, user.UserName);

                            // Ensure sub-systems know about this
                            eventAggregator.Raise(new AccountDeleted(user, true));
                            await userManager.DeleteAsync(user);
                            unitOfWork.Commit();

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
                }
            });
        }
    }
}
