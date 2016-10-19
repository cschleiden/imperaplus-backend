using ImperaPlus.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace ImperaPlus.Integration.Tests
{
    [TestClass]
    public class LadderEndpointTests : BaseIntegrationTest
    {
        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();
        }

        [TestMethod]
        // TODO: Admin controller referenced doesn't exist anymore!
        [Ignore]
        public void NewLadderQueueAndCreate()
        {
            /*var player1GameClient = 
            var gamesBefore1 = await player1Client.GetMyGames();

            var player2HttpClient = await ApiClient.GetAuthenticatedClient(2);
            var player2Client = new TestClient(player2HttpClient);
            var gamesBefore2 = await player2Client.GetMyGames();

            this.Log("Admin: Create new ladder");
            var ladderCreationOptions = new DTO.Ladder.Admin.CreationOptions
            {
                Name = "1vs1",
                NumberOfTeams = 2,
                NumberOfPlayers = 1                
            };
            var createResponse = await this.HttpClientAdmin.PostAsJsonAsync("api/admin/ladder", ladderCreationOptions);
            createResponse.AssertIsSuccessful();
            var ladderSummary = await createResponse.Content.ReadAsAsync<LadderSummary>();
            this.Log(" Done.");

            this.Log("Admin: Set game options");
            (await this.HttpClientAdmin.PutAsJsonAsync("api/admin/ladder/" + ladderSummary.Id + "/gameOptions", new DTO.Games.GameOptions
                {
                    NumberOfPlayersPerTeam = 1,
                    NumberOfTeams = 2,
                    AttacksPerTurn = 3,
                    InitialCountryUnits = 3,
                    MapDistribution = DTO.Games.MapDistribution.Default,
                    MaximumNumberOfCards = 3,
                    MaximumTimeoutsPerPlayer = 1,
                    MinUnitsPerCountry = 1,
                    MovesPerTurn = 3,
                    NewUnitsPerTurn = 3,
                    TimeoutInSeconds = (int)TimeSpan.FromDays(1).TotalSeconds,
                    VictoryConditions = new[] { DTO.Games.VictoryConditionType.Survival },
                    VisibilityModifier = new[] { DTO.Games.VisibilityModifierType.None },
                })).AssertIsSuccessful();
            this.Log(" Done.");

            this.Log("Admin: Set map template");
            (await this.HttpClientAdmin.PutAsJsonAsync("api/admin/ladder/" + ladderSummary.Id + "/mapTemplates", new DTO.Ladder.Admin.MapTemplateUpdate
            {
                MapTemplateNames = new[] 
                {
                    TestMaps.TestMap().Name
                }
            })).AssertIsSuccessful();
            this.Log(" Done.");

            this.Log("Admin: Activate");
            (await this.HttpClientAdmin.PutAsJsonAsync("api/admin/ladder/" + ladderSummary.Id + "/active", true)).AssertIsSuccessful();
            this.Log(" Done.");

            this.Log("Player 1 - Get all ladders with standings");
            var ladderResp = await this.HttpClientDefault.GetAsync("api/ladder");
            var player1Ladders = await ladderResp.Content.ReadAsAsync<IEnumerable<LadderSummary>>();
            Assert.IsNotNull(player1Ladders, "Ladders should not be null");
            Assert.IsNull(player1Ladders.First().Standing, "Player should not have standing in new ladder");

            this.Log("Player 1 - Join Ladder");
            var joinResponse = await this.HttpClientDefault.PostAsync("api/ladder/" + ladderSummary.Id + "/queue", null);
            joinResponse.AssertIsSuccessful();
            this.Log(" Done.");

            this.Log("Player 2 - Join Ladder");            
            var joinResponse2 = await player2HttpClient.PostAsync("api/ladder/" + ladderSummary.Id + "/queue", null);
            joinResponse2.AssertIsSuccessful();
            this.Log(" Done.");

            this.Log("Force game generation");
            await this.HttpClientAdmin.PostAsync("api/admin/ladder/forceCreate", null);

            this.Log("Player 1 - Has game");
            var games1 = await player1Client.GetMyGames();        
            Assert.AreEqual(gamesBefore1.Count() + 1, games1.Count(), "Expected 1 game");
            Assert.AreEqual(games1.Last().Type, DTO.Games.GameType.Ranking, "Game is not ranking");
            Assert.AreEqual(games1.Last().LadderId, ladderSummary.Id);
            Assert.AreEqual(games1.Last().LadderName, ladderSummary.Name);

            this.Log("Player 2 - Has game");            
            var games2 = await player2Client.GetMyGames();
            Assert.AreEqual(gamesBefore2.Count() + 1, games2.Count(), "Expected 1 game");
            Assert.AreEqual(games2.Last().Type, DTO.Games.GameType.Ranking, "Game is not ranking");
            Assert.AreEqual(games2.Last().LadderId, ladderSummary.Id);
            Assert.AreEqual(games2.Last().LadderName, ladderSummary.Name);

            this.Log("Player 2 - Surrender game");
            await player2Client.Surrender(games2.Last().Id);

            this.Log("Player 1 - Get all ladders with standings");
            ladderResp = await this.HttpClientDefault.GetAsync("api/ladder");
            player1Ladders = await ladderResp.Content.ReadAsAsync<IEnumerable<LadderSummary>>();
            var player1Standing = player1Ladders.First(x => x.Id == ladderSummary.Id).Standing;
            Assert.IsNotNull(player1Standing, "Player should have standing now");

            this.Log("Player 2 - Get all ladders with standings");
            ladderResp = await player2HttpClient.GetAsync("api/ladder");
            var player2Ladders = await ladderResp.Content.ReadAsAsync<IEnumerable<LadderSummary>>();
            var player2Standing = player2Ladders.First(x => x.Id == ladderSummary.Id).Standing;
            Assert.IsNotNull(player2Standing, "Player should have standing now");

            Assert.AreEqual(1, player1Standing.GamesPlayed);
            Assert.AreEqual(1, player1Standing.GamesWon);
            Assert.AreEqual(1, player1Standing.Position, "Player 1 should be first");
            Assert.AreEqual(2, player2Standing.Position, "Player 2 should be second");
            Assert.IsTrue(player1Standing.Rating > player2Standing.Rating, "Player 1 should be above player 2");*/
        }
    }
}
