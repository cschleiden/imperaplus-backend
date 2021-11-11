using Autofac;
using Hangfire;
using Hangfire.Server;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Normal)]
    [DisableConcurrentExecution(60)]
    [AutomaticRetry(Attempts = 0)]
    public class GameCleanupJob : Job
    {
        public const string JobId = "GameCleanup";

        private IUnitOfWork unitOfWork;

        public GameCleanupJob(ILifetimeScope scope)
            : base(scope)
        {
            unitOfWork = LifetimeScope.Resolve<IUnitOfWork>();
        }

        public override void Handle(PerformContext performContext)
        {
            base.Handle(performContext);

            Log.Log(Domain.LogLevel.Info, "Cleaning up fun games...");

            var deletedCount = unitOfWork.Games.DeleteOpenPasswordFunGames();
            Log.Log(Domain.LogLevel.Info, "Removed {0} fun games", deletedCount);
            unitOfWork.Commit();

            var deletedEndedCount = unitOfWork.Games.DeleteEndedGames();
            Log.Log(Domain.LogLevel.Info, "Removed {0} ended games", deletedEndedCount);
            unitOfWork.Commit();

            Log.Log(Domain.LogLevel.Info, "Done");
        }
    }
}
