using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.Domain.Tests.Map
{
    using Map = ImperaPlus.Domain.Games.Map;
    using ImperaPlus.Domain.Games;
    using Moq;

    [TestClass]
    public class MapTests
    {
        [TestMethod]
        public void WillClone()
        {
            // Arrange
            var mockGame = new Mock<Game>();
            var map = new Map(mockGame.Object, mockGame.Object.Countries);
            var countryA = new Domain.Games.Country("A", 1);
            map.Countries.Add(countryA);

            // Act
            var clone = map.Clone();
            countryA.Units = 42;
            
            // Assert
            Assert.AreEqual(1, clone.Countries.Count);
            var clonedCountryA = clone.Countries.First();
            Assert.AreNotEqual(42, clonedCountryA.Units);
        }
    }
}
