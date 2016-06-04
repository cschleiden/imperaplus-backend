using AutoMapper;
using ImperaPlus.Application.Visibility;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Games.History;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.Application
{
    public class BaseGameService : BaseService
    {
        protected readonly IVisibilityModifierFactory visibilityModifierFactory;

        public BaseGameService(IUnitOfWork unitOfWork, IUserProvider userProvider, IVisibilityModifierFactory visibilityModifierFactory)
            : base(unitOfWork, userProvider)
        {
            this.visibilityModifierFactory = visibilityModifierFactory;
        }

        protected DTO.Games.Game MapAndApplyModifiers(Game game)
        {
            var mappedGame = Mapper.Map<DTO.Games.Game>(game);

            // Apply visibility modifications
            foreach (var visibilityModifier in game.Options.VisibilityModifier)
            {
                var visibilityModifierInstance = this.visibilityModifierFactory.Construct(visibilityModifier);

                visibilityModifierInstance.Apply(this.CurrentUser, mappedGame);
            }

            if (game.CurrentPlayer != null)
            {
                if (game.CurrentPlayer.UserId == this.userProvider.GetCurrentUserId())
                {
                    // Show extended information
                    mappedGame.CurrentPlayer = Mapper.Map<DTO.Games.Player>(game.CurrentPlayer);
                }
                else
                {
                    mappedGame.CurrentPlayer = Mapper.Map<DTO.Games.PlayerSummary>(game.CurrentPlayer);
                }
            }

            return mappedGame;
        }

        protected ImperaPlus.DTO.Games.History.HistoryTurn MapAndApplyModifiers(HistoryGameTurn turn, Map previousTurnMap)
        {
            var mappedTurn = Mapper.Map<DTO.Games.History.HistoryTurn>(turn);

            var mappedMap = Mapper.Map<DTO.Games.Map.Map>(previousTurnMap);

            // Apply visibility modifications
            foreach (var visibilityModifier in turn.Game.Options.VisibilityModifier)
            {
                var visibilityModifierInstance = this.visibilityModifierFactory.Construct(visibilityModifier);

                visibilityModifierInstance.Apply(this.CurrentUser, mappedTurn, mappedMap);
            }

            return mappedTurn;
        }

        protected string CurrentUserId
        {
            get
            {
                return this.userProvider.GetCurrentUserId();
            }
        }

        protected User CurrentUser
        {
            get
            {
                // TODO: Move to user provider
                return this.UnitOfWork.Users.FindById(this.userProvider.GetCurrentUserId());
            }
        }        
    }
}
