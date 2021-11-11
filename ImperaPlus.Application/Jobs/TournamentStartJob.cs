using System;
using Autofac;
using Hangfire;
using Hangfire.Server;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Normal)]
    [DisableConcurrentExecution(60)]
    [AutomaticRetry(Attempts = 0)]
    public class TournamentStartJob : Job
    {
        private IUnitOfWork unitOfWork;
        private ITournamentService tournamentService;
        private IRandomGenProvider randomGenProvider;

        public TournamentStartJob(ILifetimeScope scope)
            : base(scope)
        {
            unitOfWork = LifetimeScope.Resolve<IUnitOfWork>();
            tournamentService = LifetimeScope.Resolve<ITournamentService>();
            randomGenProvider = LifetimeScope.Resolve<IRandomGenProvider>();
        }

        public override void Handle(PerformContext performContext)
        {
            base.Handle(performContext);

            try
            {
                if (tournamentService.CheckOpenTournaments(Log, randomGenProvider.GetRandomGen()))
                {
                    Log.Log(Domain.LogLevel.Info, "Found changes, saving...");
                    unitOfWork.Commit();
                    Log.Log(Domain.LogLevel.Info, "done.");
                }
            }
            catch (Exception e)
            {
                Log.Log(Domain.LogLevel.Error, "Error {0}", e);
            }
        }
    }
}
