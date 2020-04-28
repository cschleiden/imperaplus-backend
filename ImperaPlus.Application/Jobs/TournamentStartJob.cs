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
            this.unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>();
            this.tournamentService = this.LifetimeScope.Resolve<ITournamentService>();
            this.randomGenProvider = this.LifetimeScope.Resolve<IRandomGenProvider>();
        }

        public override void Handle(PerformContext performContext)
        {
            base.Handle(performContext);

            try
            {
                if (this.tournamentService.CheckOpenTournaments(this.Log, this.randomGenProvider.GetRandomGen()))
                {
                    this.Log.Log(Domain.LogLevel.Info, "Found changes, saving...");
                    this.unitOfWork.Commit();
                    this.Log.Log(Domain.LogLevel.Info, "done.");
                }
            }
            catch (Exception e)
            {
                this.Log.Log(Domain.LogLevel.Error, "Error {0}", e);
            }
        }
    }

}