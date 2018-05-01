using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Hangfire;
using Hangfire.Server;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Normal)]
    [DisableConcurrentExecution(60)]
    [AutomaticRetry(Attempts = 0)]
    public class GameCleanupJob : Job
    {
        private IGameService gameService;

        public GameCleanupJob(ILifetimeScope scope)
            : base(scope)
        {
            this.gameService = this.LifetimeScope.Resolve<IGameService>();
        }

        public override void Handle(PerformContext performContext)
        {
            base.Handle(performContext);

            this.Log.Log(Domain.LogLevel.Info, "Cleaning up fun games...");

            this.gameService.CleanupFunGames();

            this.Log.Log(Domain.LogLevel.Info, "Done");
        }
    }
}
