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

        public TournamentJob(ILifetimeScope scope)
        : base(scope)
        {
            this.unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>();
            this.tournamentService = this.LifetimeScope.Resolve<ITournamentService>();
        }

        [AutomaticRetry(Attempts = 0)]
        public void Handle()
        {
            this.tournamentService.CheckTournaments();
            this.unitOfWork.Commit();
        }
    }
}