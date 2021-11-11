using System;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Games;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.Domain.Tests.Games
{
    [TestClass]
    public class CountryUpdateTests
    {
        private Country _country;

        [TestInitialize]
        public void TestInitialize()
        {
            _country = new Country("a", 1);
        }

        [TestMethod]
        public void UpdatingUnitsSetsFlag()
        {
            _country.Units = 2;

            Assert.IsTrue(_country.IsUpdated);
        }

        [TestMethod]
        public void UpdatingOwnershipSetsFlag()
        {
            _country.PlayerId = Guid.NewGuid();

            Assert.IsTrue(_country.IsUpdated);
        }

        [TestMethod]
        public void UpdatingOwnershipRemovesCapitals()
        {
            _country.Flags |= CountryFlags.Capital;

            _country.PlayerId = Guid.NewGuid();

            Assert.IsFalse(_country.Flags.HasFlag(CountryFlags.Capital));
        }

        [TestMethod]
        public void UpdatingTeamSetsFlag()
        {
            _country.TeamId = Guid.NewGuid();

            Assert.IsTrue(_country.IsUpdated);
        }
    }
}
