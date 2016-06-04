using ImperaPlus.Domain.Bots;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Tests.Games;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ImperaPlus.Domain.Tests.Bots
{
    [TestClass]
    public class BotTests
    {
        [TestMethod]
        public void PlayTurnSuccess()
        {
            // Arrange
            var attackService = new AttackService(new AttackerRandomGen(new RandomGen()));
            var game = TestUtils.CreateStartedGameWithMapAndPlayers();

            var currentPlayer = game.CurrentPlayer;

            game.AttackService = attackService;

            // Act
            var bot = new Bot(game);
            bot.PlayTurn();

            // Assert
            Assert.AreNotEqual(currentPlayer, game.CurrentPlayer, "Current player has not changed");
            Assert.IsFalse(game.HistoryEntries.Count() == 0, "No history entries");
        }
    }
}
