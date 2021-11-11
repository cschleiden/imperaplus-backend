﻿using System;
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
    public class TournamentServiceGroupOrderTests
    {
        [TestMethod]
        public void OrderGroups()
        {
            // Arrange
            var mockUnitOfWork = TestUtils.GetUnitOfWorkMock();
            mockUnitOfWork.SetupGet(x => x.Tournaments).Returns(new Mock<ITournamentRepository>().Object);
            mockUnitOfWork.SetupGet(x => x.Games).Returns(new Mock<IGameRepository>().Object);
            var unitOfWork = mockUnitOfWork.Object;

            var gameServiceMock = new Mock<IGameService>();
            var service = new TournamentService(TestUtils.MockUserProvider(), unitOfWork, gameServiceMock.Object,
                TestUtils.MockMapTemplateProvider());

            const int GroupGames = 3;
            var tournament = new Tournament(
                "T",
                8,
                GroupGames,
                1,
                1,
                DateTime.UtcNow,
                DateTime.UtcNow,
                new GameOptions { NumberOfPlayersPerTeam = 1 });

            for (var i = 0; i < 8; ++i)
            {
                AddTeam(tournament, i);
            }

            tournament.Start(new TestRandomGen());

            var teams1 = tournament.Groups.ElementAt(0).Teams.ToArray();
            var teams2 = tournament.Groups.ElementAt(1).Teams.ToArray();

            // Generate scenarios

            // Group 1
            //     T1 T2 T3 T4
            // T1   -  w  w  w
            // T2      -  w  w
            // T3         -  w
            // T4            -

            // Expected:
            // T1 3
            // T2 2
            // T3 1
            // T4 0

            // T1
            SetWins(tournament, teams1[0], teams1[1], GroupGames, 0);
            SetWins(tournament, teams1[0], teams1[2], GroupGames, 0);
            SetWins(tournament, teams1[0], teams1[3], GroupGames, 0);

            // T2
            SetWins(tournament, teams1[1], teams1[2], GroupGames, 0);
            SetWins(tournament, teams1[1], teams1[3], GroupGames, 0);

            // T3
            SetWins(tournament, teams1[2], teams1[3], GroupGames, 0);


            // Group 2
            //     T1 T2 T3 T4
            // T1   -  w  w  l
            // T2      -  w  w
            // T3         -  l
            // T4            -

            // Expected:
            // T2 2 - 6 - won against T4
            // T4 2 - 6
            // T1 2 - 5
            // T3 0 - 1

            // T1
            SetWins(tournament, teams2[0], teams2[1], GroupGames, 0); // T1 3 - 0 T2
            SetWins(tournament, teams2[0], teams2[2], GroupGames - 1, 1); // T1 2 - 1 T3
            SetWins(tournament, teams2[0], teams2[3], 0, GroupGames); // T1 0 - 3 T4

            // T2
            SetWins(tournament, teams2[1], teams2[2], GroupGames, 0); // T2 3 - 0 T3
            SetWins(tournament, teams2[1], teams2[3], GroupGames, 0); // T2 3 - 0 T4

            // T3
            SetWins(tournament, teams2[2], teams2[3], 0, GroupGames); // T3 0 - 3 T4

            // Act
            service.OrderGroupTeams(tournament);

            // Assert
            var g1 = tournament.Groups.ElementAt(0).Teams.OrderBy(t => t.GroupOrder).ToArray();
            AssertArray(new[] { teams1[0].Id, teams1[1].Id, teams1[2].Id, teams1[3].Id },
                g1.Select(x => x.Id).ToArray());

            var g2 = tournament.Groups.ElementAt(1).Teams.OrderBy(t => t.GroupOrder).ToArray();
            AssertArray(new[] { teams2[1].Id, teams2[3].Id, teams2[0].Id, teams2[2].Id },
                g2.Select(x => x.Id).ToArray());
        }

        private void AssertArray<T>(T[] expected, T[] v)
        {
            Assert.AreEqual(expected.Length, v.Length);

            for (var i = 0; i < expected.Length; ++i)
            {
                Assert.AreEqual(expected[i], v[i]);
            }
        }

        private void SetWins(Tournament tournament, TournamentTeam teamA, TournamentTeam teamB, int winsA, int winsB)
        {
            foreach (var group in tournament.Groups)
            {
                foreach (var pairing in group.Pairings)
                {
                    if (SetWins(pairing, teamA, teamB, winsA, winsB)
                        || SetWins(pairing, teamB, teamA, winsB, winsA))
                    {
                        return;
                    }
                }
            }
        }

        private bool SetWins(TournamentPairing pairing, TournamentTeam teamA, TournamentTeam teamB, int winsA,
            int winsB)
        {
            if (pairing.TeamA == teamA && pairing.TeamB == teamB)
            {
                pairing.TeamAWon = winsA;
                pairing.TeamBWon = winsB;

                return true;
            }

            return false;
        }

        private void AddTeam(Tournament tournament, int number)
        {
            var team = new TournamentTeam(tournament);
            team.Name = number.ToString();
            tournament.Teams.Add(team);

            var user = TestUtils.CreateUser(number.ToString());
            tournament.AddUser(user, team);
        }
    }
}
