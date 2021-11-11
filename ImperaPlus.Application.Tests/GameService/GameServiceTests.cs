using System.Linq;
using Autofac;
using ImperaPlus.Application.Games;
using ImperaPlus.DTO.Games;
using ImperaPlus.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.Application.Tests.GameService
{
    [TestClass]
    public class GameServiceTests : TestBase
    {
        private IGameService gameService;

        [TestInitialize]
        public void Setup()
        {
            gameService = Scope.Resolve<IGameService>();
        }

        [TestMethod]
        public void CreateGameShouldSucceed()
        {
            // Arrange
            var mapTemplate = TestData.CreateAndSaveMapTemplate();
            var gameCreationOptions = new GameCreationOptions
            {
                Name = "TestGame",
                MapTemplate = mapTemplate.Name,
                NumberOfPlayersPerTeam = 1,
                NumberOfTeams = 2,
                VictoryConditions = new[] { VictoryConditionType.Survival },
                VisibilityModifier = new[] { VisibilityModifierType.None },
                TimeoutInSeconds = 600
            };
            UnitOfWork.Commit();

            // Act
            var game = gameService.Create(gameCreationOptions);
            var openGames = gameService.GetOpen();

            // Assert
            Assert.IsNotNull(game);
            Assert.IsNotNull(openGames);
            var dbGame = UnitOfWork.Games.Find(game.Id);
            Assert.IsNotNull(dbGame);
            Assert.AreEqual(dbGame.Id, game.Id);
            Assert.IsTrue(dbGame.Teams.SelectMany(x => x.Players).Any(x => x.UserId == TestUser.Id));
        }

        [TestMethod]
        public void CreateGameWithBotShouldSucceed()
        {
            // Arrange
            var mapTemplate = TestData.CreateAndSaveMapTemplate();
            var gameCreationOptions = new GameCreationOptions
            {
                Name = "TestGame",
                MapTemplate = mapTemplate.Name,
                NumberOfPlayersPerTeam = 1,
                NumberOfTeams = 2,
                VictoryConditions = new[] { VictoryConditionType.Survival },
                VisibilityModifier = new[] { VisibilityModifierType.None },
                TimeoutInSeconds = 600,
                AddBot = true
            };
            UnitOfWork.Commit();

            // Act
            var game = gameService.Create(gameCreationOptions);

            // Assert
            Assert.IsNotNull(game);
            Assert.AreEqual(GameState.Active, game.State);
            Assert.IsTrue(game.Teams.SelectMany(x => x.Players).Any(x => x.Name == Constants.BotName));
        }

        [TestMethod]
        public void GetHistoryTurn()
        {
            // Act
            var historyTurn = CreateGameAndGetHistoryTurn(0);

            // Assert
            Assert.IsNotNull(historyTurn);
        }

        [TestMethod]
        [ExpectedException(typeof(Domain.Exceptions.DomainException))]
        public void GetHistoryTurnInvalidTurnId()
        {
            CreateGameAndGetHistoryTurn(100);
        }

        [TestMethod]
        public void HideAllGamesSucceed()
        {
            // Arrange
            var game1 = CreateGame("a");
            gameService.Surrender(game1.Id);

            var game2 = CreateGame("b");
            gameService.Surrender(game2.Id);

            // Act
            var hiddenGameIds = gameService.HideAll().ToArray();

            // Assert
            Assert.IsNotNull(hiddenGameIds);
            Assert.AreEqual(game1.Id, hiddenGameIds[0]);
            Assert.AreEqual(game2.Id, hiddenGameIds[1]);
        }

        private DTO.Games.History.HistoryTurn CreateGameAndGetHistoryTurn(long turnNo)
        {
            var game = CreateGame();

            return gameService.Get(game.Id, turnNo);
        }

        private GameSummary CreateGame(string suffix = null)
        {
            var mapTemplate = TestData.CreateAndSaveMapTemplate();
            UnitOfWork.Commit();

            var gameCreationOptions = new GameCreationOptions
            {
                Name = "TestGame" + (suffix ?? string.Empty),
                MapTemplate = mapTemplate.Name,
                NumberOfPlayersPerTeam = 1,
                NumberOfTeams = 2,
                VictoryConditions = new[] { VictoryConditionType.Survival },
                VisibilityModifier = new[] { VisibilityModifierType.None },
                TimeoutInSeconds = 5 * 60,
                AddBot = true
            };

            var game = gameService.Create(gameCreationOptions);
            return game;
        }
    }
}
