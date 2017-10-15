using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Users;

namespace ImperaPlus.Domain.Games.EventHandler
{
    public class AccountDeletedHandler : IEventHandler<AccountDeleted>
    {
        private IUnitOfWork unitOfWork;
        private IGameService gameService;

        public AccountDeletedHandler(IUnitOfWork unitOfWork, IGameService gameService)
        {
            this.unitOfWork = unitOfWork;
            this.gameService = gameService;
        }

        public void Handle(AccountDeleted evt)
        {
            var user = evt.User;

            // Surrender in all games
            var games = this.unitOfWork.Games.FindForUser(user.Id);
            foreach (var game in games)
            {
                var player = game.GetPlayerForUser(user.Id);
                if (player != null)
                {
                    if (game.CanLeave)
                    {
                        if (game.CanBeDeleted && game.CreatedById == user.Id)
                        {
                            this.gameService.Delete(user, game.Id);
                        }
                        else
                        {
                            game.Leave(user);
                        }
                    }
                    else if (game.State == Enums.GameState.Active)
                    {
                        player.Surrender();
                    }
                }

                // TODO: CS: Game chat messages? 
                // TODO: CS: Game history?
            }
        }
    }
}
