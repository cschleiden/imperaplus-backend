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
        public void PlayersStartWithInitialCountryUnits()
        {
            // Arrange
            var game = TestUtils.CreateGameWithMapAndPlayers();
            game.Options.MapDistribution = Enums.MapDistribution.Malibu;

            // Act
            game.Start(TestUtils.GetMapTemplate(), new TestRandomGen());

            // Assert
            Assert.AreEqual(game.Options.InitialCountryUnits, game.Map.Countries.First(x => !x.IsNeutral).Units,
                "Units do not match");
            Assert.IsTrue(game.Map.Countries.Where(x => x.IsNeutral).All(c => c.Units == 1),
                "Neutral country does not have 1 unit");
        }

        [TestMethod]
        public void Malibu3_PlayersHave3Countries()
        {
            // Arrange
            var game = TestUtils.CreateGameWithMapAndPlayers();
            game.Options.MapDistribution = Enums.MapDistribution.Malibu3;

            // Act
            game.Start(TestUtils.GetMapTemplate(), new TestRandomGen());

            // Assert
            Assert.IsTrue(game.Teams.SelectMany(t => t.Players)
                .All(p => game.Map.Countries.Count(c => c.PlayerId == p.Id) == 3));
        }
    }
}
