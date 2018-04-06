using System.Linq;
using ImperaPlus.Domain.Bots;
using ImperaPlus.Domain.Services;
using ImperaPlus.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.Domain.Tests.Bots
{
    [TestClass]
    public class BotTests
    {
        [TestMethod]
        public void PlayTurnSuccess()
        {
            // Arrange            
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();

            var currentPlayer = game.CurrentPlayer;

            // Act
            var bot = new Bot(new TestLogger(), game, TestUtils.GetMapTemplate(), new AttackService(new AttackerWinsRandomGen()), new TestRandomGen());
            bot.PlayTurn();

            // Assert
            Assert.AreNotEqual(currentPlayer, game.CurrentPlayer, "Current player has not changed");
            Assert.IsFalse(game.HistoryEntries.Count() == 0, "No history entries");
        }
    }
}
