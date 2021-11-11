using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ImperaPlus.Application.Visibility;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using ImperaPlus.DTO.Games;
using ImperaPlus.DTO.Games.Chat;
using ImperaPlus.DTO.Games.History;
using ImperaPlus.Utils;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.Application.Games
{
    public interface IGameService
    {
        GameSummary Create(GameCreationOptions creationOptions);

        void Delete(long gameId);

        void Join(long gameId, string password);

        void Leave(long gameId);

        GameSummary Surrender(long gameId);

        /// <summary>
        /// Hide game if player is inactive
        /// </summary>
        void Hide(long gameId);

        /// <summary>
        /// Hide all games where player is inactive
        /// </summary>
        IEnumerable<long> HideAll();

        Game Get(long id);

        HistoryTurn Get(long gameId, long turnNo);

        IEnumerable<GameSummary> GetOpen();

        IEnumerable<GameSummary> GetForCurrentUserReadOnly();

        IEnumerable<GameSummary> GetForCurrentUserTurn();

        /// <summary>
        /// Sends message to given game chat
        /// </summary>
        /// <param name="gameId">Id of game</param>
        /// <param name="text">Message text</param>
        /// <param name="isPublic">Value indicating whether message is to all players</param>
        /// <returns>Newly created message</returns>
        GameChatMessage SendMessage(long gameId, string text, bool isPublic);

        /// <summary>
        /// Returns messages for the given game
        /// </summary>
        /// <param name="gameId">Id of game</param>
        /// /// <param name="isPublic">Value indicating whether to return only public messages</param>
        /// <returns>List of messages, contains both public and team messages</returns>
        IEnumerable<GameChatMessage> GetMessages(long gameId, bool isPublic);
    }

    public class GameService : BaseGameService, IGameService
    {
        private readonly Domain.Services.IGameService gameService;
        private readonly IRandomGenProvider randomGenProvider;

        public GameService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            Domain.IUserProvider userProvider,
            Domain.Services.IGameService gameService,
            IMapTemplateProvider mapTemplateProvider,
            IVisibilityModifierFactory visibilityModifierFactory,
            IRandomGenProvider randomGenProvider)
            : base(unitOfWork, mapper, userProvider, mapTemplateProvider, visibilityModifierFactory)
        {
            this.gameService = gameService;
            this.randomGenProvider = randomGenProvider;
        }

        public GameSummary Create(GameCreationOptions creationOptions)
        {
            var user = CurrentUser;

            var mapTemplate = mapTemplateProvider.GetTemplate(creationOptions.MapTemplate);
            if (mapTemplate == null)
            {
                throw new Exceptions.ApplicationException("Cannot find map template", ErrorCode.CannotFindMapTemplate);
            }

            if (creationOptions.VictoryConditions == null || !creationOptions.VictoryConditions.Any())
            {
                throw new Exceptions.ApplicationException("VictoryConditions are required",
                    ErrorCode.GenericApplicationError);
            }

            if (creationOptions.VisibilityModifier == null || !creationOptions.VisibilityModifier.Any())
            {
                throw new Exceptions.ApplicationException("VisibilityModifier are required",
                    ErrorCode.GenericApplicationError);
            }

            if (creationOptions.TimeoutInSeconds < 5 * 60)
            {
                throw new Exceptions.ApplicationException("Timouts has to be at least 300 seconds",
                    ErrorCode.GenericApplicationError);
            }

            var password = creationOptions.Password;
            if (password != null)
            {
                // Trim any whitespace off the password and optionall convert to null, disabling it
                password = password.Trim();
                if (string.IsNullOrWhiteSpace(password))
                {
                    password = null;
                }
            }

            var game = gameService.Create(
                Domain.Enums.GameType.Fun,
                user,
                creationOptions.Name,
                password,
                creationOptions.TimeoutInSeconds,
                mapTemplate.Name,
                creationOptions.NumberOfPlayersPerTeam,
                creationOptions.NumberOfTeams,
                Mapper.Map<IEnumerable<Domain.Enums.VictoryConditionType>>(creationOptions.VictoryConditions),
                Mapper.Map<IEnumerable<Domain.Enums.VisibilityModifierType>>(creationOptions.VisibilityModifier));

            game.Options.MapDistribution = Mapper.Map<Domain.Enums.MapDistribution>(creationOptions.MapDistribution);

            game.Options.AttacksPerTurn = creationOptions.AttacksPerTurn;
            game.Options.MovesPerTurn = creationOptions.MovesPerTurn;
            game.Options.InitialCountryUnits = creationOptions.InitialCountryUnits;
            game.Options.MinUnitsPerCountry = creationOptions.MinUnitsPerCountry;
            game.Options.NewUnitsPerTurn = creationOptions.NewUnitsPerTurn;

            game.Options.MaximumNumberOfCards = creationOptions.MaximumNumberOfCards;

            // Add player, use the given password
            game.AddPlayer(user, password);

            if (creationOptions.AddBot)
            {
                using (TraceContext.Trace("Add Bot"))
                {
                    var botUser = UnitOfWork.Users.Query().First(x => x.UserName == Constants.BotName);
                    game.AddPlayer(botUser, password);
                }
            }

            UnitOfWork.Games.Add(game);

            if (game.CanStart)
            {
                using (TraceContext.Trace("Start Game"))
                {
                    game.Start(mapTemplate, randomGenProvider.GetRandomGen());
                }
            }

            using (TraceContext.Trace("Save Game"))
            {
                UnitOfWork.Commit();
            }

            return Mapper.Map<GameSummary>(game);
        }

        public void Delete(long gameId)
        {
            gameService.Delete(CurrentUser, gameId);

            UnitOfWork.Commit();
        }

        public void Join(long gameId, string password)
        {
            var game = GetGame(gameId);
            var user = CurrentUser;

            game.AddPlayer(user, password);

            // Ensure all Ids are generated
            UnitOfWork.Commit();

            var mapTemplate = mapTemplateProvider.GetTemplate(game.MapTemplateName);

            if (game.CanStart)
            {
                game.Start(mapTemplate, randomGenProvider.GetRandomGen());
            }

            UnitOfWork.Commit();
        }

        public void Leave(long gameId)
        {
            var game = GetGame(gameId);
            var user = CurrentUser;

            game.Leave(user);

            UnitOfWork.Commit();
        }

        public GameSummary Surrender(long gameId)
        {
            var game = GetGame(gameId);
            var user = CurrentUser;

            var player = game.GetPlayerForUser(user.Id);
            player.Surrender();

            UnitOfWork.Commit();

            return Mapper.Map<GameSummary>(game);
        }

        public void Hide(long gameId)
        {
            var game = GetGame(gameId);

            var user = CurrentUser;

            var player = game.GetPlayerForUser(user.Id);
            player.Hide();

            UnitOfWork.Commit();
        }

        public IEnumerable<long> HideAll()
        {
            var hiddenGameIds = new List<long>();

            var games = UnitOfWork.Games.FindNotHiddenNotOutcomeForUser(CurrentUserId, Domain.Enums.PlayerOutcome.None);
            foreach (var game in games)
            {
                var player = game.GetPlayerForUser(CurrentUserId);
                player.Hide();

                hiddenGameIds.Add(game.Id);
            }

            UnitOfWork.Commit();

            return hiddenGameIds;
        }

        public Game Get(long gameId)
        {
            var game = GetGame(gameId);

            return MapAndApplyModifiers(game);
        }

        public HistoryTurn Get(long gameId, long turnNo)
        {
            var game = GetGameWithHistory(gameId, turnNo);

            var historyTurn = game.GameHistory.GetTurn(turnNo);

            Domain.Games.Map previousMap;
            if (turnNo > 0)
            {
                // Get previous turn's map
                previousMap = game.GameHistory.GetMapForTurn(turnNo - 1);
            }
            else
            {
                previousMap = historyTurn.Game.Map;
            }

            return MapAndApplyModifiers(historyTurn, previousMap);
        }

        public IEnumerable<GameSummary> GetOpen()
        {
            var userId = CurrentUserId;

            return Mapper.Map<IEnumerable<GameSummary>>(UnitOfWork.Games.FindOpen(userId));
        }

        public IEnumerable<GameSummary> GetForCurrentUserReadOnly()
        {
            var userId = CurrentUserId;

            return Mapper.Map<IEnumerable<GameSummary>>(UnitOfWork.Games.FindForUser(userId));
        }

        public IEnumerable<GameSummary> GetForCurrentUserTurn()
        {
            var userId = CurrentUserId;

            return Mapper.Map<IEnumerable<GameSummary>>(UnitOfWork.Games.FindForUserAtTurnReadOnly(userId));
        }

        public GameChatMessage SendMessage(long gameId, string text, bool isPublic)
        {
            var user = CurrentUser;
            var game = GetGame(gameId);

            var message = game.PostMessage(user, text, isPublic);

            UnitOfWork.Commit();

            return Mapper.Map<GameChatMessage>(message);
        }

        public IEnumerable<GameChatMessage> GetMessages(long gameId, bool isPublic)
        {
            var user = CurrentUser;
            var messages = UnitOfWork.Games.GetGameMessages(gameId, isPublic, user.Id);
            return Mapper.Map<IEnumerable<GameChatMessage>>(messages);
        }
    }
}
