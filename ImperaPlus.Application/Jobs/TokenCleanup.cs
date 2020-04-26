using Autofac;
using Hangfire;
using Hangfire.Server;
using ImperaPlus.DataAccess;
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
        private ImperaContext dbContext;

        public TokenCleanupJob(ILifetimeScope scope)
            : base(scope)
        {
            this.tokenManager = this.LifetimeScope.Resolve<OpenIddictTokenManager<OpenIddictToken>>();
            this.authorizationManager = this.LifetimeScope.Resolve<OpenIddictAuthorizationManager<OpenIddictAuthorization>>();
            this.dbContext = this.LifetimeScope.Resolve<ImperaContext>();
        }

        public override void Handle(PerformContext performContext)
        {
            base.Handle(performContext);

            this.Log.Log(Domain.LogLevel.Info, "Cleaning up tokens...");

            var strategy = dbContext.Database.CreateExecutionStrategy();
            strategy.Execute<object, object>(null, (context, state) =>
            {
                this.tokenManager.PruneAsync().Wait();

                this.authorizationManager.PruneAsync().Wait();

                return null;
            }, null);

            this.Log.Log(Domain.LogLevel.Info, "Done");
        }
    }
}
