using AutoMapper;
using ImperaPlus.Application.Exceptions;
using ImperaPlus.Application.Visibility;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Games.History;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Application
{
    public class BaseGameService : BaseService
    {
        protected readonly IVisibilityModifierFactory visibilityModifierFactory;
        protected readonly IMapTemplateProvider mapTemplateProvider;

        public BaseGameService(
            IUnitOfWork unitOfWork, 
            IUserProvider userProvider,
            IMapTemplateProvider mapTemplateProvider,
            IVisibilityModifierFactory visibilityModifierFactory)
            : base(unitOfWork, userProvider)
        {
            this.mapTemplateProvider = mapTemplateProvider;
            this.visibilityModifierFactory = visibilityModifierFactory;
        }

        protected Game GetGame(long gameId)
        {
            var game = this.UnitOfWork.Games.Find(gameId);
            if (game == null)
            {
                throw new Exceptions.ApplicationException("Cannot find game", ErrorCode.CannotFindGame);
            }        

            return game;
        }

        protected Game GetGameWithHistory(long gameId)
        {
            var game = this.UnitOfWork.Games.FindWithHistory(gameId);
            if (game == null)
            {
                throw new ApplicationException("Cannot find game", ErrorCode.CannotFindGame);
            }

            return game;
        }

        protected Game GetGameMessages(long gameId)
        {
            var game = this.UnitOfWork.Games.FindWithMessages(gameId);
            if (game == null)
            {
                throw new ApplicationException("Cannot find game", ErrorCode.CannotFindGame);
            }

            return game;
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

            mappedGame.UnitsToPlace = game.GetUnitsToPlace(this.mapTemplateProvider.GetTemplate(game.MapTemplateName), game.CurrentPlayer);

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
    }
}
