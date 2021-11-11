using System;
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

        private OpenIddictTokenManager<OpenIddictEntityFrameworkCoreToken> tokenManager;

        private OpenIddictAuthorizationManager<OpenIddictEntityFrameworkCoreAuthorization> authorizationManager;
        private ImperaContext dbContext;

        public TokenCleanupJob(ILifetimeScope scope)
            : base(scope)
        {
            tokenManager = LifetimeScope.Resolve<OpenIddictTokenManager<OpenIddictEntityFrameworkCoreToken>>();
            authorizationManager = LifetimeScope
                .Resolve<OpenIddictAuthorizationManager<OpenIddictEntityFrameworkCoreAuthorization>>();
            dbContext = LifetimeScope.Resolve<ImperaContext>();
        }

        public override void Handle(PerformContext performContext)
        {
            base.Handle(performContext);

            Log.Log(Domain.LogLevel.Info, "Cleaning up tokens...");

            var strategy = dbContext.Database.CreateExecutionStrategy();
            strategy.Execute<object, object>(null, (context, state) =>
            {
                tokenManager.PruneAsync(DateTimeOffset.Now - TimeSpan.FromDays(7));
                authorizationManager.PruneAsync(DateTimeOffset.Now - TimeSpan.FromDays(7));

                return null;
            }, null);

            Log.Log(Domain.LogLevel.Info, "Done");
        }
    }
}
