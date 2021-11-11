using ImperaPlus.Application.Play;
using ImperaPlus.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
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
            playService = Scope.Resolve<IPlayService>();
            gameService = Scope.Resolve<IGameService>();
        }

        [TestMethod]
        public void PlaceUnitsSucceeds()
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
                TimeoutInSeconds = 5 * 60,
                AddBot = true
            };
            UnitOfWork.Commit();

            var game = gameService.Create(gameCreationOptions);
            UnitOfWork.Commit();

            var fullGame = gameService.Get(game.Id);

            if (fullGame.CurrentPlayer.Name == "Bot")
            {
                Assert.Inconclusive();
                return;
            }

            TestUserProvider.User = UnitOfWork.Users.FindById(fullGame.CurrentPlayer.UserId);

            // Act
            var actionResult = playService.Place(game.Id,
                new[]
                {
                    new PlaceUnitsOptions
                    {
                        CountryIdentifier =
                            fullGame.Map.Countries.First(x => x.PlayerId == fullGame.CurrentPlayer.Id)
                                .Identifier,
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
