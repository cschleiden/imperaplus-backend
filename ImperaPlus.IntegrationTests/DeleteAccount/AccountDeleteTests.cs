using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ImperaPlus.Backend.Areas.Admin.Helpers;
using ImperaPlus.DTO.Account;
using ImperaPlus.GeneratedClient;
using ImperaPlus.Integration.Tests;
using ImperaPlus.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.IntegrationTests.DeleteAccount
{
    [TestClass]
    public class AccountDeleteTests : BaseIntegrationTest
    {
        private AccountClient defaultAccountClient;
        private UserInfo defaultUser;

        private AccountClient otherAccountClient;
        private UserInfo otherUser;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            this.defaultAccountClient = ApiClient.GetAuthenticatedClientDefaultUser<AccountClient>().Result;
            this.otherAccountClient = ApiClient.GetAuthenticatedClient<AccountClient>(1).Result;

            this.defaultUser = this.defaultAccountClient.GetUserInfoAsync().Result;
            this.otherUser = this.otherAccountClient.GetUserInfoAsync().Result;
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod]
        public async Task DeleteAcccount()
        {
            // Arrange
            await this.SetupAccount();

            // Act
            await this.defaultAccountClient.DeleteAccountAsync(new DeleteAccountBindingModel
            {
                Password = ApiClient.GetUserPassword(0)
            });

            await new Application.Jobs.UserCleanupJob(Startup.Container).Handle();            
        }

        private async Task SetupAccount()
        {
            await this.SendMessages();
            await this.JoinLadder();
            await this.JoinTournament();
            await this.JoinGame();
        }

        private async Task SendMessages()
        {
            var messageClient = await ApiClient.GetAuthenticatedClientDefaultUser<MessageClient>();

            // Send message to other user
            await messageClient.PostSendAsync(new DTO.Messages.SendMessage
            {
                To = new DTO.Users.UserReference
                {
                    Id = this.otherUser.UserId,
                    Name = this.otherUser.UserName
                },

                Subject = "Test",

                Text = "Test"
            });

            // Send message to self
            await messageClient.PostSendAsync(new DTO.Messages.SendMessage
            {
                To = new DTO.Users.UserReference
                {
                    Id = this.defaultUser.UserId,
                    Name = this.defaultUser.UserName
                },

                Subject = "Test",

                Text = "Test"
            });
        }

        private async Task JoinLadder()
        {
            var ladderClient = await ApiClient.GetAuthenticatedClient<LadderClient>(0);

            var ladders = await ladderClient.GetAllAsync();
            var ladder = ladders.First();
            await ladderClient.PostJoinAsync(ladder.Id);
        }

        private async Task JoinTournament()
        {
            // Team
            await this.CreateAndJoinTournament(2);

            // Single player
            await this.CreateAndJoinTournament(1);
        }

        private async Task CreateAndJoinTournament(int numberOfPlayersPerTeam)
        {
            var tournamentService = Startup.Container.Resolve<Application.Tournaments.ITournamentService>();

            var options = new DTO.Games.GameOptions
            {
                NumberOfTeams = 2,
                NumberOfPlayersPerTeam = numberOfPlayersPerTeam
            };
            GameOptionsHelper.SetDefaultGameOptions(options);

            var teamTournamentId = await tournamentService.Create(new DTO.Tournaments.Tournament
            {
                Name = "TestTournament" + Guid.NewGuid().ToString(),
                StartOfTournament = DateTime.UtcNow,
                StartOfRegistration = DateTime.UtcNow,
                Options = options,
                MapTemplates = new[]
                {
                    "WorldDeluxe"
                },
                NumberOfGroupGames = 3,
                NumberOfKnockoutGames = 3,
                NumberOfFinalGames = 3,
                NumberOfTeams = 8
            });

            // Join tournament
            var tournamentClient = await ApiClient.GetAuthenticatedClient<TournamentClient>(0);
            await tournamentClient.PostJoinAsync(teamTournamentId);
        }

        private async Task JoinGame()
        {
            var gameClient = await ApiClient.GetAuthenticatedClientDefaultUser<GameClient>();

            var options = new DTO.Games.GameCreationOptions();
            GameOptionsHelper.SetDefaultGameOptions(options);
            options.NumberOfPlayersPerTeam = 1;
            options.NumberOfTeams = 2;
            options.AddBot = true;
            options.Name = Guid.NewGuid().ToString();
            options.MapTemplate = "WorldDeluxe";

            var game = await gameClient.PostAsync(options);
        }
    }
}
