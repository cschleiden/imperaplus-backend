using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Games.History;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Tests.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace ImperaPlus.Domain.Tests.Games
{
    [TestClass]
    public class HistoryTests
    {
        [TestMethod]
        public void PlaceAttackMoveRecordedInHistory()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();
            var mapTemplate = TestUtils.GetMapTemplate();            
            var origin = TestHelper.GetCountryWithEnemyConnection(game, game.CurrentPlayer, mapTemplate);
            var destination = TestHelper.GetConnectedEnemyCountry(game, game.CurrentPlayer, origin, mapTemplate);
            var player = game.CurrentPlayer;

            // Reset history
            game.HistoryEntries.Clear();

            // Act
            game.PlaceUnits(mapTemplate, new List<Tuple<string, int>>
            {
                Tuple.Create(origin.CountryIdentifier, game.GetUnitsToPlace(mapTemplate, game.CurrentPlayer))
            });

            game.Attack(new AttackService(new AttackerWinsRandomGen()), new RandomGen(), mapTemplate, origin.CountryIdentifier, destination.CountryIdentifier, origin.Units - game.Options.MinUnitsPerCountry);
            game.EndTurn();

            // Assert
            Assert.IsTrue(game.HistoryEntries.Any());

            var placeEntry = game.HistoryEntries.Skip(0).First();
            Assert.IsNotNull(placeEntry);
            Assert.AreEqual(HistoryAction.PlaceUnits, placeEntry.Action);
            Assert.AreEqual(player.Id, placeEntry.ActorId);

            var attackEntry = game.HistoryEntries.Skip(1).First();
            Assert.IsNotNull(attackEntry);
            Assert.AreEqual(HistoryAction.Attack, attackEntry.Action);
            Assert.AreEqual(player.Id, attackEntry.ActorId);
            Assert.AreNotEqual(player.Id, attackEntry.OtherPlayerId);

            var endTurnEntry = game.HistoryEntries.Last();
            Assert.AreEqual(player.Id, endTurnEntry.ActorId);
        }

        [TestMethod]
        public void PlaceTurnCanBeRetrievedFromHistory()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();
            var mapTemplate = TestUtils.GetMapTemplate();
            var origin = TestHelper.GetCountryWithEnemyConnection(game, game.CurrentPlayer, mapTemplate);
            var destination = TestHelper.GetConnectedEnemyCountry(game, game.CurrentPlayer, origin, mapTemplate);
            var player = game.CurrentPlayer;            

            // Act 
            var originalUnitCount = origin.Units;

            game.PlaceUnits(mapTemplate, new List<Tuple<string, int>>
            {
                Tuple.Create(origin.CountryIdentifier, game.GetUnitsToPlace(mapTemplate, game.CurrentPlayer))
            });
            game.EndTurn();

            var historicTurn = game.GameHistory.GetTurn(0);

            // Assert
            Assert.AreEqual(originalUnitCount, historicTurn.Game.Map.GetCountry(origin.CountryIdentifier).Units);
        }

        [TestMethod]
        public void AttackSuccessTurnCanBeRetrievedFromHistory()
        {
            this.AttackAndVerify(new AttackerWinsRandomGen());
        }

        [TestMethod]
        public void AttackFailTurnCanBeRetrievedFromHistory()
        {
            this.AttackAndVerify(new DefenderWinsRandomGen());
        }

        [TestMethod]
        public void MoveTurnCanBeRetrievedFromHistory()
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();
            var mapTemplate = TestUtils.GetMapTemplate();
            var origin = TestHelper.GetCountryWithFriendlyConnection(game, game.CurrentPlayer, mapTemplate);
            var destination = TestHelper.GetConnectedFriendlyCountry(game, game.CurrentPlayer, origin, mapTemplate);
            var player = game.CurrentPlayer;

            origin.Units = 4;
            game.PlayState = Enums.PlayState.Move;

            // Act 
            var originUnitCount = origin.Units;
            var originOwnerId = origin.PlayerId;
            var destinationOwnerId = destination.PlayerId;

            game.Move(mapTemplate, origin.CountryIdentifier, destination.CountryIdentifier, origin.Units - game.Options.MinUnitsPerCountry);
            game.EndTurn();

            var historicTurn = game.GameHistory.GetTurn(game.TurnCounter - 2);

            // Assert
            Assert.AreEqual(1, historicTurn.Actions.Count());

            var historicOrigin = historicTurn.Game.Map.GetCountry(origin.CountryIdentifier);
            var historicDestination = historicTurn.Game.Map.GetCountry(destination.CountryIdentifier);

            Assert.AreEqual(originUnitCount, historicOrigin.Units);
            Assert.AreEqual(originOwnerId, historicOrigin.PlayerId);
            Assert.AreEqual(destinationOwnerId, historicDestination.PlayerId);
        }

        private void AttackAndVerify(IAttackRandomGen randomGen)
        {
            // Arrange
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();
            var mapTemplate = TestUtils.GetMapTemplate();
            var origin = TestHelper.GetCountryWithEnemyConnection(game, game.CurrentPlayer, mapTemplate);
            var destination = TestHelper.GetConnectedEnemyCountry(game, game.CurrentPlayer, origin, mapTemplate);
            var player = game.CurrentPlayer;

            origin.Units = 4;
            game.PlayState = Enums.PlayState.Attack;

            // Act 
            var originUnitCount = origin.Units;
            var originOwnerId = origin.PlayerId;
            var destinationOwnerId = destination.PlayerId;

            game.Attack(new AttackService(randomGen), new RandomGen(), mapTemplate, origin.CountryIdentifier, destination.CountryIdentifier, origin.Units - game.Options.MinUnitsPerCountry);
            game.EndTurn();

            var historicTurnBefore = game.GameHistory.GetTurn(game.TurnCounter - 2);
            var historicTurn = game.GameHistory.GetTurn(game.TurnCounter - 1);

            // Assert
            Assert.AreEqual(2, historicTurn.Actions.Count());

            var historicOrigin = historicTurnBefore.Game.Map.GetCountry(origin.CountryIdentifier);
            var historicDestination = historicTurnBefore.Game.Map.GetCountry(destination.CountryIdentifier);
            Assert.AreEqual(originUnitCount, historicOrigin.Units);
            Assert.AreEqual(originOwnerId, historicOrigin.PlayerId);
            Assert.AreEqual(destinationOwnerId, historicDestination.PlayerId);
        }        
    }
}
