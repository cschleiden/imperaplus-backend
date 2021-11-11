using System;
using System.Linq;
using Autofac;
using Hangfire;
using Hangfire.Server;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Normal)]
    [DisableConcurrentExecution(60)]
    public class LadderScorejob : Job
    {
        public const string JobId = "LadderScore";

        private IUnitOfWork unitOfWork;
        private IScoringService scoringService;

        public LadderScorejob(ILifetimeScope scope)
            : base(scope)
        {
            unitOfWork = LifetimeScope.Resolve<IUnitOfWork>();
            scoringService = LifetimeScope.Resolve<IScoringService>();
        }

        public override void Handle(PerformContext performContext)
        {
            base.Handle(performContext);

            Log.Log(Domain.LogLevel.Info, "Entering scoring job");

            var unscoredGames = unitOfWork.Games.FindUnscoredLadderGames();

            foreach (var unscoredGame in unscoredGames.ToArray())
            {
                try
                {
                    Log.Log(Domain.LogLevel.Info, "Scoring game " + unscoredGame.Id);

                    var ladder = unitOfWork.Ladders.GetById(unscoredGame.LadderId.Value);
                    scoringService.Score(ladder, unscoredGame);

                    Log.Log(Domain.LogLevel.Info, "Done " + unscoredGame.Id);
                }
                catch (Exception ex)
                {
                    Log.Log(Domain.LogLevel.Error, "Error scoring game {0} {1}", unscoredGame.Id, ex);
                }

                unitOfWork.Commit();
            }
        }
    }
}
