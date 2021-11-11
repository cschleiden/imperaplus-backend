using Autofac;
using Hangfire;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Bots;
using ImperaPlus.Domain.Services;
using Hangfire.Server;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Normal)]
    [AutomaticRetry(Attempts = 0)]
    public class BotJob : BackgroundJob
    {
        private IUnitOfWork unitOfWork;

        public BotJob(ILifetimeScope scope)
            : base(scope)
        {
            unitOfWork = LifetimeScope.Resolve<IUnitOfWork>();
        }

        public void Play(long gameId, PerformContext performContext)
        {
            var logger = new JobLogger(performContext);

            var game = unitOfWork.Games.Find(gameId);
            if (game.State != Domain.Enums.GameState.Active)
            {
                return;
            }

            var mapTemplateProvider = LifetimeScope.Resolve<IMapTemplateProvider>();
            var mapTemplate = mapTemplateProvider.GetTemplate(game.MapTemplateName);

            var attackService = LifetimeScope.Resolve<IAttackService>();
            var randomGen = LifetimeScope.Resolve<IRandomGen>();

            var bot = new Bot(logger, game, mapTemplate, attackService, randomGen);

            bot.PlayTurn();

            unitOfWork.Commit();
        }
    }
}
