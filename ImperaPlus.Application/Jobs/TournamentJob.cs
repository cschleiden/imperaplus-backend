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
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public class TournamentJob : Job
    {
        private IUnitOfWork unitOfWork;
        private ITournamentService tournamentService;
        private IRandomGenProvider randomGenProvider;

        public TournamentJob(ILifetimeScope scope)
        : base(scope)
        {
            this.unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>();
            this.tournamentService = this.LifetimeScope.Resolve<ITournamentService>();
            this.randomGenProvider = this.LifetimeScope.Resolve<IRandomGenProvider>();
        }

        public override void Handle(PerformContext performContext)
        {
            base.Handle(performContext);

            this.tournamentService.CheckTournaments(this.randomGenProvider.GetRandomGen());

            try
            {
                this.unitOfWork.Commit();
            }
            catch(Exception e)
            {
                this.Log.Log(LogLevel.Error, "Error: {0}", e);
            }
        }
    }
}