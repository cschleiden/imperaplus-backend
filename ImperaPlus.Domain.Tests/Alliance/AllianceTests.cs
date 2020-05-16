using System;
using ImperaPlus.DataAccess.InMemory;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Tests.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.Domain.Tests.Alliance
{
    [TestClass]
    public class AllianceTests
    {
        private IUnitOfWork unitOfWork;

        [TestInitialize]
        public void Setup()
        {
            this.unitOfWork = InMemory.GetInMemoryUnitOfWork();
        }

        [TestMethod]
        public void AddPlayerToAlliance()
        {
            var alliance = this.GetTestAlliance(TestUtils.CreateUser("admin"));
            var user = TestUtils.CreateUser("test");

            alliance.AddMember(user);
            this.unitOfWork.Commit();

            Assert.IsTrue(alliance.IsMember(user));
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.UserAlreadyInAlliance)]
        public void AddPlayerToAllianceWhoHasAlreadyJoined()
        {
            var alliance = this.GetTestAlliance(TestUtils.CreateUser("admin"));
            var user = TestUtils.CreateUser("test");
            alliance.AddMember(user);
            this.unitOfWork.Commit();

            alliance.AddMember(user);
            this.unitOfWork.Commit();
        }

        [TestMethod]
        public void RemovePlayerFromAlliance()
        {
            var alliance = this.GetTestAlliance(TestUtils.CreateUser("admin"));
            var user = TestUtils.CreateUser("test");
            alliance.AddMember(user);
            this.unitOfWork.Commit();

            Assert.IsTrue(alliance.IsMember(user));

            alliance.RemoveMember(user);
            this.unitOfWork.Commit();

            Assert.IsFalse(alliance.IsMember(user));
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.UserNotAMemberOfAlliance)]
        public void RemovePlayerFromAllianceNotAMember()
        {
            var alliance = this.GetTestAlliance(TestUtils.CreateUser("admin"));
            this.unitOfWork.Commit();

            var user = TestUtils.CreateUser("test");

            alliance.RemoveMember(user);
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.UserNotAMemberOfAlliance)]
        public void MakeAdminNotAMember()
        {
            var alliance = this.GetTestAlliance(TestUtils.CreateUser("admin"));
            this.unitOfWork.Commit();

            var user = TestUtils.CreateUser("test");
            alliance.MakeAdmin(user);
        }

        [TestMethod]
        public void RequestToJoin()
        {
            var alliance = this.GetTestAlliance(TestUtils.CreateUser("admin"));

            var user = TestUtils.CreateUser("user");
            var request = alliance.RequestToJoin(user, "Reason");
            this.unitOfWork.Commit();

            Assert.IsNotNull(request);
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.ActiveRequestToJoinAllianceExists)]
        public void RequestToJoinAlreadyExists()
        {
            var alliance = this.GetTestAlliance(TestUtils.CreateUser("admin"));
            var user = TestUtils.CreateUser("user");
            var request = alliance.RequestToJoin(user, "Reason");
            this.unitOfWork.Commit();

            alliance.RequestToJoin(user, "Reason");
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.UserAlreadyInAlliance)]
        public void RequestToJoinAlreadyMember()
        {
            var alliance = this.GetTestAlliance(TestUtils.CreateUser("admin"));
            var user = TestUtils.CreateUser("user");
            alliance.AddMember(user);
            this.unitOfWork.Commit();

            alliance.RequestToJoin(user, "Reason");
        }

        [TestMethod]
        public void ApproveJoinRequest()
        {
            var admin = TestUtils.CreateUser("admin");
            var alliance = this.GetTestAlliance(admin);
            var user = TestUtils.CreateUser("user");
            var request = alliance.RequestToJoin(user, "Reason");
            this.unitOfWork.Commit();

            alliance.ApproveRequest(admin, request.Id);

            Assert.IsTrue(alliance.IsMember(user));
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.NoActiveRequestToJoinAlliance)]
        public void ApproveJoinRequestNotExists()
        {
            var admin = TestUtils.CreateUser("admin");
            var alliance = this.GetTestAlliance(admin);
            var user = TestUtils.CreateUser("user");
            this.unitOfWork.Commit();

            alliance.ApproveRequest(admin, Guid.NewGuid());
        }


        [TestMethod]
        public void DenyJoinRequest()
        {
            var admin = TestUtils.CreateUser("admin");
            var alliance = this.GetTestAlliance(admin);
            var user = TestUtils.CreateUser("user");
            var request = alliance.RequestToJoin(user, "Reason");
            this.unitOfWork.Commit();

            alliance.DenyRequest(admin, request.Id);

            Assert.IsFalse(alliance.IsMember(user));
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.NoActiveRequestToJoinAlliance)]
        public void DenyJoinRequestNotExists()
        {
            var admin = TestUtils.CreateUser("admin");
            var alliance = this.GetTestAlliance(admin);
            var user = TestUtils.CreateUser("user");
            this.unitOfWork.Commit();

            alliance.DenyRequest(admin, Guid.NewGuid());
        }

        private Alliances.Alliance GetTestAlliance(User admin)
        {
            var alliance = new Alliances.Alliance("test", "testDesc");
            alliance.AddMember(admin);
            alliance.MakeAdmin(admin);
            this.unitOfWork.Alliances.Add(alliance);
            this.unitOfWork.Commit();

            return alliance;
        }
    }
}
