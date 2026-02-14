using System;
using System.Linq;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Tests.Helper;
using ImperaPlus.Domain.Tournaments;
using ImperaPlus.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.Domain.Tests.Tournaments
{
    [TestClass]
    public class TournamentTests
    {
        [TestClass]
        public class Create
        {
            private GameOptions options;

            [TestInitialize]
            public void Initialize()
            {
                options = new GameOptions() { Id = 42 };
            }

            [TestMethod]
            public void Fail()
            {
                // Name
                AssertHelper.VerifyThrows<ArgumentNullException>(
                    () => new Tournament(
                        null, 16, 0, 3, 3, DateTime.UtcNow, DateTime.UtcNow, null),
                    () => new Tournament(
                        "", 16, 0, 3, 3, DateTime.UtcNow, DateTime.UtcNow, null));

                // Options
                AssertHelper.VerifyThrows<ArgumentNullException>(
                    () => new Tournament(
                        "Tournament", 16, 0, 3, 3, DateTime.UtcNow, DateTime.UtcNow, null));

                // Number of teams
                AssertHelper.VerifyThrowsDomain(ErrorCode.TournamentInvalidOption,
                    () => new Tournament("Tournament", 15, 0, 3, 3, DateTime.UtcNow, DateTime.UtcNow, options),
                    () => new Tournament("Tournament", 0, 0, 3, 3, DateTime.UtcNow, DateTime.UtcNow, options),
                    () => new Tournament("Tournament", -16, 0, 3, 3, DateTime.UtcNow, DateTime.UtcNow, options));

                // Number of group games
                AssertHelper.VerifyThrowsDomain(ErrorCode.TournamentInvalidOption,
                    () => new Tournament("Tournament", 16, 2, 3, 3, DateTime.UtcNow, DateTime.UtcNow, options),
                    () => new Tournament("Tournament", 16, -2, 3, 3, DateTime.UtcNow, DateTime.UtcNow, options));

                // Number of knockout games
                AssertHelper.VerifyThrowsDomain(ErrorCode.TournamentInvalidOption,
                    () => new Tournament("Tournament", 16, 0, 0, 3, DateTime.UtcNow, DateTime.UtcNow, options),
                    () => new Tournament("Tournament", 16, 0, 2, 3, DateTime.UtcNow, DateTime.UtcNow, options),
                    () => new Tournament("Tournament", 16, 0, -1, 3, DateTime.UtcNow, DateTime.UtcNow, options));

                // Number of final games
                AssertHelper.VerifyThrowsDomain(ErrorCode.TournamentInvalidOption,
                    () => new Tournament("Tournament", 16, 0, 3, 0, DateTime.UtcNow, DateTime.UtcNow, options),
                    () => new Tournament("Tournament", 16, 0, 3, 2, DateTime.UtcNow, DateTime.UtcNow, options),
                    () => new Tournament("Tournament", 16, 0, 3, -1, DateTime.UtcNow, DateTime.UtcNow, options));
            }

            [TestMethod]
            public void Success()
            {
                var tournament = new Tournament("Tournament", 16, 3, 3, 3, DateTime.UtcNow, DateTime.UtcNow, options);

                Assert.AreNotEqual(Guid.Empty, tournament.Id);

                Assert.AreEqual(16, tournament.NumberOfTeams);

                Assert.AreEqual(3, tournament.NumberOfGroupGames);
                Assert.AreEqual(3, tournament.NumberOfKnockoutGames);
                Assert.AreEqual(3, tournament.NumberOfFinalGames);

                Assert.AreEqual(options.Id, tournament.OptionsId);
                Assert.AreEqual(options, tournament.Options);
            }
        }

        [TestClass]
        public class Join
        {
            [TestMethod]
            public void SinglePlayerTeamJoin_Success()
            {
                var tournament = CreateTournament(0);

                tournament.AddUser(TestUtils.CreateUser("User"));

                Assert.AreEqual(1, tournament.Teams.Count());
                Assert.AreEqual("User", tournament.Teams.First().Name);
            }

            [TestMethod]
            public void SinglePlayerTeamJoin_MultipleSuccess()
            {
                var tournament = CreateTournament(0);

                tournament.AddUser(TestUtils.CreateUser("User1"));
                tournament.AddUser(TestUtils.CreateUser("User2"));

                Assert.AreEqual(2, tournament.Teams.Count());
            }

            [TestMethod]
            [ExpectedDomainException(ErrorCode.TournamentTooManyTeams)]
            public void SinglePlayerTeamJoin_AlreadyFull()
            {
                var tournament = CreateTournamentWithUsers(0);

                tournament.AddUser(TestUtils.CreateUser("User"));
            }

            [TestMethod]
            [ExpectedDomainException(ErrorCode.TournamentCannotJoinLeave)]
            public void SinglePlayerTeamJoin_AlreadyStarted()
            {
                var tournament = CreateTournamentWithUsers(0);
                tournament.Start(new TestRandomGen());

                tournament.AddUser(TestUtils.CreateUser("User"));
            }

            [TestMethod]
            public void PlayerTeamJoin_CreateTeamNoPassword_Success()
            {
                var tournament = CreateTournament(0);

                var team = tournament.CreateTeam(TestUtils.CreateUser("User1"), "Team");

                Assert.IsNotNull(team);
                Assert.AreEqual("Team", team.Name);
            }

            [TestMethod]
            [ExpectedDomainException(ErrorCode.TournamentDuplicateTeamName)]
            public void PlayerTeamJoin_CreateTeam_DuplicateName()
            {
                var tournament = CreateTournament(0);

                tournament.CreateTeam(TestUtils.CreateUser("User1"), "Team");
                tournament.CreateTeam(TestUtils.CreateUser("User2"), "Team");
            }

            [TestMethod]
            public void PlayerTeamJoin_CreateTeamWithPassword_Success()
            {
                var tournament = CreateTournament(0);

                var team = tournament.CreateTeam(TestUtils.CreateUser("User1"), "Team", "Password");

                Assert.IsNotNull(team);
                Assert.AreEqual("Team", team.Name);
                Assert.AreEqual("Password", team.Password);
            }

            [TestMethod]
            public void PlayerTeamJoin_JoinTeamWithPassword_Success()
            {
                var tournament = CreateTournament(0);
                tournament.Options.NumberOfPlayersPerTeam = 2;
                var team = tournament.CreateTeam(TestUtils.CreateUser("User1"), "Team", "Password");

                tournament.AddUser(TestUtils.CreateUser("User2"), team, "Password");

                Assert.AreEqual(2, team.Participants.Count());
            }

            [TestMethod]
            [ExpectedDomainException(ErrorCode.TournamentIncorrectPassword)]
            public void PlayerTeamJoin_IncorrectPassword()
            {
                var tournament = CreateTournament(0);
                var team = tournament.CreateTeam(TestUtils.CreateUser("User1"), "Team", "Password");

                tournament.AddUser(TestUtils.CreateUser("User2"), team, "incorrect");
            }

            [TestMethod]
            [ExpectedDomainException(ErrorCode.TournamentRequiresExplicitTeam)]
            public void PlayerTeamJoin_NotExplicit()
            {
                var tournament = CreateTournament(0);
                tournament.Options.NumberOfPlayersPerTeam = 2;

                tournament.AddUser(TestUtils.CreateUser("User1"));
            }

            [TestMethod]
            public void SinglePlayerJoin_WithTournamentPassword_Success()
            {
                var tournament = CreateTournament(0);
                tournament.Password = "TournamentSecret";

                tournament.AddUser(TestUtils.CreateUser("User1"), "TournamentSecret");

                Assert.AreEqual(1, tournament.Teams.Count());
            }

            [TestMethod]
            [ExpectedDomainException(ErrorCode.TournamentIncorrectPassword)]
            public void SinglePlayerJoin_WithTournamentPassword_WrongPassword()
            {
                var tournament = CreateTournament(0);
                tournament.Password = "TournamentSecret";

                tournament.AddUser(TestUtils.CreateUser("User1"), "WrongPassword");
            }

            [TestMethod]
            [ExpectedDomainException(ErrorCode.TournamentIncorrectPassword)]
            public void SinglePlayerJoin_WithTournamentPassword_NoPassword()
            {
                var tournament = CreateTournament(0);
                tournament.Password = "TournamentSecret";

                tournament.AddUser(TestUtils.CreateUser("User1"));
            }

            [TestMethod]
            public void SinglePlayerJoin_NoTournamentPassword_NullPasswordAccepted()
            {
                var tournament = CreateTournament(0);

                tournament.AddUser(TestUtils.CreateUser("User1"));

                Assert.AreEqual(1, tournament.Teams.Count());
            }

            [TestMethod]
            public void TeamJoin_WithTournamentPassword_Success()
            {
                var tournament = CreateTournament(0);
                tournament.Options.NumberOfPlayersPerTeam = 2;
                tournament.Password = "TournamentSecret";

                var team = tournament.CreateTeam(TestUtils.CreateUser("User1"), "Team", null, "TournamentSecret");
                tournament.AddUser(TestUtils.CreateUser("User2"), team, null, "TournamentSecret");

                Assert.AreEqual(2, team.Participants.Count());
            }

            [TestMethod]
            [ExpectedDomainException(ErrorCode.TournamentIncorrectPassword)]
            public void CreateTeam_WithTournamentPassword_WrongPassword()
            {
                var tournament = CreateTournament(0);
                tournament.Options.NumberOfPlayersPerTeam = 2;
                tournament.Password = "TournamentSecret";

                tournament.CreateTeam(TestUtils.CreateUser("User1"), "Team", null, "WrongPassword");
            }

            [TestMethod]
            [ExpectedDomainException(ErrorCode.TournamentIncorrectPassword)]
            public void JoinTeam_WithTournamentPassword_WrongPassword()
            {
                var tournament = CreateTournament(0);
                tournament.Options.NumberOfPlayersPerTeam = 2;
                tournament.Password = "TournamentSecret";

                var team = tournament.CreateTeam(TestUtils.CreateUser("User1"), "Team", null, "TournamentSecret");
                tournament.AddUser(TestUtils.CreateUser("User2"), team, null, "WrongPassword");
            }

            [TestMethod]
            public void HasPassword_ReturnsTrueWhenPasswordSet()
            {
                var tournament = CreateTournament(0);
                tournament.Password = "Secret";

                Assert.IsTrue(tournament.HasPassword);
            }

            [TestMethod]
            public void HasPassword_ReturnsFalseWhenNoPassword()
            {
                var tournament = CreateTournament(0);

                Assert.IsFalse(tournament.HasPassword);
            }
        }

        [TestClass]
        public class Leave
        {
            [TestMethod]
            public void DeleteTeam_AllowedBeforeStarting()
            {
                var tournament = CreateTournament(0);
                var team = tournament.CreateTeam(TestUtils.CreateUser("User1"), "Team");

                Assert.IsTrue(tournament.CanChangeTeams);
            }

            [TestMethod]
            public void DeleteTeam_NotAllowedWhenStarted()
            {
                var tournament = CreateTournamentWithUsers(3, 8);
                tournament.Start(new TestRandomGen());

                Assert.IsFalse(tournament.CanChangeTeams);
            }

            [TestMethod]
            public void DeleteTeam_CreatorAllowed()
            {
                var tournament = CreateTournament(0);
                var user = TestUtils.CreateUser("User1");
                var team = tournament.CreateTeam(user, "Team");

                tournament.DeleteTeam(user, team);
            }

            [TestMethod]
            [ExpectedDomainException(ErrorCode.TournamentTeamDeleteNoPermission)]
            public void DeleteTeam_NotCreatorNotAllowed()
            {
                var tournament = CreateTournament(0);
                var team = tournament.CreateTeam(TestUtils.CreateUser("User1"), "Team");
                var user2 = TestUtils.CreateUser("User2");

                tournament.DeleteTeam(user2, team);
            }
        }

        [TestClass]
        public class Start
        {
            [TestMethod]
            public void WithGroupGames()
            {
                var tournament = CreateTournamentWithUsers(3, 8);

                tournament.Start(new TestRandomGen());

                Assert.AreEqual(TournamentState.Groups, tournament.State);
                Assert.AreEqual(DateTime.UtcNow.Ticks, tournament.StartOfTournament.Ticks, 100);
                Assert.AreEqual(2, tournament.Groups.Count());
                Assert.IsTrue(tournament.HasGroupPhase);
            }

            [TestMethod]
            public void WithoutGroupGames()
            {
                var tournament = CreateTournamentWithUsers(0);

                tournament.Start(new TestRandomGen());

                Assert.AreEqual(TournamentState.Knockout, tournament.State);
                Assert.AreEqual(DateTime.UtcNow.Ticks, tournament.StartOfTournament.Ticks, 100);
                Assert.AreEqual(1, tournament.Pairings.Count());
                Assert.IsFalse(tournament.HasGroupPhase);
            }
        }

        [TestClass]
        public class StartNextRound
        {
            [TestMethod]
            public void GroupToKnockout()
            {
                var tournament = CreateTournamentWithUsers(3, 8);
                tournament.Start(new TestRandomGen());

                foreach (var pairing in tournament.Pairings)
                {
                    pairing.TeamAWon = 2;
                    pairing.TeamBWon = 1;
                    pairing.State = PairingState.Done;
                }

                Assert.IsTrue(tournament.CanStartNextRound);

                tournament.StartNextRound(new TestRandomGen(), new TestLogger());

                Assert.AreEqual(TournamentState.Knockout, tournament.State);
                Assert.AreEqual(2, tournament.Pairings.Where(x => x.State == PairingState.None).Count());
            }

            [TestMethod]
            public void KnockoutToKnockout()
            {
                var tournament = CreateTournamentWithUsers(0, 8);
                tournament.Start(new TestRandomGen());

                foreach (var pairing in tournament.Pairings)
                {
                    pairing.TeamAWon = 2;
                    pairing.TeamBWon = 1;
                    pairing.State = PairingState.Done;
                }

                Assert.IsTrue(tournament.CanStartNextRound);

                tournament.StartNextRound(new TestRandomGen(), new TestLogger());

                Assert.AreEqual(TournamentState.Knockout, tournament.State);
                Assert.AreEqual(1, tournament.Phase);
                Assert.AreEqual(2, tournament.Pairings.Where(x => x.State == PairingState.None).Count());
            }

            [TestMethod]
            public void EndTournament()
            {
                var tournament = CreateTournamentWithUsers(0, 2);
                tournament.Start(new TestRandomGen());

                tournament.Teams.First().State = TournamentTeamState.InActive;

                Assert.IsTrue(tournament.CanEnd);

                tournament.End();

                Assert.AreEqual(TournamentState.Closed, tournament.State);
            }
        }

        private static Tournament CreateTournamentWithUsers(int numberOfGroupGames, int numberOfTeams = 2)
        {
            var tournament = CreateTournament(numberOfGroupGames, numberOfTeams);

            for (var i = 0; i < numberOfTeams; ++i)
            {
                tournament.AddUser(TestUtils.CreateUser($"User-{i}"));
            }

            return tournament;
        }

        private static Tournament CreateTournament(int numberOfGroupGames, int numberOfTeams = 2)
        {
            return new Tournament("Tournament", numberOfTeams, numberOfGroupGames, 3, 3, DateTime.UtcNow,
                DateTime.UtcNow, new GameOptions { NumberOfPlayersPerTeam = 1 });
        }
    }
}
