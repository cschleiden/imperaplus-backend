using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ImperaPlus.DTO.Games;
using ImperaPlus.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImperaPlus.TestSupport.Testdata;
using System;
using ImperaPlus.DTO.Games.Play;
using ImperaPlus.DTO.Games.Map;
using ImperaPlus.Integration.Tests.Support;
using ImperaPlus.DataAccess.ConvertedMaps;

namespace ImperaPlus.Integration.Tests
{
    [TestClass]
    public class GameEndpointTests : BaseIntegrationTest
    {       
        private const string BaseMapTemplate = "api/map/";
        private TestClient clientDefault;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            this.clientDefault = new TestClient(this.HttpClientDefault);
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod]
        [Integration]
        public async Task CreateAndDeleteGame()
        {
            this.Log("Create game");
            var gameSummary = await this.clientDefault.CreateGame(this.GetCreationOptions(this.GetGameName(), 2, 1));

            this.Log("Find game using other user");
            var otherUser = new TestClient(await ApiClient.GetAuthenticatedClient(1));
            await this.EnsureGameDoesShowInOpenList(otherUser, gameSummary.Id);

            this.Log("Delete game");
            await this.clientDefault.DeleteGame(gameSummary.Id);

            this.Log("Make sure game does not show up anymore");
            await this.EnsureGameDoesNotShowInOpenList(this.clientDefault, gameSummary.Id);
        }

        [TestMethod]
        [Integration]
        public async Task StartJoinAndCompleteGame()
        {
            await CreateAndPlayGameToEnd(this.GetCreationOptions(this.GetGameName(), 2, 1));
        }
        
        [TestMethod]
        [Integration]
        public async Task StartJoinAndCompleteGameWithBot()
        {
            this.Log("Create game");
            var gameCreationOptions = this.GetCreationOptions("BotGame", 2, 1);
            gameCreationOptions.AddBot = true;
            var gameSummary = await this.clientDefault.CreateGame(gameCreationOptions);
            Assert.AreEqual(GameState.Active, gameSummary.State);

            var game = await this.clientDefault.GetGame(gameSummary.Id);

            var turnCount = game.TurnCounter;
            if (gameSummary.CurrentPlayer.Name != "Bot")
            {
                this.Log("End turn to trigger bot");
                var result = await this.clientDefault.EndTurn();
                var game2 = await this.clientDefault.GetGame(gameSummary.Id);
                Assert.IsTrue(turnCount + 2 <= game2.TurnCounter);
            }
            else
            {
                this.Log("Bot made it's move already");
                Assert.IsTrue(2 <= turnCount);
            }
        }

        private GameCreationOptions GetCreationOptions(string name, int numberOfTeams, int numberOfPlayersPerTeam)
        {
            return new GameCreationOptions
            {
                Name = name,
                MapTemplate = TestMaps.TestMap().Name,
                TimeoutInSeconds = 60 * 60 * 24,
                NumberOfTeams = numberOfTeams,
                NumberOfPlayersPerTeam = numberOfPlayersPerTeam,
                AttacksPerTurn = 1,
                MovesPerTurn = 1,
                MapDistribution = MapDistribution.Default,
                MinUnitsPerCountry = 1,
                NewUnitsPerTurn = 3,
                MaximumNumberOfCards = 5,
                InitialCountryUnits = 1,
                VictoryConditions = new[] { VictoryConditionType.Survival },
                VisibilityModifier = new[] { VisibilityModifierType.None }
            };
        }

        private async Task CreateAndPlayGameToEnd(GameCreationOptions gameCreationOptions)
        {
            var gameHistory = new Dictionary<int, Game>();

            var userClients = new List<TestClient>();
            for (int i = 0; i < gameCreationOptions.NumberOfTeams * gameCreationOptions.NumberOfPlayersPerTeam - 1; ++i)
            {
                var httpClient = ApiClient.GetAuthenticatedClient(i + 1).Result;

                var userClient = new TestClient(httpClient);

                await userClient.Init();

                userClients.Add(userClient);
            }

            this.Log("Create game");
            var gameSummary = await this.clientDefault.CreateGame(gameCreationOptions);

            foreach (var userClient in userClients)
            {                
                this.Log("Find game");
                await this.EnsureGameDoesShowInOpenList(userClient, gameSummary.Id);

                this.Log("Join game for player");
                await userClient.JoinGame(gameSummary.Id);
            }

            this.Log("Make sure game has disappeared from open list");
            await this.EnsureGameDoesNotShowInOpenList(userClients.First(), gameSummary.Id);

            this.Log("Make sure game is now listed as active");
            IEnumerable<GameSummary> myGames = await clientDefault.GetMyGames();
            var gameSummary2 = myGames.FirstOrDefault(x => x.Id == gameSummary.Id);
            Assert.IsNotNull(gameSummary2);
            Assert.AreEqual(GameState.Active, gameSummary2.State);
            Assert.IsTrue(gameSummary2.Teams.Any(), "No teams in summary");
            Assert.IsTrue(gameSummary2.Teams.SelectMany(x => x.Players).Any(), "No players in teams");
            Assert.IsNotNull(gameSummary2.CurrentPlayer);

            this.Log("Get game for default player");
            var gameDefault = await this.clientDefault.GetGame(gameSummary.Id);
            Assert.IsNotNull(gameDefault.Teams);
            Assert.IsTrue(gameDefault.Teams.Any());
            Assert.IsNotNull(gameDefault.Map);
            Assert.AreEqual(PlayState.PlaceUnits, gameDefault.PlayState);

            this.Log("Get map template");
            var mapTemplateResponse = await this.HttpClientDefault.GetAsync(BaseMapTemplate + gameDefault.MapTemplate);
            mapTemplateResponse.AssertIsSuccessful();
            var mapTemplate = await mapTemplateResponse.Content.ReadAsAsync<DTO.Games.Map.MapTemplate>();

            while (gameDefault.State == GameState.Active)
            {
                bool placeOnlyTurn = false;

                this.Log("Begin of turn");

                var currentPlayerId = gameDefault.CurrentPlayer.Id;
                var currentTeamId = gameDefault.CurrentPlayer.TeamId;
                
                this.Log("\tCurrent player:{0} - {1}", currentPlayerId, currentTeamId);

                var playerClient = userClients.FirstOrDefault(x => x.UserName == gameDefault.CurrentPlayer.Name);
                if (playerClient == null)
                {
                    playerClient = this.clientDefault;
                }

                playerClient.PlayerId = gameDefault.CurrentPlayer.Id;

                {
                    // Place units
                    this.Log("Placing units - player {0} - {1}", currentPlayerId, gameDefault.UnitsToPlace);
                    var ownCountries = gameDefault.Map.Countries.Where(x => x.TeamId == currentTeamId);
                    Country ownCountry;
                    if (ownCountries.Count() == 1)
                    {
                        ownCountry = ownCountries.First();
                    }
                    else
                    {
                        ownCountry = ownCountries.FirstOrDefault(x =>
                           gameDefault.Map.Countries.Any(
                                y => y.TeamId != currentTeamId
                                                && mapTemplate
                                                .Connections
                                                .Any(c => c.Origin == x.Identifier && c.Destination == y.Identifier)));
                    }

                    if (ownCountry == null)
                    {
                        Assert.Fail("No connected, enemy country found");
                    }

                    var placeOptions = new[] {
                        new PlaceUnitsOptions
                        {
                            CountryIdentifier = ownCountry.Identifier,
                            NumberOfUnits = gameDefault.UnitsToPlace
                        }
                    };

                    var placeResponse = await playerClient.Place(placeOptions);
                    this.ApplyMapUpdates(gameDefault.Map, placeResponse.CountryUpdates);

                    if (placeResponse.State != GameState.Active)
                    {
                        break;
                    }

                    if (placeResponse.CurrentPlayer.Id != currentPlayerId)
                    {
                        this.Log("Place only turn");
                        placeOnlyTurn = true;
                    }
                }

                // Attack
                if (gameDefault.TurnCounter > 3)
                {
                    bool breakExecution = false;

                    for (int a = 0; a < gameDefault.Options.AttacksPerTurn; ++a)
                    {
                        var ownCountries = gameDefault.Map.Countries.Where(x => x.TeamId == currentTeamId);
                        var ownCountry = ownCountries.FirstOrDefault(x =>
                            x.Units > gameDefault.Options.MinUnitsPerCountry
                            && gameDefault.Map.Countries.Any(y => y.TeamId != currentTeamId
                                                && mapTemplate
                                                .Connections
                                                .Any(c => c.Origin == x.Identifier && c.Destination == y.Identifier)));
                        if (ownCountry == null)
                        {
                            this.Log("Cannot find own country");

                            // Abort attack
                            break;
                        }

                        // Find enemy country
                        var enemyCountries = gameDefault.Map.Countries.Where(x => x.TeamId != currentTeamId);
                        var enemyCountry = enemyCountries.FirstOrDefault(x => mapTemplate
                                                .Connections.Any(c =>
                                                c.Origin == ownCountry.Identifier
                                                && c.Destination == x.Identifier));
                        if (enemyCountry == null)
                        {
                            Assert.Fail("Cannot find enemy country connected to selected own country");
                        }

                        var numberOfUnits = ownCountry.Units - gameDefault.Options.MinUnitsPerCountry;
                        if (playerClient != this.clientDefault)
                        {
                            numberOfUnits = 1;
                        }

                        var attackOptions = new AttackOptions()
                        {
                            OriginCountryIdentifier = ownCountry.Identifier,
                            DestinationCountryIdentifier = enemyCountry.Identifier,
                            NumberOfUnits = numberOfUnits
                        };

                        this.Log("Attack from {0} to {1} with {2} units",
                            attackOptions.OriginCountryIdentifier,
                            attackOptions.DestinationCountryIdentifier,
                            attackOptions.NumberOfUnits);

                        var attackResult = await playerClient.Attack(attackOptions);

                        if (attackResult.ActionResult == ActionResult.Successful)
                        {
                            this.Log("\tAttack successful, units left {0}", attackResult.CountryUpdates.First(x => x.Identifier == attackOptions.DestinationCountryIdentifier).Units);
                        }
                        else
                        {
                            this.Log("\tAttack failed");
                        }

                        this.ApplyMapUpdates(gameDefault.Map, attackResult.CountryUpdates);

                        if (attackResult.State != GameState.Active)
                        {
                            breakExecution = true;
                            break;
                        }
                    }

                    if (breakExecution)
                    {
                        break;
                    }
                }

                // Move
                {
                }

                if (!placeOnlyTurn)
                {
                    // Record turn 
                    gameHistory.Add(gameDefault.TurnCounter, await this.clientDefault.GetGame(gameSummary.Id));

                    // End turn
                    this.Log("End turn");
                    await playerClient.EndTurn();
                }

                gameDefault = await this.clientDefault.GetGame(gameSummary.Id);
                if (gameDefault.State == GameState.Ended)
                {
                    break;
                }

                Assert.IsTrue(
                    gameDefault.CurrentPlayer.Id != currentPlayerId,
                    "Current player did not switch");

                if (gameDefault.TurnCounter > 50)
                {
                    foreach (var player in gameDefault.Teams.SelectMany(x => x.Players))
                    {
                        this.Log("Player {0} has {1} countries",
                            player.Name,
                            gameDefault.Map.Countries.Count(x => x.PlayerId == player.Id));
                    }

                    Assert.Inconclusive("Turn counter to high, possibly no end?");
                }
            }

            this.Log("Game ended");

            // Refresh
            gameDefault = await this.clientDefault.GetGame(gameSummary.Id);

            Assert.IsTrue(
                gameDefault.Teams.SelectMany(x => x.Players)
                .Any(x => x.Outcome == PlayerOutcome.Won
                && x.State == PlayerState.InActive),
                "No winner after game has ended");
            Assert.IsTrue(
                gameDefault.Teams.SelectMany(x => x.Players)
                .Any(x => x.Outcome == PlayerOutcome.Defeated
                && x.State == PlayerState.InActive),
                "No loser after game has ended");

            // Output debug information
            foreach (var player in gameDefault.Teams.SelectMany(x => x.Players))
            {
                this.Log("Player {0} result {1}", player.Name, player.Outcome);
            }

            this.Log("Verifying history");

            foreach(var gameHistoryEntry in gameHistory)
            {
                this.Log("Get history for turn {0}", gameHistoryEntry.Key);

                var historyTurn = await this.clientDefault.GetGame(gameHistoryEntry.Value.Id, gameHistoryEntry.Value.TurnCounter);

                // Verify players
                foreach(var player in gameHistoryEntry.Value.Teams.SelectMany(x => x.Players))
                {
                    var historyPlayer = historyTurn.Game.Teams.SelectMany(x => x.Players).FirstOrDefault(x => x.Id == player.Id);
                    Assert.IsNotNull(historyPlayer);

                    Assert.AreEqual(player.State, historyPlayer.State);
                    Assert.AreEqual(player.Outcome, historyPlayer.Outcome);
                }
                
                // Verify map
                foreach(var country in gameHistoryEntry.Value.Map.Countries)
                {
                    var historyCountry = historyTurn.Game.Map.Countries.FirstOrDefault(x => x.Identifier == country.Identifier);
                    Assert.IsNotNull(historyCountry);

                    Assert.AreEqual(country.Units, historyCountry.Units);
                    Assert.AreEqual(country.PlayerId, historyCountry.PlayerId);
                    Assert.AreEqual(country.TeamId, historyCountry.TeamId);
                }
            }
        }

        private void ApplyMapUpdates(DTO.Games.Map.Map map, IEnumerable<DTO.Games.Map.Country> countryUpdates)
        {
            foreach (var countryUpdate in countryUpdates)
            {
                var country = map.Countries.Single(x => x.Identifier == countryUpdate.Identifier);
                country.Units = countryUpdate.Units;
                country.PlayerId = countryUpdate.PlayerId;
                country.TeamId = countryUpdate.TeamId;
            }
        }

        private string GetGameName()
        {
            return string.Format("game-{0}", Guid.NewGuid());
        }

        private async Task EnsureGameDoesNotShowInOpenList(TestClient client, long gameId)
        {
            IEnumerable<GameSummary> open2Games = await client.GetOpenGames();
            Assert.IsFalse(open2Games.Any(x => x.Id == gameId), "Game does show in open list but should not");
        }

        private async Task EnsureGameDoesShowInOpenList(TestClient client, long gameId)
        {
            IEnumerable<GameSummary> open2Games = await client.GetOpenGames();
            Assert.IsTrue(open2Games.Any(x => x.Id == gameId), "Game does not show in open list");
        }
    }    
}