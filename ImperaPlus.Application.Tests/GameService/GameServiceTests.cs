using System;
using System.Linq;
using Autofac;
using ImperaPlus.Application.Games;
using ImperaPlus.DataAccess;
using ImperaPlus.DTO.Games;
using ImperaPlus.TestSupport;
using ImperaPlus.TestSupport.Testdata;
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
            this.gameService = this.Scope.Resolve<IGameService>();
        }

        [TestMethod]
        [LayerApplication]
        public void CreateGameShouldSucceed()
        {
            // Arrange
            var mapTemplate = this.TestData.CreateAndSaveMapTemplate();
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
            this.UnitOfWork.Commit();

            // Act
            var game = this.gameService.Create(gameCreationOptions);
            var openGames = this.gameService.GetOpen();

            // Assert
            Assert.IsNotNull(game);
            Assert.IsNotNull(openGames);
            var dbGame = this.UnitOfWork.Games.FindById(game.Id);
            Assert.IsNotNull(dbGame);
            Assert.AreEqual(dbGame.Id, game.Id);
            Assert.IsTrue(dbGame.Teams.SelectMany(x => x.Players).Any(x => x.UserId == this.TestUser.Id));
        }

        [TestMethod]
        [LayerApplication]
        public void CreateGameWithBotShouldSucceed()
        {
            // Arrange
            var mapTemplate = this.TestData.CreateAndSaveMapTemplate();
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
            this.UnitOfWork.Commit();

            // Act
            var game = this.gameService.Create(gameCreationOptions);

            // Assert
            Assert.IsNotNull(game);
            Assert.AreEqual(GameState.Active, game.State);
            Assert.IsTrue(game.Teams.SelectMany(x => x.Players).Any(x => x.Name == Constants.BotName));
        }

        [TestMethod]
        [LayerApplication]
        public void GetHistoryTurn()
        {
            // Act
            var historyTurn = this.CreateGameAndGetHistoryTurn(0);

            // Assert
            Assert.IsNotNull(historyTurn);
        }

        [TestMethod]
        [LayerApplication]
        [ExpectedException(typeof(Domain.Exceptions.DomainException))]
        public void GetHistoryTurnInvalidTurnId()
        {
            this.CreateGameAndGetHistoryTurn(100);
        }
        
        [TestMethod]
        [LayerApplication]
        public void HideAllGamesSucceed()
        {
            // Arrange
            var game1 = this.CreateGame("a");
            this.gameService.Surrender(game1.Id);
            
            var game2 = this.CreateGame("b");
            this.gameService.Surrender(game2.Id);

            // Act
            var hiddenGameIds = this.gameService.HideAll().ToArray();

            // Assert
            Assert.IsNotNull(hiddenGameIds);
            Assert.AreEqual(game1.Id, hiddenGameIds[0]);
            Assert.AreEqual(game2.Id, hiddenGameIds[1]);
        }

        private DTO.Games.History.HistoryTurn CreateGameAndGetHistoryTurn(long turnNo)
        {
            var game = CreateGame();

            return this.gameService.Get(game.Id, turnNo);
        }

        private GameSummary CreateGame(string suffix = null)
        {
            var mapTemplate = this.TestData.CreateAndSaveMapTemplate();
            this.UnitOfWork.Commit();

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

            var game = this.gameService.Create(gameCreationOptions);            
            return game;
        }
    }
}
