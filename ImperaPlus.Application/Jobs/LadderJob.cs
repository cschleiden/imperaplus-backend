using Autofac;
using Hangfire;
using Hangfire.Server;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using ImperaPlus.Utils;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Critical)]
    [DisableConcurrentExecution(60)]
    [AutomaticRetry(Attempts = 0)]
    public class LadderJob : Job
    {
        private IUnitOfWork unitOfWork;
        private ILadderService ladderService;
        private IRandomGenProvider randomGenProvider;

        public LadderJob(ILifetimeScope scope)
            : base(scope)
        {
            unitOfWork = LifetimeScope.Resolve<IUnitOfWork>();
            ladderService = LifetimeScope.Resolve<ILadderService>();
            randomGenProvider = LifetimeScope.Resolve<IRandomGenProvider>();
        }

        public override void Handle(PerformContext performContext)
        {
            base.Handle(performContext);

            TraceContext.Trace("Processing ladder", () =>
            {
                try
                {
                    ladderService.CheckAndCreateMatches(randomGenProvider.GetRandomGen());
                }
                catch (DbUpdateConcurrencyException)
                {
                    Log.Log(Domain.LogLevel.Error, "DbUpdateConcurrencyException while processing ladders");
                }
            });
        }
    }
}
