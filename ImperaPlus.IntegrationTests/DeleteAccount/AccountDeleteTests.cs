using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ImperaPlus.Backend.Areas.Admin.Helpers;
using ImperaPlus.DTO.Account;
using ImperaPlus.GeneratedClient;
using ImperaPlus.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.IntegrationTests
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

            TestSetup.RegisterClient(99);
            defaultAccountClient = ApiClient.GetAuthenticatedClient<AccountClient>(99).Result;
            otherAccountClient = ApiClient.GetAuthenticatedClient<AccountClient>(1).Result;

            defaultUser = defaultAccountClient.GetUserInfoAsync().Result;
            otherUser = otherAccountClient.GetUserInfoAsync().Result;
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
            await SetupAccount();

            // Act
            await defaultAccountClient.DeleteAccountAsync(new DeleteAccountBindingModel
            {
                Password = ApiClient.GetUserPassword(99)
            });

            await new Application.Jobs.UserCleanupJob(Startup.Container).Handle(null);
        }

        private async Task SetupAccount()
        {
            await SendMessages();
            await JoinLadder();
            await JoinTournament();
            await JoinGame();
        }

        private async Task SendMessages()
        {
            var messageClient = await ApiClient.GetAuthenticatedClientDefaultUser<MessageClient>();

            // Send message to other user
            await messageClient.PostSendAsync(new DTO.Messages.SendMessage
            {
                To = new DTO.Users.UserReference { Id = otherUser.UserId, Name = otherUser.UserName },
                Subject = "Test",
                Text = "Test"
            });

            // Send message to self
            await messageClient.PostSendAsync(new DTO.Messages.SendMessage
            {
                To = new DTO.Users.UserReference { Id = defaultUser.UserId, Name = defaultUser.UserName },
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
            await CreateAndJoinTournament(2);

            // Single player
            await CreateAndJoinTournament(1);
        }

        private async Task CreateAndJoinTournament(int numberOfPlayersPerTeam)
        {
            var tournamentService = Startup.Container.Resolve<Application.Tournaments.ITournamentService>();

            var options = new DTO.Games.GameOptions
            {
                NumberOfTeams = 2, NumberOfPlayersPerTeam = numberOfPlayersPerTeam
            };
            GameOptionsHelper.SetDefaultGameOptions(options);

            var teamTournamentId = await tournamentService.Create(new DTO.Tournaments.Tournament
            {
                Name = "TestTournament" + Guid.NewGuid().ToString(),
                StartOfTournament = DateTime.UtcNow,
                StartOfRegistration = DateTime.UtcNow,
                Options = options,
                MapTemplates = new[] { "WorldDeluxe" },
                NumberOfGroupGames = 3,
                NumberOfKnockoutGames = 3,
                NumberOfFinalGames = 3,
                NumberOfTeams = 8
            });

            // Join tournament
            var tournamentClient = await ApiClient.GetAuthenticatedClient<TournamentClient>(0);
            await tournamentClient.PostJoinAsync(teamTournamentId, null);
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
