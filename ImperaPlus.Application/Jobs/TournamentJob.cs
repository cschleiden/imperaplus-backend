using Autofac;
using Hangfire;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Application.Jobs
{
    /// <summary>
    /// Handle tournaments, synchronizes games to pairings, advances rounds etc.
    /// </summary>
    [Queue(JobQueues.Critical)]
    [DisableConcurrentExecution(60)]
    [AutomaticRetry(Attempts = 0)]
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

        [AutomaticRetry(Attempts = 0)]
        public override void Handle()
        {
            this.tournamentService.CheckTournaments(this.randomGenProvider.GetRandomGen());
            this.unitOfWork.Commit();
        }
    }
}