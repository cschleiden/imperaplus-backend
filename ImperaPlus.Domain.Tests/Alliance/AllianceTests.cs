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
            unitOfWork = InMemory.GetInMemoryUnitOfWork();
        }

        [TestMethod]
        public void AddPlayerToAlliance()
        {
            var alliance = GetTestAlliance(TestUtils.CreateUser("admin"));
            var user = TestUtils.CreateUser("test");

            alliance.AddMember(user);
            unitOfWork.Commit();

            Assert.IsTrue(alliance.IsMember(user));
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.UserAlreadyInAlliance)]
        public void AddPlayerToAllianceWhoHasAlreadyJoined()
        {
            var alliance = GetTestAlliance(TestUtils.CreateUser("admin"));
            var user = TestUtils.CreateUser("test");
            alliance.AddMember(user);
            unitOfWork.Commit();

            alliance.AddMember(user);
            unitOfWork.Commit();
        }

        [TestMethod]
        public void RemovePlayerFromAlliance()
        {
            var alliance = GetTestAlliance(TestUtils.CreateUser("admin"));
            var user = TestUtils.CreateUser("test");
            alliance.AddMember(user);
            unitOfWork.Commit();

            Assert.IsTrue(alliance.IsMember(user));

            alliance.RemoveMember(user);
            unitOfWork.Commit();

            Assert.IsFalse(alliance.IsMember(user));
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.UserNotAMemberOfAlliance)]
        public void RemovePlayerFromAllianceNotAMember()
        {
            var alliance = GetTestAlliance(TestUtils.CreateUser("admin"));
            unitOfWork.Commit();

            var user = TestUtils.CreateUser("test");

            alliance.RemoveMember(user);
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.UserNotAMemberOfAlliance)]
        public void MakeAdminNotAMember()
        {
            var alliance = GetTestAlliance(TestUtils.CreateUser("admin"));
            unitOfWork.Commit();

            var user = TestUtils.CreateUser("test");
            alliance.MakeAdmin(user);
        }

        [TestMethod]
        public void RequestToJoin()
        {
            var alliance = GetTestAlliance(TestUtils.CreateUser("admin"));

            var user = TestUtils.CreateUser("user");
            var request = alliance.RequestToJoin(user, "Reason");
            unitOfWork.Commit();

            Assert.IsNotNull(request);
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.ActiveRequestToJoinAllianceExists)]
        public void RequestToJoinAlreadyExists()
        {
            var alliance = GetTestAlliance(TestUtils.CreateUser("admin"));
            var user = TestUtils.CreateUser("user");
            var request = alliance.RequestToJoin(user, "Reason");
            unitOfWork.Commit();

            alliance.RequestToJoin(user, "Reason");
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.UserAlreadyInAlliance)]
        public void RequestToJoinAlreadyMember()
        {
            var alliance = GetTestAlliance(TestUtils.CreateUser("admin"));
            var user = TestUtils.CreateUser("user");
            alliance.AddMember(user);
            unitOfWork.Commit();

            alliance.RequestToJoin(user, "Reason");
        }

        [TestMethod]
        public void ApproveJoinRequest()
        {
            var admin = TestUtils.CreateUser("admin");
            var alliance = GetTestAlliance(admin);
            var user = TestUtils.CreateUser("user");
            var request = alliance.RequestToJoin(user, "Reason");
            unitOfWork.Commit();

            alliance.ApproveRequest(admin, request.Id);

            Assert.IsTrue(alliance.IsMember(user));
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.NoActiveRequestToJoinAlliance)]
        public void ApproveJoinRequestNotExists()
        {
            var admin = TestUtils.CreateUser("admin");
            var alliance = GetTestAlliance(admin);
            var user = TestUtils.CreateUser("user");
            unitOfWork.Commit();

            alliance.ApproveRequest(admin, Guid.NewGuid());
        }


        [TestMethod]
        public void DenyJoinRequest()
        {
            var admin = TestUtils.CreateUser("admin");
            var alliance = GetTestAlliance(admin);
            var user = TestUtils.CreateUser("user");
            var request = alliance.RequestToJoin(user, "Reason");
            unitOfWork.Commit();

            alliance.DenyRequest(admin, request.Id);

            Assert.IsFalse(alliance.IsMember(user));
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.NoActiveRequestToJoinAlliance)]
        public void DenyJoinRequestNotExists()
        {
            var admin = TestUtils.CreateUser("admin");
            var alliance = GetTestAlliance(admin);
            var user = TestUtils.CreateUser("user");
            unitOfWork.Commit();

            alliance.DenyRequest(admin, Guid.NewGuid());
        }

        private Alliances.Alliance GetTestAlliance(User admin)
        {
            var alliance = new Alliances.Alliance("test", "testDesc");
            alliance.AddMember(admin);
            alliance.MakeAdmin(admin);
            unitOfWork.Alliances.Add(alliance);
            unitOfWork.Commit();

            return alliance;
        }
    }
}
