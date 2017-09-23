using System.Linq;
using ImperaPlus.Domain.Games.Distribution;
using ImperaPlus.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            game.Start(TestUtils.GetMapTemplate(), new RandomGen());

            // Assert
            Assert.AreEqual(MalibuMapDistribution.START_UNITS, game.Map.Countries.First(x => !x.IsNeutral).Units, "Units do not match");
        }
    }
}
