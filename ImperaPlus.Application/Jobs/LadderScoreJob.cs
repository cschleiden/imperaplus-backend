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
            this.unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>();
            this.scoringService = this.LifetimeScope.Resolve<IScoringService>();
        }

        public override void Handle(PerformContext performContext)
        {
            base.Handle(performContext);

            this.Log.Log(Domain.LogLevel.Info, "Entering scoring job");

            var unscoredGames = this.unitOfWork.Games.FindUnscoredLadderGames();

            foreach(var unscoredGame in unscoredGames.ToArray())
            {
                try
                {
                    this.Log.Log(Domain.LogLevel.Info, "Scoring game " + unscoredGame.Id);

                    var ladder = this.unitOfWork.Ladders.GetById(unscoredGame.LadderId.Value);
                    this.scoringService.Score(ladder, unscoredGame);

                    this.Log.Log(Domain.LogLevel.Info, "Done " + unscoredGame.Id);
                }
                catch (Exception ex)
                {
                    this.Log.Log(Domain.LogLevel.Error, "Error scoring game {0} {1}", unscoredGame.Id, ex);
                }

                this.unitOfWork.Commit();
            }
        }
    }
}
