using System.Linq;
using ImperaPlus.Domain.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.Domain.Tests.Map
{
    [TestClass]
    public class MapTemplateTests
    {
        [TestMethod]
        public void AreCountriesConnectedSuccess()
        {
            var mapTemplate = new MapTemplate("Name");
            mapTemplate.Countries.Add(new CountryTemplate("A", "A"));
            mapTemplate.Countries.Add(new CountryTemplate("B", "B"));
            mapTemplate.Countries.Add(new CountryTemplate("C", "C"));
            mapTemplate.Connections.Add(new Connection("A", "B"));

            Assert.IsTrue(mapTemplate.AreConnected("A", "B"));
            Assert.IsFalse(mapTemplate.AreConnected("B", "A"));
            Assert.IsFalse(mapTemplate.AreConnected("A", "C"));
        }

        [TestMethod]
        public void GetContinentBonusSuccess()
        {
            // Arrange
            var mapTemplate = new MapTemplate("Name");
            var countryA = new CountryTemplate("A", "A");
            mapTemplate.Countries.Add(countryA);
            var countryB = new CountryTemplate("B", "B");
            mapTemplate.Countries.Add(countryB);

            var continentA = new Continent("A", 42);
            continentA.Countries.Add(countryA);
            mapTemplate.Continents.Add(continentA);

            var continentB = new Continent("B", 23);
            continentB.Countries.Add(countryB);
            mapTemplate.Continents.Add(continentB);

            // Act
            var bonus = mapTemplate.CalculateBonus(mapTemplate.Countries.Select(x => x.Identifier));

            // Assert
            Assert.AreEqual(mapTemplate.Continents.Sum(x => x.Bonus), bonus);
        }
    }
}
