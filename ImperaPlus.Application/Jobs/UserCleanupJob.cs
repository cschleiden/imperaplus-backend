using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Hangfire;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Fluent;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Normal)]
    [DisableConcurrentExecution(60)]
    public class UserCleanupJob : AsyncJob
    {
        public const string JobId = "UserCleanup";

        private readonly UserManager<User> userManager;
        private IUnitOfWork unitOfWork;

        public UserCleanupJob(ILifetimeScope scope)
            : base(scope)
        {
            this.unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>();
            this.userManager = this.LifetimeScope.Resolve<UserManager<User>>();
        }

        [AutomaticRetry(Attempts = 0)]
        public override async Task Handle()
        {
            await TraceContext.TraceAsync("Processing user cleanup", async () =>
            {
                try
                {
                    int days = -30;
                    if (TestSupport.RunningUnderTest)
                    {
                        days = 1;
                    }

                    days = 1; // TODO: CS: Change! 

                    var users = this.unitOfWork.Users.FindUsersToDelete(days).ToArray();
                    foreach (var user in users)
                    {
                        Log.Info().Message("Deleting user {0} '{1}'", user.Id, user.UserName).Write();

                        await this.userManager.DeleteAsync(user);
                        this.unitOfWork.Commit();

                        Log.Info().Message("Deleted user {0} '{1}'", user.Id, user.UserName).Write();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    Log.Warn().Message("DbUpdateConcurrencyException while processing ladders").Write();
                }
                catch (Exception ex)
                {
                    Log
                        .Error()
                        .Message("Error while deleting user")
                        .Exception(ex)
                        .Write();
                }
            });
        }
    }
}
