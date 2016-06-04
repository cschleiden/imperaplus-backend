using Autofac;
using Hangfire;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Bots;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Normal)]
    [AutomaticRetry(Attempts = 0)]
    public class BotJob : Job
    {
        private IUnitOfWork unitOfWork;
       
        public BotJob(ILifetimeScope scope)
            : base(scope)
        {
            this.unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>();
        }

        public void Play(long gameId)
        {
            var game = this.unitOfWork.Games.Find(gameId);
            if (game.State != Domain.Enums.GameState.Active)
            {
                return;
            }

            var bot = new Bot(game);

            bot.PlayTurn();

            this.unitOfWork.Commit();
        }
    }
}
