using System;
using Autofac;
using Hangfire;
using Hangfire.Server;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Application.Jobs
{
    /// <summary>
    /// Handle tournaments, synchronizes games to pairings, advances rounds etc.
    /// </summary>
    [Queue(JobQueues.Critical)]
    [DisableConcurrentExecution(60)]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public class TournamentJob : Job
    {
        private IUnitOfWork unitOfWork;
        private ITournamentService tournamentService;
        private IRandomGenProvider randomGenProvider;

        public TournamentJob(ILifetimeScope scope)
            : base(scope)
        {
            unitOfWork = LifetimeScope.Resolve<IUnitOfWork>();
            tournamentService = LifetimeScope.Resolve<ITournamentService>();
            randomGenProvider = LifetimeScope.Resolve<IRandomGenProvider>();
        }

        public override void Handle(PerformContext performContext)
        {
            base.Handle(performContext);

            tournamentService.CheckTournaments(Log, randomGenProvider.GetRandomGen());

            try
            {
                unitOfWork.Commit();
            }
            catch (Exception e)
            {
                Log.Log(LogLevel.Error, "Error: {0}", e);
            }

            Log.Log(LogLevel.Info, "Done");
        }
    }
}
