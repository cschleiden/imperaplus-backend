using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Games.Events;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using ImperaPlus.Utils;
using NLog.Fluent;

namespace ImperaPlus.Application.Notifications
{
    public class LadderNotifications : ICompletedEventHandler<GameEndedEvent>
    {
        private IScoringService scoringService;
        private IUnitOfWork unitOfWork;

        public LadderNotifications(IUnitOfWork unitOfWork, IScoringService scoringService)
        {
            this.unitOfWork = unitOfWork;
            this.scoringService = scoringService;
        }

        public void Handle(GameEndedEvent evt)
        {
            var game = evt.Game;

            if (game.Type == Domain.Enums.GameType.Ranking)
            {
                TraceContext.Trace("Score ranking game", () =>
                {
                    if (game.LadderId == null)
                    {
                        Log.Error().Message("Ranking game '{0}' has ended, but ladder id is empty", game.Id).Write();
                        return;
                    }

                    var ladder = unitOfWork.Ladders.GetById(game.LadderId.Value);

                    // Score game
                    scoringService.Score(ladder, game);

                    unitOfWork.Commit();
                });
            }
        }
    }
}
