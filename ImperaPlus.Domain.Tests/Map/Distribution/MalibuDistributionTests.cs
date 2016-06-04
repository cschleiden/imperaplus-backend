using ImperaPlus.Domain.Games.Distribution;
using ImperaPlus.Domain.Tests.Games;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ImperaPlus.Domain.Tests.Map.Distribution
{
    [TestClass]
    public class MalibuDistributionTests
    {
        [TestMethod]
        public void PlayersShouldStartWith5Units()
        {
            // Arrange
            var game = TestUtils.CreateGameWithMapAndPlayers();
            game.Options.MapDistribution = Enums.MapDistribution.Malibu;

            // Act
            game.Start();

            // Assert
            Assert.AreEqual(MalibuMapDistribution.START_UNITS, game.Map.Countries.First(x => !x.IsNeutral).Units, "Units do not match");
        }
    }
}
