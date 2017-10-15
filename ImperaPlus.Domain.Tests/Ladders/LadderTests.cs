using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Ladders;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Tests.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading;
using ImperaPlus.TestSupport;

namespace ImperaPlus.Domain.Tests.Ladders
{
    [TestClass]
    public class LadderTests
    {
        [TestMethod]
        public void GameNameIsGenerated()
        {
            // Arrange
            var ladder = new Ladder("1vs1", 2, 1);

            // Act
            var gameName = ladder.GetGameName();
            Thread.Sleep(1);
            var gameName2 = ladder.GetGameName();

            // Assert
            Assert.IsTrue(gameName.Contains(ladder.Name));
            Assert.AreNotEqual(gameName, gameName2);
        }

        [TestMethod]
        [ExpectedDomainException(ErrorCode.LadderCannotActivate)]
        public void ActiveOnlyWithTemplates()
        {
            // Arrange
            var ladder = new Ladder("1vs1", 2, 1);

            // Act
            ladder.ToggleActive(true);
        }

        [TestMethod]
        public void ActiveWithTemplates()
        {
            // Arrange
            var ladder = new Ladder("1vs1", 2, 1);
            ladder.MapTemplates.Add("MapTemplate");

            // Act
            ladder.ToggleActive(true);

            // Assert
            Assert.IsTrue(ladder.IsActive);
        }

        [TestClass]
        public class QueueUser
        {
            [TestMethod]
            public void QueueForUserSuccess()
            {
                // Arrange
                var user1 = TestUtils.CreateUser("1");
                var ladder = new Ladder("Default", 2, 1);

                // Act
                ladder.QueueUser(user1);

                // Assert
                Assert.IsTrue(ladder.Queue.Any());
            }

            [TestMethod]
            [ExpectedDomainException(ErrorCode.LadderUserAlreadyQueue)]
            public void UserCannotQueueTwice()
            {
                // Arrange
                var user1 = TestUtils.CreateUser("1");
                var ladder = new Ladder("Default", 2, 1);

                // Act
                ladder.QueueUser(user1);
                ladder.QueueUser(user1);
            }

            [TestMethod]
            public void UserCanLeaveQueue()
            {
                var user1 = TestUtils.CreateUser("1");
                var ladder = new Ladder("Default", 2, 1);

                ladder.QueueUser(user1);
                ladder.QueueLeaveUser(user1);
            }

            [TestMethod]
            [ExpectedDomainException(ErrorCode.LadderUserNotInQueue)]
            public void UserCannotLeaveQueueIfNotInIt()
            {
                var user1 = TestUtils.CreateUser("1");
                var ladder = new Ladder("Default", 2, 1);
                
                ladder.QueueLeaveUser(user1);
            }
        }
    }

    [TestClass]
    public class LadderServiceTests
    {       
        [TestClass]
        public class CreateGames
        {
            [TestMethod]
            public void GamesAreCreated()
            {
                // Arrange
                var mockUnitOfWork = TestUtils.GetUnitOfWorkMock();
                mockUnitOfWork.SetupGet(x => x.Games).Returns(new MockGamesRepository());
                mockUnitOfWork.SetupGet(x => x.Ladders).Returns(new MockLadderRepository());
                var unitOfWork = mockUnitOfWork.Object;

                var gameServiceMock = new Mock<IGameService>();
                gameServiceMock.Setup(x => x.Create(
                    Enums.GameType.Ranking,
                    It.IsAny<User>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Domain.Games.GameOptions>()))
                    .Returns(TestUtils.CreateGame(2, 1, Enums.GameType.Ranking));

                var ladderService = new LadderService(unitOfWork, gameServiceMock.Object, TestUtils.MockMapTemplateProvider(), new Mock<IEventAggregator>().Object);

                var mapTemplate = TestUtils.GetMapTemplate();

                var ladder = new Ladder("Default", 2, 1);                

                ladder.MapTemplates.Add(mapTemplate.Name);

                ladder.Options.MapDistribution = Enums.MapDistribution.Default;
                ladder.Options.VisibilityModifier.Add(Enums.VisibilityModifierType.None);

                ladder.ToggleActive(true);

                unitOfWork.Ladders.Add(ladder);

                var user1 = TestUtils.CreateUser("1");
                var user2 = TestUtils.CreateUser("2");
                ladder.QueueUser(user1);
                ladder.QueueUser(user2);

                // Act
                ladderService.CheckAndCreateMatches(new RandomGen());

                // Assert
                Assert.IsTrue(ladder.Games.Any());
                var game = ladder.Games.First();
                Assert.AreEqual(Enums.GameState.Active, game.State);
                Assert.AreEqual(Enums.GameType.Ranking, game.Type);
                Assert.AreEqual(ladder, game.Ladder);
                Assert.IsFalse(ladder.Queue.Any());
            }
        }
    }
}
