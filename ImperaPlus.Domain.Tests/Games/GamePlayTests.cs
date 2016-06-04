using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImperaPlus.Domain.Services;
using System.Diagnostics;
using ImperaPlus.Domain.Tests.Helper;

namespace ImperaPlus.Domain.Tests.Games
{
    [TestClass]
    public class GamePlayTests
    {
        [TestMethod]
        public void StartGameAndCreateMap()
        {
            var game = TestUtils.CreateGameWithMapAndPlayers();

            // Act
            game.Start();

            // Assert
            Assert.IsNotNull(game.Map);
            Assert.IsTrue(game.Map.Countries.Any());
        }

        [TestMethod]
        public void EndTurn()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();

            var currentPlayer = game.CurrentPlayer;
            var nextPlayer = game.Teams.SelectMany(x => x.Players).First(x => x.PlayOrder == (currentPlayer.PlayOrder + 1) % game.Options.PlayerCount);

            // Act
            game.EndTurn();

            // Assert
            Assert.AreEqual(GameState.Active, game.State);
            Assert.AreEqual(PlayState.PlaceUnits, game.PlayState);

            Assert.AreEqual(2, game.TurnCounter);

            Assert.AreEqual(nextPlayer.Id, game.CurrentPlayer.Id);
        }

        [TestMethod]
        public void EndTurnWithInActivePlayers()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers(3, 1);

            var inactivePlayer = game.Teams.SelectMany(x => x.Players).FirstOrDefault(x => x.Id != game.CurrentPlayerId);
            inactivePlayer.State = PlayerState.InActive;

            var previousPlayer = game.CurrentPlayer;

            // Act
            for (int i = 0; i < 5; ++i)
            {
                game.EndTurn();

                // Assert
                Assert.AreEqual(GameState.Active, game.State);
                Assert.AreEqual(PlayState.PlaceUnits, game.PlayState);

                Assert.AreEqual(2 + i, game.TurnCounter);

                Assert.AreEqual(PlayerState.Active, game.CurrentPlayer.State);

                Assert.AreNotEqual(inactivePlayer.Id, game.CurrentPlayerId);

                Assert.AreNotEqual(previousPlayer.Id, game.CurrentPlayerId);

                previousPlayer = game.CurrentPlayer;
            }
        }

        [TestMethod]
        public void PlaceUnits()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();


            // Act
            var currentPlayer = game.CurrentPlayer;
            var unitsToPlace = game.GetUnitsToPlace(currentPlayer);

            var countries = new List<Tuple<string, int>>
            {
                Tuple.Create(currentPlayer.Countries.First().CountryIdentifier, 
                    unitsToPlace)
            };

            game.PlaceUnits(countries);

            // Assert
            Assert.AreEqual(1 + unitsToPlace, currentPlayer.Countries.First().Units);
            Assert.AreEqual(PlayState.PlaceUnits, game.PlayState);
            Assert.AreNotEqual(currentPlayer, game.CurrentPlayer);
        }

        [TestMethod]
        public void PlaceUnitsAfterInit()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();


            // Act
            var currentPlayer = game.CurrentPlayer;
            var unitsToPlace = game.GetUnitsToPlace(currentPlayer);

            var country = currentPlayer.Countries.First();
            var originalUnits = country.Units;

            var countries = new List<Tuple<string, int>>
            {
                Tuple.Create(country.CountryIdentifier, 
                    unitsToPlace)
            };

            game.PlaceUnits(countries);

            // Assert
            Assert.AreEqual(originalUnits + unitsToPlace, currentPlayer.Countries.First().Units);
            Assert.AreEqual(PlayState.Attack, game.PlayState);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void PlaceLessUnitsThanAvailable()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();


            // Act
            var currentPlayer = game.CurrentPlayer;

            var countries = new List<Tuple<string, int>>
            {
                Tuple.Create(currentPlayer.Countries.First().CountryIdentifier, 
                game.GetUnitsToPlace(currentPlayer) - 1)
            };

            game.PlaceUnits(countries);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void PlaceMoreUnitsThanAvailable()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();


            // Act
            var currentPlayer = game.CurrentPlayer;

            var countries = new List<Tuple<string, int>>
            {
                Tuple.Create(currentPlayer.Countries.First().CountryIdentifier, game.GetUnitsToPlace(currentPlayer) + 1)
            };

            game.PlaceUnits(countries);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void PlaceUnitsForPlayerOtherThanCurrentNotAllowed()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();




            // Act
            var currentPlayer = game.Teams.OrderByDescending(x => x.PlayOrder).First().Players.First();

            var countries = new List<Tuple<string, int>>
            {
                Tuple.Create(currentPlayer.Countries.First().CountryIdentifier, 1)
            };

            game.PlaceUnits(countries);

            // Assert
        }

        [TestMethod]
        public void AttackSuccess()
        {
            // Arrange
            var attackService = new AttackService(new AttackerWinsRandomGen());
            var mapTemplate = TestUtils.GetMapTemplate();
            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();
            game.AttackService = attackService;

            var currentPlayer = game.CurrentPlayer;

            var source = TestHelper.GetCountryWithEnemyConnection(game, currentPlayer, mapTemplate);
            game.PlaceUnits(new[] { Tuple.Create(source.CountryIdentifier, game.GetUnitsToPlace(currentPlayer)) });

            var destination = TestHelper.GetConnectedEnemyCountry(game, currentPlayer, source, mapTemplate);

            // Act
            game.Attack(source.CountryIdentifier, destination.CountryIdentifier, 1);

            // Assert
            Assert.AreEqual(source.PlayerId, destination.PlayerId);
        }

        [TestMethod]
        public void AttackNeutralSuccess()
        {
            // Arrange
            var attackService = new AttackService(new AttackerWinsRandomGen());
            var mapTemplate = TestUtils.GetMapTemplate();
            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();
            game.AttackService = attackService;

            var currentPlayer = game.CurrentPlayer;

            var source = TestHelper.GetCountryWithEnemyConnection(game, currentPlayer, mapTemplate);
            game.PlaceUnits(new[] { Tuple.Create(source.CountryIdentifier, game.GetUnitsToPlace(currentPlayer)) });

            var destination = TestHelper.GetConnectedEnemyCountry(game, currentPlayer, source, mapTemplate);
            destination.PlayerId = Guid.Empty;

            // Act
            game.Attack(source.CountryIdentifier, destination.CountryIdentifier, 1);

            // Assert
            Assert.AreEqual(source.PlayerId, destination.PlayerId);
        }

        [TestMethod]
        public void AttackWillDistributeBonusCardSuccess()
        {
            // Arrange
            var attackService = new AttackService(new AttackerWinsRandomGen());
            var mapTemplate = TestUtils.GetMapTemplate();
            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();
            game.AttackService = attackService;

            var currentPlayer = game.CurrentPlayer;

            var source = TestHelper.GetCountryWithEnemyConnection(game, currentPlayer, mapTemplate);
            source.PlaceUnits(game.GetUnitsToPlace(currentPlayer));
            var destination = TestHelper.GetConnectedEnemyCountry(game, currentPlayer, source, mapTemplate);
            game.PlayState = PlayState.Attack;

            // Act
            game.Attack(source.CountryIdentifier, destination.CountryIdentifier, 1);



            // Assert
            Assert.AreEqual(1, currentPlayer.Cards.Count(), "Player did not receive a card");
        }

        [TestMethod]
        public void AttackWillDistributeOnlyOneBonusCardSuccess()
        {
            // Arrange
            var attackService = new AttackService(new AttackerWinsRandomGen());
            var mapTemplate = TestUtils.GetMapTemplate();
            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();
            game.AttackService = attackService;

            game.PlayState = PlayState.Attack;

            var currentPlayer = game.CurrentPlayer;

            for (int i = 0; i < 2; ++i)
            {
                var source = TestHelper.GetCountryWithEnemyConnection(game, currentPlayer, mapTemplate);
                source.PlaceUnits(game.GetUnitsToPlace(currentPlayer));
                var destination = TestHelper.GetConnectedEnemyCountry(game, currentPlayer, source, mapTemplate);

                // Act
                game.Attack(source.CountryIdentifier, destination.CountryIdentifier, 1);
            }

            // Assert
            Assert.AreEqual(1, currentPlayer.Cards.Count(), "Player did not receive only one card");
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AttackWhenPlacingIsRequiredWillThrow()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();


            // Act
            var currentPlayer = game.CurrentPlayer;

            var source = currentPlayer.Countries.First();

            var destination =
                game.Teams.First(x => !x.Players.Contains(currentPlayer)).Players.First().Countries.First();

            game.Attack(source.CountryIdentifier, destination.CountryIdentifier, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AttackFromForeignCountryWillThrow()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();


            // Act
            var currentPlayer = game.CurrentPlayer;

            var source = currentPlayer.Countries.First();

            var destination =
                game.Teams.First(x => !x.Players.Contains(currentPlayer)).Players.First().Countries.First();

            game.Attack(destination.CountryIdentifier, source.CountryIdentifier, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void AttackOwnCountryWillThrow()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();


            // Act
            var currentPlayer = game.CurrentPlayer;

            var source = currentPlayer.Countries.First();
            var destination = currentPlayer.Countries.Skip(1).First();

            game.Attack(destination.CountryIdentifier, source.CountryIdentifier, 1);
        }

        [TestMethod]
        public void MoveSuccess()
        {
            // Arrange
            var mapTemplate = TestUtils.GetMapTemplate();
            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();

            var currentPlayer = game.CurrentPlayer;

            var source = game.Map.Countries.First();
            game.Map.UpdateOwnership(currentPlayer, source);
            source.PlaceUnits(game.GetUnitsToPlace(currentPlayer));
            var destination =
                game.Map.Countries.First(
                    d =>
                        mapTemplate.Connections.Any(
                            x => x.Origin == source.CountryIdentifier && x.Destination == d.CountryIdentifier));
            game.Map.UpdateOwnership(currentPlayer, destination);

            game.PlayState = PlayState.Move;

            // Act           
            game.Move(source.CountryIdentifier, destination.CountryIdentifier, 1);

            // Assert
            Assert.AreEqual(source.PlayerId, destination.PlayerId);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void MoveToForeignCountry()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();

            // Act
            var currentPlayer = game.CurrentPlayer;

            var source = currentPlayer.Countries.First();
            var destination =
                game.Teams.First(x => !x.Players.Contains(currentPlayer)).Players.First().Countries.First();

            game.Move(source.CountryIdentifier, destination.CountryIdentifier, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void MoveToNotConnectedCountry()
        {
            // Arrange
            var mapTemplate = TestUtils.GetMapTemplate();
            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();

            // Act
            var currentPlayer = game.CurrentPlayer;

            // Get to unconnected countries
            var source = currentPlayer.Countries.First().CountryIdentifier;

            string destination = currentPlayer.Countries.Skip(1).First().CountryIdentifier;
            while (mapTemplate.AreConnected(source, destination))
            {
                destination = currentPlayer.Countries.RandomElement().CountryIdentifier;
            }

            game.Move(source, destination, 1);
        }
    }
}
