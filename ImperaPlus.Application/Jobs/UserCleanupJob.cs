using Autofac;
using Hangfire;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Fluent;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Normal)]
    [DisableConcurrentExecution(60)]
    public class UserCleanupJob : AsyncJob
    {
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
                    var users = this.unitOfWork.Users.FindUsersToDelete().ToArray();
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
