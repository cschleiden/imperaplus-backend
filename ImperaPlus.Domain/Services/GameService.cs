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
            string password,
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
            string password,
            string mapTemplate,
            GameOptions options);

        void Delete(User currentUser, long gameId);
    }

    public class GameService : IGameService
    {
        private readonly IGameRepository gameRepository;

        public GameService(IUnitOfWork unitOfWork)
        {
            gameRepository = unitOfWork.Games;
        }

        public Game Create(
            GameType type,
            User user,
            string name,
            string password,
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
                throw new DomainException(ErrorCode.NotEnoughSlots,
                    "User does not have enough available slots to create game");
            }

            // Check if name is not already taken
            if (gameRepository.FindByName(name) != null)
            {
                throw new DomainException(ErrorCode.NameAlreadyTaken, "Name for this game is already taken");
            }

            // Create game
            return new Game(user, type, name, password, mapTemplate, timeoutInSeconds, numberOfTeams,
                numberOfPlayersPerTeam, victoryConditions, visibilityModifier);
        }

        public Game Create(
            GameType type,
            User user,
            string name,
            string password,
            string mapTemplate,
            GameOptions options)
        {
            // Check if user is allowed to create games
            if (!user.CanCreateGame)
            {
                throw new DomainException(ErrorCode.NotEnoughSlots,
                    "User does not have enough available slots to create game");
            }

            // Check if name is not already taken
            if (gameRepository.FindByName(name) != null)
            {
                throw new DomainException(ErrorCode.NameAlreadyTaken, "Name for this game is already taken");
            }

            // Create game
            return new Game(user, type, name, password, mapTemplate, options);
        }

        public void Delete(User user, long gameId)
        {
            var game = GetGame(gameId);

            if (game.CreatedBy != user)
            {
                throw new DomainException(ErrorCode.CannotDeleteGame, "User is not allowed to perform this action");
            }

            if (!game.CanBeDeleted)
            {
                throw new DomainException(ErrorCode.CannotDeleteGame, "Game cannot be delete");
            }

            gameRepository.Remove(game);
        }

        private Game GetGame(long gameId)
        {
            var game = gameRepository.Find(gameId);
            if (game == null)
            {
                throw new DomainException(ErrorCode.CannotFindGame, "Cannot find game");
            }

            return game;
        }
    }
}
