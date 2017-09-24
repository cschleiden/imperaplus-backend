using ImperaPlus.Application.Play;
using ImperaPlus.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ImperaPlus.Application.Games;
using ImperaPlus.DTO.Games;
using ImperaPlus.DTO.Games.Play;

namespace ImperaPlus.Application.Tests.Play
{
    [TestClass]
    public class PlayServiceTests : TestBase
    {
        private IPlayService playService;
        private IGameService gameService;

        [TestInitialize]
        public void Setup()
        {
            this.playService = this.Scope.Resolve<IPlayService>();
            this.gameService = this.Scope.Resolve<IGameService>();
        }

        [TestMethod]
        public void PlaceUnitsSucceeds()
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
                TimeoutInSeconds = 5 * 60,
                AddBot = true
            };
            this.UnitOfWork.Commit();

            var game = this.gameService.Create(gameCreationOptions);
            this.UnitOfWork.Commit();

            var fullGame = this.gameService.Get(game.Id);

            if (fullGame.CurrentPlayer.Name == "Bot")
            {
                Assert.Inconclusive();
                return;
            }

            TestUserProvider.User = this.UnitOfWork.Users.FindById(fullGame.CurrentPlayer.UserId);

            // Act
            var actionResult = this.playService.Place(game.Id, new[]
            {
                new PlaceUnitsOptions
                {
                    CountryIdentifier = fullGame.Map.Countries.First(x => x.PlayerId == fullGame.CurrentPlayer.Id).Identifier,
                    NumberOfUnits = fullGame.UnitsToPlace
                }
            });

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.CountryUpdates);
            Assert.AreEqual(1, actionResult.CountryUpdates.Count());
        }
    }
}
