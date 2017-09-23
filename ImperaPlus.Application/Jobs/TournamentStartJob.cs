using Autofac;
using Hangfire;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Normal)]
    [DisableConcurrentExecution(60)]
    public class TournamentStartJob : Job
    {
        private IUnitOfWork unitOfWork;
        private ITournamentService tournamentService;
        private IRandomGenProvider randomGenProvider;

        public TournamentStartJob(ILifetimeScope scope)
        : base(scope)
        {
            this.unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>();
            this.tournamentService = this.LifetimeScope.Resolve<ITournamentService>();
            this.randomGenProvider = this.LifetimeScope.Resolve<IRandomGenProvider>();
        }

        [AutomaticRetry(Attempts = 0)]
        public override void Handle()
        {
            if (this.tournamentService.CheckOpenTournaments(this.randomGenProvider.GetRandomGen()))
            {
                this.unitOfWork.Commit();
            }
        }
    }

}