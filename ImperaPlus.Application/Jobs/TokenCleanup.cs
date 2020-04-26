using Autofac;
using Hangfire;
using Hangfire.Server;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Normal)]
    [DisableConcurrentExecution(60)]
    [AutomaticRetry(Attempts = 0)]
    public class TokenCleanupJob : Job
    {
        public const string JobId = "TokenCleanup";

        private OpenIddictTokenManager<OpenIddictToken> tokenManager;

        private OpenIddictAuthorizationManager<OpenIddictAuthorization> authorizationManager;

        public TokenCleanupJob(ILifetimeScope scope)
            : base(scope)
        {
            this.tokenManager = this.LifetimeScope.Resolve<OpenIddictTokenManager<OpenIddictToken>>();
            this.authorizationManager = this.LifetimeScope.Resolve<OpenIddictAuthorizationManager<OpenIddictAuthorization>>();
        }

        public override void Handle(PerformContext performContext)
        {
            base.Handle(performContext);

            this.Log.Log(Domain.LogLevel.Info, "Cleaning up tokens...");

            this.tokenManager.PruneAsync().RunSynchronously();

            this.authorizationManager.PruneAsync().RunSynchronously();

            this.Log.Log(Domain.LogLevel.Info, "Done");
        }
    }
}
