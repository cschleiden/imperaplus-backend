using Autofac;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Repositories;
using System.Collections.Generic;

namespace ImperaPlus.Domain.Services
{
    public interface IGameService
    {
        Game Create(
            GameType type,
            User user, 
            string name, 
            int timeoutInSeconds,
            string mapTemplate, 
            int numberOfPlayersPerTeam, 
            int numberOfTeams, 
            IEnumerable<VictoryConditionType> victoryConditions, 
            IEnumerable<VisibilityModifierType> visibilityModifier);

        Game Create(
            GameType type,
            User user,
            string name,
            string mapTemplate,
            GameOptions options);
    }

    public class GameService : IGameService
    {
        private readonly IComponentContext componentContext;
        private readonly IGameRepository gameRepository;

        public GameService(IComponentContext componentContext, IUnitOfWork unitOfWork)
        {
            this.componentContext = componentContext;
            this.gameRepository = unitOfWork.Games;
        }

        public Game Create(
            GameType type,
            User user,
            string name,
            int timeoutInSeconds,
            string mapTemplate, 
            int numberOfPlayersPerTeam, 
            int numberOfTeams, 
            IEnumerable<VictoryConditionType> victoryConditions,
            IEnumerable<VisibilityModifierType> visibilityModifier)
        {
            // Check if user is allowed to create games
            if (!user.CanCreateGame)
            {
                throw new DomainException(ErrorCode.NotEnoughSlots, "User does not have enough available slots to create game");
            }

            // Check if name is not already taken
            if (this.gameRepository.FindByName(name) != null)
            {
                throw new DomainException(ErrorCode.NameAlreadyTaken, "Name for this game is already taken");
            }

            // Create game
            var game = new Game(user, type, name, mapTemplate, timeoutInSeconds, numberOfTeams, numberOfPlayersPerTeam, victoryConditions, visibilityModifier);
            this.componentContext.InjectProperties(game);

            return game;
        }

        public Game Create(
            GameType type,
            User user,
            string name,
            string mapTemplate,
            GameOptions options)
        {
            // Check if user is allowed to create games
            if (!user.CanCreateGame)
            {
                throw new DomainException(ErrorCode.NotEnoughSlots, "User does not have enough available slots to create game");
            }

            // Check if name is not already taken
            if (this.gameRepository.FindByName(name) != null)
            {
                throw new DomainException(ErrorCode.NameAlreadyTaken, "Name for this game is already taken");
            }

            // Create game
            var game = new Game(user, type, name, mapTemplate, options);
            this.componentContext.InjectProperties(game);

            return game;
        }
    }
}
