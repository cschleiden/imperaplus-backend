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
            this._country = new Country("a", 1);
        }

        [TestMethod]
        public void UpdatingUnitsSetsFlag()
        {
            this._country.Units = 2;

            Assert.IsTrue(this._country.IsUpdated);
        }

        [TestMethod]
        public void UpdatingOwnershipSetsFlag()
        {
            this._country.PlayerId = Guid.NewGuid();

            Assert.IsTrue(this._country.IsUpdated);
        }

        [TestMethod]
        public void UpdatingOwnershipRemovesCapitals()
        {
            this._country.Flags |= CountryFlags.Capital;

            this._country.PlayerId = Guid.NewGuid();

            Assert.IsFalse(this._country.Flags.HasFlag(CountryFlags.Capital));
        }

        [TestMethod]
        public void UpdatingTeamSetsFlag()
        {
            this._country.TeamId = Guid.NewGuid();

            Assert.IsTrue(this._country.IsUpdated);
        }
    }
}