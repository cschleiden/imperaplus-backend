using Autofac;
using Hangfire;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using ImperaPlus.Utils;
using NLog.Fluent;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Critical)]
    [DisableConcurrentExecution(60)]
    public class LadderJob : Job
    {
        private IUnitOfWork unitOfWork;
        private ILadderService ladderService;

        public LadderJob(ILifetimeScope scope)
            : base(scope)
        {
            this.unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>();
            this.ladderService = this.LifetimeScope.Resolve<ILadderService>();
        }

        [AutomaticRetry(Attempts = 0)]
        public void Handle()
        {
            TraceContext.Trace("Processing ladder", () =>
            {
                try
                {
                    this.ladderService.CheckAndCreateMatches();
                }
                catch (OptimisticConcurrencyException)
                {
                    Log.Warn().Message("OptimisticConcurrencyException while processing ladders").Write();
                }
                catch (DbUpdateConcurrencyException)
                {
                    Log.Warn().Message("DbUpdateConcurrencyException while processing ladders").Write();
                }
            });
        }
    }
}
