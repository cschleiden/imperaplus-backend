using System;
using System.Linq;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Tournaments;
using ImperaPlus.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ImperaPlus.Domain.Tests.Tournaments
{
    [TestClass]
    public class TournamentServiceTests
    {
        [TestMethod]
        public void CheckOpenShouldStart()
        {
            // Arrange
            var mockUnitOfWork = TestUtils.GetUnitOfWorkMock();
            var tournamentRepositoryMock = new Mock<ITournamentRepository>();
            mockUnitOfWork.SetupGet(x => x.Tournaments).Returns(tournamentRepositoryMock.Object);
            var unitOfWork = mockUnitOfWork.Object;
            var gameServiceMock = new Mock<IGameService>();
            var service = new TournamentService(TestUtils.MockUserProvider(), unitOfWork, gameServiceMock.Object,
                TestUtils.MockMapTemplateProvider());

            var openTournament = new Tournament(
                "Tournament",
                8,
                3,
                3,
                3,
                DateTime.UtcNow.AddDays(-1),
                DateTime.UtcNow,
                new GameOptions { NumberOfPlayersPerTeam = 1 });
            tournamentRepositoryMock
                .Setup(x => x.Get(TournamentState.Open))
                .Returns(new Tournament[] { openTournament }.AsQueryable());

            for (var i = 0; i < 8; ++i)
            {
                openTournament.AddUser(TestUtils.CreateUser($"User{i}"));
            }

            // Act
            var started = service.CheckOpenTournaments(new TestLogger(), new TestRandomGen());

            // Assert
            Assert.IsTrue(started);
            Assert.AreEqual(TournamentState.Groups, openTournament.State);
            Assert.AreEqual(12, openTournament.Pairings.Count());
        }

        [TestMethod]
        public void SynchronizeGames()
        {
            // Arrange
            var mockUnitOfWork = TestUtils.GetUnitOfWorkMock();
            mockUnitOfWork.SetupGet(x => x.Tournaments).Returns(new Mock<ITournamentRepository>().Object);
            mockUnitOfWork.SetupGet(x => x.Games).Returns(new Mock<IGameRepository>().Object);
            var unitOfWork = mockUnitOfWork.Object;

            var gameServiceMock = new Mock<IGameService>();
            var service = new TournamentService(TestUtils.MockUserProvider(), unitOfWork, gameServiceMock.Object,
                TestUtils.MockMapTemplateProvider());

            var tournament = new Tournament("T", 2, 0, 1, 1, DateTime.UtcNow, DateTime.UtcNow,
                new GameOptions { NumberOfPlayersPerTeam = 1 });
            tournament.State = TournamentState.Knockout;

            var user1 = TestUtils.CreateUser("1");
            var user2 = TestUtils.CreateUser("2");

            var teamA = new TournamentTeam(tournament);
            teamA.AddUser(user1);

            var teamB = new TournamentTeam(tournament);
            teamB.AddUser(user2);

            var pairing = new TournamentPairing(tournament, 1, 1, teamA, teamB, 1);
            pairing.State = PairingState.Active;
            tournament.Pairings = new[] { pairing };

            tournament.Teams.Add(teamA);
            tournament.Teams.Add(teamB);

            var game = new Game(null, Enums.GameType.Tournament, "T", null, "WorldDeluxe", new GameOptions());

            game.State = Enums.GameState.Ended;

            var team1 = new Team(game);
            team1.Players.Add(new Player(game, user1, team1) { Outcome = Enums.PlayerOutcome.Won });
            game.Teams.Add(team1);

            var team2 = new Team(game);
            team2.Players.Add(new Player(game, user2, team2) { Outcome = Enums.PlayerOutcome.Defeated });
            game.Teams.Add(team2);

            pairing.Games = new[] { game };

            // Act
            service.SynchronizeGamesToPairings(new TestLogger(), tournament);

            // Assert
            Assert.IsTrue(tournament.Pairings.First().CanWinnerBeDetermined);
            Assert.AreEqual(PairingState.Done, pairing.State);
            Assert.AreEqual(TournamentTeamState.InActive, teamB.State);
            Assert.AreEqual(TournamentTeamState.Active, teamA.State);
        }
    }
}
