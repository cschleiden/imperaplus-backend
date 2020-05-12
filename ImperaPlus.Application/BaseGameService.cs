using System.Linq;
using AutoMapper;
using ImperaPlus.Application.Exceptions;
using ImperaPlus.Application.Visibility;
using ImperaPlus.Domain;
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
            IMapper mapper,
            IUserProvider userProvider,
            IMapTemplateProvider mapTemplateProvider,
            IVisibilityModifierFactory visibilityModifierFactory)
            : base(unitOfWork, mapper, userProvider)
        {
            this.mapTemplateProvider = mapTemplateProvider;
            this.visibilityModifierFactory = visibilityModifierFactory;
        }

        protected Game GetGame(long gameId)
        {
            var game = this.UnitOfWork.Games.Find(gameId);
            if (game == null)
            {
                throw new ApplicationException("Cannot find game", ErrorCode.CannotFindGame);
            }

            game.ResetMapTracking();

            return game;
        }

        protected Game GetGameWithHistory(long gameId, long turnNo)
        {
            var game = this.UnitOfWork.Games.FindWithHistory(gameId, turnNo);
            if (game == null)
            {
                throw new ApplicationException("Cannot find game", ErrorCode.CannotFindGame);
            }

            return game;
        }

        protected DTO.Games.Game MapAndApplyModifiers(Game game)
        {
            var currentUserId = this.CurrentUserId;

            var mappedGame = Mapper.Map<DTO.Games.Game>(game, this.GetMapperOptions());

            // Apply visibility modifications
            foreach (var visibilityModifier in game.Options.VisibilityModifier)
            {
                var visibilityModifierInstance = this.visibilityModifierFactory.Construct(visibilityModifier);

                visibilityModifierInstance.Apply(this.CurrentUser, mappedGame);
            }

            // Only send information for current player
            if (game.CurrentPlayer != null && game.CurrentPlayer.UserId == this.CurrentUserId)
            {
                mappedGame.UnitsToPlace = game.GetUnitsToPlace(this.mapTemplateProvider.GetTemplate(game.MapTemplateName), game.CurrentPlayer);
            }

            return mappedGame;
        }

        protected ImperaPlus.DTO.Games.History.HistoryTurn MapAndApplyModifiers(HistoryGameTurn turn, Map previousTurnMap)
        {
            var mappedTurn = Mapper.Map<DTO.Games.History.HistoryTurn>(turn, this.GetMapperOptions());
            var mappedMap = Mapper.Map<DTO.Games.Map.Map>(previousTurnMap, this.GetMapperOptions());

            // Apply visibility modifications
            foreach (var visibilityModifier in turn.Game.Options.VisibilityModifier)
            {
                var visibilityModifierInstance = this.visibilityModifierFactory.Construct(visibilityModifier);

                visibilityModifierInstance.Apply(this.CurrentUser, mappedTurn, mappedMap);
            }

            // Fix game stats, we want them to reflect the history turn's state, not the current game's turn
            foreach (var team in mappedTurn.Game.Teams)
            {
                foreach (var player in team.Players)
                {
                    var countriesForPlayerInTurnMap = previousTurnMap.GetCountriesForPlayer(player.Id);
                    player.NumberOfCountries = countriesForPlayerInTurnMap.Count();
                    player.NumberOfUnits = countriesForPlayerInTurnMap.Sum(c => c.Units);
                }
            }

            return mappedTurn;
        }

        private System.Action<IMappingOperationOptions> GetMapperOptions()
        {
            return opts =>
            {
                opts.Items.Add("userId", this.CurrentUserId);
            };
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
