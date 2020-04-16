using System;
using System.Linq;
using Autofac;
using Hangfire;
using Hangfire.Server;
using ImperaPlus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Critical)]
    [DisableConcurrentExecution(60)]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public class TimeoutJob : Job
    {
        private IUnitOfWork unitOfWork;

        public TimeoutJob(ILifetimeScope scope)
            : base(scope)
        { 
            this.unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>();
        }

        public override void Handle(PerformContext performContext)
        {
            base.Handle(performContext);

            this.Log.Log(Domain.LogLevel.Info, "Processing timeouts");

            var gameIds = this.unitOfWork.Games.FindTimeoutGames().ToArray();
            
            foreach(var gameId in gameIds)
            {
                try
                {
                    var game = this.unitOfWork.Games.Find(gameId);
                    this.Log.Log(Domain.LogLevel.Info, "Processing timeout in game {0} {1}", game.Id, game.Name);
                    game.ProcessTimeouts();
                }
                catch (Exception e)
                {
                    // Log and continue with next game
                    this.Log.Log(Domain.LogLevel.Error, "Error while processing timeouts for game {0} {1}", gameId, e.ToString());
                }

                try
                {
                    this.unitOfWork.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    this.Log.Log(Domain.LogLevel.Error, "DbUpdateConcurrencyException for game {0}", gameId);
                }
            }
        }
    }
}
