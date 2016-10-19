using AutoMapper;
using ImperaPlus.Application.Exceptions;
using ImperaPlus.Application.Visibility;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Events;
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
        protected readonly IEventAggregator eventAggregator;

        public BaseGameService(
            IUnitOfWork unitOfWork, 
            IUserProvider userProvider, 
            IVisibilityModifierFactory visibilityModifierFactory, 
            IMapTemplateProvider mapTemplateProvider,
            IEventAggregator eventAggregator)
            : base(unitOfWork, userProvider)
        {
            this.visibilityModifierFactory = visibilityModifierFactory;
            this.mapTemplateProvider = mapTemplateProvider;
            this.eventAggregator = eventAggregator;
        }

        protected Game GetGame(long gameId)
        {
            var game = this.UnitOfWork.Games.Find(gameId);
            if (game == null)
            {
                throw new ApplicationException("Cannot find game", ErrorCode.CannotFindGame);
            }

            game.EventAggregator = this.eventAggregator;
            game.MapTemplateProvider = this.mapTemplateProvider;

            return game;
        }

        protected Game GetGameWithHistory(long gameId)
        {
            var game = this.UnitOfWork.Games.FindWithHistory(gameId);
            if (game == null)
            {
                throw new ApplicationException("Cannot find game", ErrorCode.CannotFindGame);
            }

            game.EventAggregator = this.eventAggregator;
            game.MapTemplateProvider = this.mapTemplateProvider;

            return game;
        }

        protected Game GetGameMessages(long gameId)
        {
            var game = this.UnitOfWork.Games.FindWithMessages(gameId);
            if (game == null)
            {
                throw new ApplicationException("Cannot find game", ErrorCode.CannotFindGame);
            }

            game.EventAggregator = this.eventAggregator;
            game.MapTemplateProvider = this.mapTemplateProvider;

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
