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

        [TestMethod]
        public void LeaveGameShouldReorderPlayOrder()
        {
            // Arrange
            var game = TestUtils.CreateGame(3, 1);
            var user1 = TestUtils.CreateUser("user1");
            var user2 = TestUtils.CreateUser("user2");
            var user3 = TestUtils.CreateUser("user3");
            
            game.AddPlayer(user1);
            game.AddPlayer(user2);
            game.AddPlayer(user3);

            // Verify initial PlayOrder
            var allPlayers = game.Teams.SelectMany(x => x.Players).OrderBy(p => p.PlayOrder).ToList();
            Assert.AreEqual(3, allPlayers.Count);
            Assert.AreEqual(0, allPlayers[0].PlayOrder);
            Assert.AreEqual(1, allPlayers[1].PlayOrder);
            Assert.AreEqual(2, allPlayers[2].PlayOrder);

            // Act - remove middle player
            game.Leave(user2);

            // Assert - PlayOrder should be sequential
            var remainingPlayers = game.Teams.SelectMany(x => x.Players).OrderBy(p => p.PlayOrder).ToList();
            Assert.AreEqual(2, remainingPlayers.Count);
            Assert.AreEqual(0, remainingPlayers[0].PlayOrder, "First player should have PlayOrder 0");
            Assert.AreEqual(1, remainingPlayers[1].PlayOrder, "Second player should have PlayOrder 1");
            
            // Verify team PlayOrder is also sequential
            var remainingTeams = game.Teams.OrderBy(t => t.PlayOrder).ToList();
            Assert.AreEqual(2, remainingTeams.Count);
            Assert.AreEqual(0, remainingTeams[0].PlayOrder, "First team should have PlayOrder 0");
            Assert.AreEqual(1, remainingTeams[1].PlayOrder, "Second team should have PlayOrder 1");
        }

        [TestMethod]
        public void LeaveGameFirstPlayerShouldReorderPlayOrder()
        {
            // Arrange
            var game = TestUtils.CreateGame(3, 1);
            var user1 = TestUtils.CreateUser("user1");
            var user2 = TestUtils.CreateUser("user2");
            var user3 = TestUtils.CreateUser("user3");
            
            game.AddPlayer(user1);
            game.AddPlayer(user2);
            game.AddPlayer(user3);

            // Act - remove first player
            game.Leave(user1);

            // Assert - PlayOrder should be sequential starting from 0
            var remainingPlayers = game.Teams.SelectMany(x => x.Players).OrderBy(p => p.PlayOrder).ToList();
            Assert.AreEqual(2, remainingPlayers.Count);
            Assert.AreEqual(0, remainingPlayers[0].PlayOrder, "First remaining player should have PlayOrder 0");
            Assert.AreEqual(1, remainingPlayers[1].PlayOrder, "Second remaining player should have PlayOrder 1");
        }

        [TestMethod]
        public void LeaveGameLastPlayerShouldReorderPlayOrder()
        {
            // Arrange
            var game = TestUtils.CreateGame(3, 1);
            var user1 = TestUtils.CreateUser("user1");
            var user2 = TestUtils.CreateUser("user2");
            var user3 = TestUtils.CreateUser("user3");
            
            game.AddPlayer(user1);
            game.AddPlayer(user2);
            game.AddPlayer(user3);

            // Act - remove last player
            game.Leave(user3);

            // Assert - PlayOrder should be sequential
            var remainingPlayers = game.Teams.SelectMany(x => x.Players).OrderBy(p => p.PlayOrder).ToList();
            Assert.AreEqual(2, remainingPlayers.Count);
            Assert.AreEqual(0, remainingPlayers[0].PlayOrder);
            Assert.AreEqual(1, remainingPlayers[1].PlayOrder);
        }
    }
}
