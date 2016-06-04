﻿using Autofac;
using Hangfire;
using ImperaPlus.Domain.Repositories;
using NLog.Fluent;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;

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
                Log.Info().Message("Processing timeout in game {0} {1}", game.Id, game.Name).Write();
                game.ProcessTimeouts();

                try
                {
                    this.unitOfWork.Commit();
                }
                catch (OptimisticConcurrencyException)
                {
                    Log.Warn().Message("OptimisticConcurrencyException for game {0}", game.Id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    Log.Warn().Message("DbUpdateConcurrencyException for game {0}", game.Id);
                }
            }
        }
    }
}
