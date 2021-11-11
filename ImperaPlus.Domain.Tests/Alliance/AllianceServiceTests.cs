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
            unitOfWork = InMemory.GetInMemoryUnitOfWork();
            allianceService = new AllianceService(unitOfWork, new TestRandomGen(), new TestUserProvider());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateAllianceShouldFail()
        {
            allianceService.Create(null, null);
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.UserAlreadyInAlliance)]
        public void CreateAllianceWhenUserIsAlreadyAMember()
        {
            var user = TestUtils.CreateUser("TestUser", unitOfWork);
            user.AllianceId = Guid.NewGuid();
            user.Alliance = new Alliances.Alliance("Test", "Test");
            TestUserProvider.User = user;

            allianceService.Create("Test2", "Test");
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.AllianceWithNameAlreadyExists)]
        public void CreateAllianceWithExistingNameDifferentCapitalization()
        {
            unitOfWork.Alliances.Add(new Alliances.Alliance("testAlliance", "test"));
            unitOfWork.Commit();

            var user = TestUtils.CreateUser("TestUser", unitOfWork);
            TestUserProvider.User = user;
            allianceService.Create("TestAlliance", "Test");
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.AllianceWithNameAlreadyExists)]
        public void CreateAllianceWithExistingName()
        {
            unitOfWork.Alliances.Add(new Alliances.Alliance("testAlliance", "test"));
            unitOfWork.Commit();

            var user = TestUtils.CreateUser("TestUser", unitOfWork);
            TestUserProvider.User = user;
            allianceService.Create("testAlliance", "Test");
        }

        [TestMethod]
        public void DeleteAlliance()
        {
            var alliance = new Alliances.Alliance("testAlliance", "test");
            var admin = TestUtils.CreateUser("Admin", unitOfWork);
            TestUserProvider.User = admin;
            alliance.AddMember(admin);
            alliance.MakeAdmin(admin);
            unitOfWork.Alliances.Add(alliance);
            unitOfWork.Commit();

            allianceService.Delete(alliance.Id);
            unitOfWork.Commit();

            Assert.IsNull(unitOfWork.Alliances.Get(alliance.Id));
        }
    }
}
