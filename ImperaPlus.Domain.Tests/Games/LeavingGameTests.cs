using ImperaPlus.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ImperaPlus.Domain.Tests.Games
{
    [TestClass]
    public class LeavingGameTests
    {
        [TestMethod]
        public void LeaveGameSuccess()
        {
            // Arrange
            var game = TestUtils.CreateGame();
            var user = TestUtils.CreateUser("test");
            var player = game.AddPlayer(user);

            // Act
            game.Leave(user);

            // Assert
            Assert.IsTrue(game.Teams.All(x => !x.Players.Any()));
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void LeaveGameAfterStart()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();
            var player = game.CurrentPlayer;

            // Act
            game.Leave(player.User);
        }
    }
}
