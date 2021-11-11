using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImperaPlus.DataAccess.ConvertedMaps;
using ImperaPlus.DTO.Games;
using ImperaPlus.DTO.Games.Map;
using ImperaPlus.DTO.Games.Play;
using ImperaPlus.GeneratedClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.IntegrationTests
{
    [TestClass]
    public class GameEndpointTests : BaseIntegrationTest
    {
        private const string BaseMapTemplate = "map/";
        private GameClient clientDefault;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            clientDefault = ApiClient.GetAuthenticatedClientDefaultUser<GameClient>().Result;
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod]
        public async Task CreateAndDeleteGame()
        {
            Log("Create game");
            var gameSummary = await clientDefault.PostAsync(GetCreationOptions(GetGameName(), 2, 1));

            Log("Find game using other user");
            var otherUser = await ApiClient.GetAuthenticatedClient<GameClient>(1);
            await EnsureGameDoesShowInOpenList(otherUser, gameSummary.Id);

            Log("Delete game");
            await clientDefault.DeleteAsync(gameSummary.Id);

            Log("Make sure game does not show up anymore");
            await EnsureGameDoesNotShowInOpenList(clientDefault, gameSummary.Id);
        }

        [TestMethod]
        public async Task StartJoinAndCompleteGame()
        {
            await CreateAndPlayGameToEnd(GetCreationOptions(GetGameName(), 2, 1));
        }

        [TestMethod]
        public async Task StartJoinAndCompleteGameWithBot()
        {
            Log("Create game");
            var gameCreationOptions = GetCreationOptions("BotGame" + Guid.NewGuid().ToString(), 2, 1);
            gameCreationOptions.AddBot = true;
            var gameSummary = await clientDefault.PostAsync(gameCreationOptions);
            Assert.AreEqual(GameState.Active, gameSummary.State);

            Thread.Sleep(4000);

            var game = await clientDefault.GetAsync(gameSummary.Id);
            var turnCount = game.TurnCounter;

            if (gameSummary.CurrentPlayer.Name != "Bot")
            {
                Log("End turn to trigger bot");
                var playClient = await ApiClient.GetAuthenticatedClientDefaultUser<PlayClient>();
                var result = await playClient.PostEndTurnAsync(gameSummary.Id);

                Thread.Sleep(4000);

                var game2 = await clientDefault.GetAsync(gameSummary.Id);
                Assert.IsTrue(turnCount + 2 <= game2.TurnCounter);
            }
            else
            {
                Log("Bot made it's move already");
                Assert.IsTrue(2 <= turnCount);
            }
        }

        private GameCreationOptions GetCreationOptions(string name, int numberOfTeams, int numberOfPlayersPerTeam)
        {
            return new GameCreationOptions
            {
                Name = name,
                MapTemplate = Maps.TestMap().Name,
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

            var defaultPlayClient = await ApiClient.GetAuthenticatedClientDefaultUser<PlayClient>();

            var gameClients = new List<Tuple<GameClient, PlayClient, string>>();
            for (var i = 0; i < gameCreationOptions.NumberOfTeams * gameCreationOptions.NumberOfPlayersPerTeam - 1; ++i)
            {
                var gameClient = await ApiClient.GetAuthenticatedClient<GameClient>(i + 1);
                var playClient = await ApiClient.GetAuthenticatedClient<PlayClient>(i + 1);
                gameClients.Add(Tuple.Create(gameClient, playClient, "TestUser" + (i + 1)));
            }

            Log("Create game");
            var gameSummary = await clientDefault.PostAsync(gameCreationOptions);

            foreach (var gameClient in gameClients)
            {
                Log("Find game");
                await EnsureGameDoesShowInOpenList(gameClient.Item1, gameSummary.Id);

                Log("Join game for player");
                await gameClient.Item1.PostJoinAsync(gameSummary.Id, null);
            }

            Log("Make sure game has disappeared from open list");
            await EnsureGameDoesNotShowInOpenList(gameClients.First().Item1, gameSummary.Id);

            Log("Make sure game is now listed as active");
            IEnumerable<GameSummary> myGames = await clientDefault.GetMyAsync();
            var gameSummary2 = myGames.FirstOrDefault(x => x.Id == gameSummary.Id);
            Assert.IsNotNull(gameSummary2);
            Assert.AreEqual(GameState.Active, gameSummary2.State);
            Assert.IsTrue(gameSummary2.Teams.Any(), "No teams in summary");
            Assert.IsTrue(gameSummary2.Teams.SelectMany(x => x.Players).Any(), "No players in teams");
            Assert.IsNotNull(gameSummary2.CurrentPlayer);

            Log("Get game for default player");
            var gameDefault = await clientDefault.GetAsync(gameSummary.Id);
            Assert.IsNotNull(gameDefault.Teams);
            Assert.IsTrue(gameDefault.Teams.Any());
            Assert.IsNotNull(gameDefault.Map);
            Assert.AreEqual(PlayState.PlaceUnits, gameDefault.PlayState);

            Log("Get map template");
            var mapTemplateClient = await ApiClient.GetClient<MapClient>();
            var mapTemplate = await mapTemplateClient.GetMapTemplateAsync(gameDefault.MapTemplate);

            while (gameDefault.State == GameState.Active)
            {
                var placeOnlyTurn = false;

                Log("Begin of turn");

                var currentPlayerId = gameDefault.CurrentPlayer.Id;
                var currentTeamId = gameDefault.CurrentPlayer.TeamId;

                Log("\tCurrent player:{0} - {1}", currentPlayerId, currentTeamId);

                PlayClient playClient;
                GameClient gameClient;
                var player = gameClients.FirstOrDefault(x => x.Item3 == gameDefault.CurrentPlayer.Name);
                if (player == null)
                {
                    gameClient = clientDefault;
                    playClient = defaultPlayClient;
                }
                else
                {
                    gameClient = player.Item1;
                    playClient = player.Item2;
                }

                var gameState = await gameClient.GetAsync(gameDefault.Id);

                {
                    // Place units
                    Log("Placing units - player {0} - {1}", currentPlayerId, gameDefault.UnitsToPlace);
                    var ownCountries = gameState.Map.Countries.Where(x => x.TeamId == currentTeamId);
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

                    var placeOptions = new[]
                    {
                        new PlaceUnitsOptions
                        {
                            CountryIdentifier = ownCountry.Identifier, NumberOfUnits = gameState.UnitsToPlace
                        }
                    };

                    var placeResponse = await playClient.PostPlaceAsync(gameDefault.Id, placeOptions);
                    ApplyMapUpdates(gameState.Map, placeResponse.CountryUpdates);

                    if (placeResponse.State != GameState.Active)
                    {
                        break;
                    }

                    if (placeResponse.CurrentPlayer.Id != currentPlayerId)
                    {
                        Log("Place only turn");
                        placeOnlyTurn = true;
                    }
                }

                // Attack
                if (gameState.TurnCounter > 3)
                {
                    var breakExecution = false;

                    for (var a = 0; a < gameState.Options.AttacksPerTurn; ++a)
                    {
                        var ownCountries = gameState.Map.Countries.Where(x => x.TeamId == currentTeamId);
                        var ownCountry = ownCountries.FirstOrDefault(x =>
                            x.Units > gameState.Options.MinUnitsPerCountry
                            && gameState.Map.Countries.Any(y => y.TeamId != currentTeamId
                                                                && mapTemplate
                                                                    .Connections
                                                                    .Any(c => c.Origin == x.Identifier &&
                                                                              c.Destination == y.Identifier)));
                        if (ownCountry == null)
                        {
                            Log("Cannot find own country");

                            // Abort attack
                            break;
                        }

                        // Find enemy country
                        var enemyCountries = gameState.Map.Countries.Where(x => x.TeamId != currentTeamId);
                        var enemyCountry = enemyCountries.FirstOrDefault(x => mapTemplate
                            .Connections.Any(c =>
                                c.Origin == ownCountry.Identifier
                                && c.Destination == x.Identifier));
                        if (enemyCountry == null)
                        {
                            Assert.Fail("Cannot find enemy country connected to selected own country");
                        }

                        var numberOfUnits = ownCountry.Units - gameState.Options.MinUnitsPerCountry;
                        if (playClient != defaultPlayClient)
                        {
                            numberOfUnits = 1;
                        }

                        var attackOptions = new AttackOptions()
                        {
                            OriginCountryIdentifier = ownCountry.Identifier,
                            DestinationCountryIdentifier = enemyCountry.Identifier,
                            NumberOfUnits = numberOfUnits
                        };

                        Log("Attack from {0} to {1} with {2} units",
                            attackOptions.OriginCountryIdentifier,
                            attackOptions.DestinationCountryIdentifier,
                            attackOptions.NumberOfUnits);

                        var attackResult = await playClient.PostAttackAsync(gameState.Id, attackOptions);

                        if (attackResult.ActionResult == Result.Successful)
                        {
                            Log("\tAttack successful, units left {0}",
                                attackResult.CountryUpdates.First(x =>
                                    x.Identifier == attackOptions.DestinationCountryIdentifier).Units);
                        }
                        else
                        {
                            Log("\tAttack failed");
                        }

                        ApplyMapUpdates(gameState.Map, attackResult.CountryUpdates);

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
                    gameHistory.Add(gameState.TurnCounter, await gameClient.GetAsync(gameSummary.Id));

                    // End turn
                    Log("End turn");
                    await playClient.PostEndTurnAsync(gameState.Id);
                }

                gameDefault = await clientDefault.GetAsync(gameSummary.Id);
                if (gameDefault.State == GameState.Ended)
                {
                    break;
                }

                Assert.IsTrue(
                    gameDefault.CurrentPlayer.Id != currentPlayerId,
                    "Current player did not switch");

                if (gameDefault.TurnCounter > 50)
                {
                    foreach (var p in gameDefault.Teams.SelectMany(x => x.Players))
                    {
                        Log("Player {0} has {1} countries",
                            p.Name,
                            gameDefault.Map.Countries.Count(x => x.PlayerId == p.Id));
                    }

                    Assert.Inconclusive("Turn counter to high, possibly no end?");
                }
            }

            Log("Game ended");

            // Refresh
            gameDefault = await clientDefault.GetAsync(gameSummary.Id);

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
                Log("Player {0} result {1}", player.Name, player.Outcome);
            }

            Log("Verifying history");

            var historyClient = await ApiClient.GetAuthenticatedClientDefaultUser<HistoryClient>();

            foreach (var gameHistoryEntry in gameHistory)
            {
                Log("Get history for turn {0}", gameHistoryEntry.Key);

                var historyTurn =
                    await historyClient.GetTurnAsync(gameHistoryEntry.Value.Id, gameHistoryEntry.Value.TurnCounter);

                // Verify players
                foreach (var player in gameHistoryEntry.Value.Teams.SelectMany(x => x.Players))
                {
                    var historyPlayer = historyTurn.Game.Teams.SelectMany(x => x.Players)
                        .FirstOrDefault(x => x.Id == player.Id);
                    Assert.IsNotNull(historyPlayer);

                    Assert.AreEqual(player.State, historyPlayer.State);
                    Assert.AreEqual(player.Outcome, historyPlayer.Outcome);
                }

                // Verify map
                foreach (var country in gameHistoryEntry.Value.Map.Countries)
                {
                    var historyCountry =
                        historyTurn.Game.Map.Countries.FirstOrDefault(x => x.Identifier == country.Identifier);
                    Assert.IsNotNull(historyCountry);

                    Assert.AreEqual(country.Units, historyCountry.Units);
                    Assert.AreEqual(country.PlayerId, historyCountry.PlayerId);
                    Assert.AreEqual(country.TeamId, historyCountry.TeamId);
                }
            }
        }

        private void ApplyMapUpdates(Map map, IEnumerable<Country> countryUpdates)
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

        private async Task EnsureGameDoesNotShowInOpenList(GameClient client, long gameId)
        {
            IEnumerable<GameSummary> open2Games = await client.GetAllAsync();
            Assert.IsFalse(open2Games.Any(x => x.Id == gameId), "Game does show in open list but should not");
        }

        private async Task EnsureGameDoesShowInOpenList(GameClient client, long gameId)
        {
            IEnumerable<GameSummary> open2Games = await client.GetAllAsync();
            Assert.IsTrue(open2Games.Any(x => x.Id == gameId), "Game does not show in open list");
        }
    }
}
