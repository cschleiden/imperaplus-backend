using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Hangfire;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using NLog.Fluent;

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

        public override void Handle()
        {
            Log.Info().Message("Entering scoring job").Write();

            var unscoredGames = this.unitOfWork.Games.FindUnscoredLadderGames();

            foreach(var unscoredGame in unscoredGames)
            {
                try
                {
                    Log.Info().Message("Scoring game " + unscoredGame.Id).Write();

                    var ladder = this.unitOfWork.Ladders.GetById(unscoredGame.LadderId.Value);
                    this.scoringService.Score(ladder, unscoredGame);
                }
                catch (Exception ex)
                {
                    Log.Info().Message("Error scoring game " + unscoredGame.Id).Exception(ex).Write();
                }

                this.unitOfWork.Commit();
            }
        }
    }
}
