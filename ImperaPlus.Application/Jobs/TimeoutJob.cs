using System.Linq;
using Autofac;
using Hangfire;
using ImperaPlus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using NLog.Fluent;
using System;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Critical)]
    [DisableConcurrentExecution(60)]
    [AutomaticRetry(Attempts = 0)]
    public class TimeoutJob : Job
    {
        private IUnitOfWork unitOfWork;

        public TimeoutJob(ILifetimeScope scope)
            : base(scope)
        { 
            this.unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>();
        }

        [AutomaticRetry(Attempts = 0)]
        public void Handle()
        {
            Log.Info("Processing timeouts").Write();

            var games = this.unitOfWork.Games.FindTimeoutGames().ToArray();
            
            foreach(var game in games)
            {
                try
                {
                    Log.Info().Message("Processing timeout in game {0} {1}", game.Id, game.Name).Write();
                    game.ProcessTimeouts();
                }
                catch (Exception e)
                {
                    // Log and continue with next game
                    Log.Error()
                        .Message("Error while processing timeouts for game {0}", game.Id)
                        .Exception(e);
                }

                try
                {
                    this.unitOfWork.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    Log.Warn().Message("DbUpdateConcurrencyException for game {0}", game.Id);
                }
            }
        }
    }
}
