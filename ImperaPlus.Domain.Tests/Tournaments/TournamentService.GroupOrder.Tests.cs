using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Tournaments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            mockUnitOfWork.SetupGet(x => x.Tournaments).Returns(new MockTournamentRepository());
            mockUnitOfWork.SetupGet(x => x.Games).Returns(new MockGamesRepository());
            var unitOfWork = mockUnitOfWork.Object;

            var gameServiceMock = new Mock<IGameService>();
            var service = new TournamentService(unitOfWork, gameServiceMock.Object, TestUtils.MockMapTemplateProvider());

            const int GroupGames = 3;
            var tournament = new Tournament(
                "T", 
                numberOfTeams: 8, 
                numberOfGroupGames: GroupGames, 
                numberOfKnockoutGames: 1, 
                numberOfFinalGames: 1, 
                startOfRegistration: DateTime.UtcNow, 
                startOfTournament: DateTime.UtcNow, 
                options: new GameOptions { NumberOfPlayersPerTeam = 1 });

            for (int i = 0; i < 8; ++i)
            {
                this.AddTeam(tournament, i);
            }

            tournament.Start();

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
            this.AssertArray(new[]
            {
                teams1[0], teams1[1], teams1[2], teams1[3]
            }, g1);

            var g2 = tournament.Groups.ElementAt(1).Teams.OrderBy(t => t.GroupOrder).ToArray();
            this.AssertArray(new[]
            {
                teams2[1], teams2[3], teams2[0], teams2[2]
            }, g2);
        }

        private void AssertArray<T>(T[] exepected, T[] v)
        {
            Assert.AreEqual(exepected.Length, v.Length);

            for (int i = 0; i < exepected.Length; ++i)
            {
                Assert.AreEqual(exepected[i], v[i]);
            }
        }

        private void SetWins(Tournament tournament, TournamentTeam teamA, TournamentTeam teamB, int winsA, int winsB)
        {
            foreach(var group in tournament.Groups)
            {
                foreach(var pairing in group.Pairings)
                {
                    if (SetWins(pairing, teamA, teamB, winsA, winsB)
                        || SetWins(pairing, teamB, teamA, winsB, winsA))
                    {
                        return;
                    }
                }
            }
        }

        private bool SetWins(TournamentPairing pairing, TournamentTeam teamA, TournamentTeam teamB, int winsA, int winsB)
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
