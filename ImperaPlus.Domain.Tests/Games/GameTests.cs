using System.Linq;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Tests.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.Domain.Tests.Games
{
    [TestClass]
    public class GameTests
    {       
        [TestMethod]
        public void CreateGameShouldSucceed()
        {
            // Arrange            

            // Act
            var game = TestUtils.CreateGame();

            // Assert
            Assert.AreEqual("NewGame", game.Name);
        }       

        [TestMethod]
        public void CanStartGame()
        {
            // Arrange
            var game = TestUtils.CreateGame(2, 1);
            var user1 = TestUtils.CreateUser("1");
            var user2 = TestUtils.CreateUser("2");

            // Assert
            Assert.IsFalse(game.CanStart);

            game.AddPlayer(user1);
            Assert.IsFalse(game.CanStart);

            game.AddPlayer(user2);
            Assert.IsTrue(game.CanStart);
        }

        [TestMethod]
        public void StartGame()
        {
            // Arrange
            var game = TestUtils.CreateGame();
            var user1 = TestUtils.CreateUser("1");
            var user2 = TestUtils.CreateUser("2");

            game.AddPlayer(user1);
            game.AddPlayer(user2);
            
            // Act
            game.Start(TestUtils.GetMapTemplate());

            // Assert
            Assert.AreEqual(GameState.Active, game.State);
            Assert.IsNotNull(game.CurrentPlayer);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void StartGameFailNotEnoughPlayers()
        {
            // Arrange
            var game = TestUtils.CreateGame();
            var user1 = TestUtils.CreateUser("1");

            game.AddPlayer(user1);

            // Act
            game.Start(TestUtils.GetMapTemplate());

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void StartGameWhenAlreadyStartedFail()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();

            // Act
            game.Start(TestUtils.GetMapTemplate());

            // Assert            
        }

        [TestMethod]
        public void StartGameCountriesDistributed()
        {
            // Arrange
            var game = TestUtils.CreateGameWithMapAndPlayers();

            // Act
            game.Start(TestUtils.GetMapTemplate());

            // Assert
            Assert.AreEqual(42, game.Map.Countries.Count(x => !x.IsNeutral));
        }

        [TestMethod]
        public void StartGameCountriesDistributedMalibu()
        {
            // Arrange
            var game = TestUtils.CreateGameWithMapAndPlayers();
            game.Options.MapDistribution = MapDistribution.Malibu;

            // Act
            game.Start(TestUtils.GetMapTemplate());

            // Assert
            Assert.AreEqual(game.Options.NumberOfPlayersPerTeam * game.Options.NumberOfTeams, game.Map.Countries.Count(x => !x.IsNeutral));
        }

        [TestMethod]
        public void JoinGameSuccess()
        {
            // Arrange
            var user = TestUtils.CreateUser("test");
            var game = TestUtils.CreateGame();

            // Act
            game.AddPlayer(user);
            
            // Assert
            Assert.IsTrue(game.Teams.SelectMany(x => x.Players).Select(x => x.User).Contains(user));
        }

        [TestMethod]
        public void LeaveGameSuccess()
        {
            // Arrange
            var user = TestUtils.CreateUser("test");
            var game = TestUtils.CreateGame();

            game.AddPlayer(user);

            // Act
            game.Leave(user);
            
            // Assert
            Assert.IsFalse(game.Teams.SelectMany(x => x.Players).Select(x => x.User).Contains(user));
        }

        [TestMethod]
        public void JoinGameMultipleTeamsSuccess()
        {
            // Arrange
            var game = TestUtils.CreateGame(2, 2);

            // Act
            for(int i = 0; i < 4; ++i)
            {
                var user = TestUtils.CreateUser("test" + i);
                game.AddPlayer(user);
            }

            // Assert
            Assert.IsTrue(game.CanStart);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void JoinGameTooManyPlayers()
        {
            // Arrange
            var user1 = TestUtils.CreateUser("user1");
            var user2 = TestUtils.CreateUser("user2");
            var user3 = TestUtils.CreateUser("user3");
            var game = TestUtils.CreateGame(2, 1);

            // Act
            game.AddPlayer(user1);
            game.AddPlayer(user2);
            game.AddPlayer(user3);  

            // Assert
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.CannotLeaveGame)]
        public void LeaveGameAlreadyStarted()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();
            var player = game.Teams.First().Players.First();

            // Act
            game.Leave(player.User);

            // Assert
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.TeamAlreadyFull)]
        public void JoinGameTooManyPlayersInTeam()
        {
            // Arrange
            var user1 = TestUtils.CreateUser("user1");
            var user2 = TestUtils.CreateUser("user2");
            var game = TestUtils.CreateGame(2, 1);

            // Act
            var team = game.AddTeam();

            team.AddPlayer(user1);
            team.AddPlayer(user2);
        }

        [TestMethod]
        public void GameOptionsSaveAndRestore()
        {
            var game = TestUtils.CreateGame();

            // Act
            game.Options.MinUnitsPerCountry = 5;

            // Assert
            Assert.AreEqual(5, game.Options.MinUnitsPerCountry);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void GameChatUserNotInGame()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();

            // Act
            game.PostMessage(TestUtils.CreateUser("NotInGame"), "TestMessage", true);
        }


        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void GameChatNotStarted()
        {
            // Arrange
            var game = TestUtils.CreateGameWithMapAndPlayers();

            // Act
            game.PostMessage(game.Teams.First().Players.First().User, "TestMessage", true);
        }

        [TestMethod]
        public void GameChatPostPublicMessage()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();
            var player = game.Teams.First().Players.First();
            var team = player.Team;
            var user = player.User;

            // Act
            game.PostMessage(user, "TestMessage", true);

            // Assert
            var messages = game.GetMessages(user, true);
            
            Assert.AreEqual(1, messages.Count());

            var message = messages.First();
            Assert.AreEqual(message.Text, "TestMessage");
            Assert.IsNull(message.Team);
            Assert.IsNull(message.TeamId);
        }

        [TestMethod]
        public void GameChatPostPrivateMessage()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();
            var player = game.Teams.First().Players.First();
            var team = player.Team;
            var user = player.User;

            // Act
            game.PostMessage(user, "TestMessage", false);

            // Assert
            Assert.AreEqual(1, game.ChatMessages.Count());
            Assert.AreEqual(game.ChatMessages.First().Text, "TestMessage");
            Assert.IsNotNull(game.ChatMessages.First().Team);
        }

        [TestMethod]
        public void SurrenderGameSuccess()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();
            var player = game.Teams.First().Players.First();

            // Act 
            player.Surrender();

            // Assert
            Assert.AreEqual(GameState.Ended, game.State);

            Assert.AreEqual(PlayerState.InActive, player.State);
            Assert.AreEqual(PlayerOutcome.Surrendered, player.Outcome);

            var otherPlayer = game.Teams.Last().Players.First();
            Assert.AreEqual(PlayerOutcome.Won, otherPlayer.Outcome);
        }

        [TestMethod]
        public void SurrenderGameWhenPlayerIsCurrentPlayerSuccess()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();
            var player = game.CurrentPlayer;

            var ownedCountry = player.Countries.First();

            // Act 
            player.Surrender();

            // Assert
            Assert.AreEqual(GameState.Ended, game.State);

            Assert.AreEqual(PlayerState.InActive, player.State);
            Assert.AreEqual(PlayerOutcome.Surrendered, player.Outcome);

            Assert.IsTrue(ownedCountry.IsNeutral);
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.CannotSurrender)]
        public void SurrenderGamePlayerAlreadyInActive()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();
            var player = game.Teams.First().Players.First();
            player.Surrender();

            // Act 
            player.Surrender();
        }

        [TestMethod]
        public void HideGameSuccess()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();
            var player = game.Teams.First().Players.First();
            player.Surrender();

            // Act
            player.Hide();

            // Assert
            Assert.IsTrue(player.IsHidden, "Game is not hidden for player");
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void HideGameAlreadyHidden()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();
            var player = game.Teams.First().Players.First();
            player.Hide();

            // Act
            player.Hide();
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.CannotHideGame)]
        public void HideGameActive()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();
            var player = game.Teams.First().Players.First();

            // Act
            player.Hide();
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.CannotHideGame)]
        public void HideGameNotStarted()
        {
            // Arrange
            var game = TestUtils.CreateGameWithMapAndPlayers();
            var player = game.Teams.First().Players.First();

            // Act
            player.Hide();
        }
    }
}
