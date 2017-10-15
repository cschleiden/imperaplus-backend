using System;
using ImperaPlus.DataAccess.InMemory;
using ImperaPlus.Domain.Alliances;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Tests.Helper;
using ImperaPlus.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.Domain.Tests.Alliance
{
    [TestClass]
    public class AllianceServiceTests
    {
        private IUnitOfWork unitOfWork;
        private IAllianceService allianceService;

        [TestInitialize]
        public void Setup()
        {
            this.unitOfWork = InMemory.GetInMemoryUnitOfWork();
            this.allianceService = new AllianceService(this.unitOfWork, new TestRandomGen());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateAllianceShouldFail()
        {
            this.allianceService.Create(null, null, null);
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.UserAlreadyInAlliance)]
        public void CreateAllianceWhenUserIsAlreadyAMember()
        {
            var user = TestUtils.CreateUser("TestUser");
            user.AllianceId = Guid.NewGuid();
            user.Alliance = new Alliances.Alliance("Test", "Test");

            this.allianceService.Create(user, "Test2", "Test");
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.AllianceWithNameAlreadyExists)]
        public void CreateAllianceWithExistingNameDifferentCapitalization()
        {
            this.unitOfWork.Alliances.Add(new Alliances.Alliance("testAlliance", "test"));
            this.unitOfWork.Commit();

            var user = TestUtils.CreateUser("TestUser");
            this.allianceService.Create(user, "TestAlliance", "Test");
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.AllianceWithNameAlreadyExists)]
        public void CreateAllianceWithExistingName()
        {
            this.unitOfWork.Alliances.Add(new Alliances.Alliance("testAlliance", "test"));
            this.unitOfWork.Commit();

            var user = TestUtils.CreateUser("TestUser");
            this.allianceService.Create(user, "testAlliance", "Test");
        }

        //[TestMethod]
        //public void CreateAllianceNameAlreadyInUse()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod]
        //public void CreateAllianceInvalidName()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod]
        //public void AddPlayerToAlliance()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod]
        //public void AddPlayerToAllianceWhoHasAlreadyJoined()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod]
        //public void RemovePlayerFromAlliance()
        //{
        //    Assert.Fail();
        //}
    }
}
