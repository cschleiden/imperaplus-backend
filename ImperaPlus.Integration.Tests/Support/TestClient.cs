using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using ImperaPlus.DTO;
using ImperaPlus.DTO.Games;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImperaPlus.DTO.Games.Play;
using ImperaPlus.DTO.Games.History;
using System;

namespace ImperaPlus.Integration.Tests.Support
{
    public class TestClient
    {
        private const string Base = "api/games/";
        private const string GetOpen = "open";
        private const string GetMy = "my";

        private const string BasePlay = "api/games/{0}/play/";
        private const string PlaceAction = "place";
        private const string AttackAction = "attack";
        private const string MoveAction = "move";
        private const string EndTurnAction = "endturn";

        private const string BaseAccount = "api/Account/";

        private const string UserInfo = "userInfo";

        private HttpClient httpClient;

        public TestClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public long GameId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public Guid PlayerId { get; set; }

        public async Task Init()
        {
            var userInfoResult = await this.httpClient.GetAsync(BaseAccount + UserInfo);
            var userInfo = await userInfoResult.Content.ReadAsAsync<DTO.Account.UserInfo>();

            this.UserId = userInfo.UserId;
            this.UserName = userInfo.UserName;
        }

        public async Task<GameSummary> CreateGame(GameCreationOptions gameCreationOptions)
        {
            var createResponse = await this.httpClient.PostAsJsonAsync(Base, gameCreationOptions);
            createResponse.AssertIsSuccessful();
            var gameSummary = await createResponse.Content.ReadAsAsync<GameSummary>();

            this.GameId = gameSummary.Id;

            return gameSummary;
        }

        public async Task DeleteGame(long gameId)
        {
            var deleteResponse = await this.httpClient.DeleteAsync(Base + gameId);
            deleteResponse.AssertIsSuccessful();
        }

        internal async Task Surrender(long gameId)
        {
            (await this.httpClient.PostAsync(Base + gameId + "/surrender", null)).AssertIsSuccessful();
        }

        public async Task<Game> GetGame(long gameId)
        {
            var gameDefaultResponse = await this.httpClient.GetAsync(Base + gameId);
            return await gameDefaultResponse.Content.ReadAsAsync<Game>();
        }

        public async Task<HistoryTurn> GetGame(long gameId, long turnNo)
        {
            var turnResponse = await this.httpClient.GetAsync(Base + gameId + "/history/" + turnNo);

            return await turnResponse.Content.ReadAsAsync<HistoryTurn>();
        }

        public async Task<GameActionResult> Place(DTO.Games.Play.PlaceUnitsOptions[] placeOptions)
        {
            var placeResponse = await this.httpClient.PostAsJsonAsync(this.GetBaseUri() + PlaceAction, placeOptions);
            placeResponse.AssertIsSuccessful();
            var gameActionResult = await placeResponse.Content.ReadAsAsync<GameActionResult>();

            Assert.IsNotNull(gameActionResult.CountryUpdates);
            foreach (var placeOption in placeOptions)
            {
                Assert.IsTrue(gameActionResult
                    .CountryUpdates
                    .Any(x => x.Identifier == placeOption.CountryIdentifier),
                    "Country where unit was places is not included in updated countries");
            }

            return gameActionResult;
        }

        public async Task<GameActionResult> Attack(AttackOptions attackOptions)
        {
            var attackResponse = await this.httpClient.PostAsJsonAsync(this.GetBaseUri() + AttackAction, attackOptions);
            attackResponse.AssertIsSuccessful();
            var gameActionResult = await attackResponse.Content.ReadAsAsync<GameActionResult>();

            if (gameActionResult.ActionResult == ActionResult.Successful)
            {
                Assert.IsTrue(gameActionResult.CountryUpdates.Any(x =>
                    x.Identifier == attackOptions.DestinationCountryIdentifier
                    && x.PlayerId == this.PlayerId));
            }
            else
            {
                Assert.IsFalse(gameActionResult.CountryUpdates.Any(x =>
                    x.Identifier == attackOptions.DestinationCountryIdentifier
                    && x.PlayerId == this.PlayerId));
            }

            return gameActionResult;
        }

        public async Task<GameActionResult> Move(MoveOptions moveOptions)
        {
            var moveResponse = await this.httpClient.PostAsJsonAsync(this.GetBaseUri() + MoveAction, moveOptions);
            moveResponse.AssertIsSuccessful();

            var gameActionResult = await moveResponse.Content.ReadAsAsync<GameActionResult>();
            return gameActionResult;
        }

        public async Task<GameActionResult> EndTurn()
        {
            var endTurnResponse = await this.httpClient.PostAsync(this.GetBaseUri() + EndTurnAction, new StringContent(string.Empty));
            endTurnResponse.AssertIsSuccessful();

            var gameActionResult = await endTurnResponse.Content.ReadAsAsync<GameActionResult>();
            return gameActionResult;
        }

        private string GetBaseUri()
        {
            return string.Format(BasePlay, this.GameId);
        }

        public async Task JoinGame(long gameId)
        {
            this.GameId = gameId;

            var joinResponse = await this.httpClient.PostAsync(Base + this.GameId + "/join", null);
            joinResponse.AssertIsSuccessful();
        }

        public async Task<IEnumerable<GameSummary>> GetOpenGames()
        {
            var openResponse = await this.httpClient.GetAsync(Base + GetOpen);
            openResponse.AssertIsSuccessful();
            var openGames = await openResponse.Content.ReadAsAsync<IEnumerable<GameSummary>>();
            return openGames;
        }

        public async Task<IEnumerable<GameSummary>> GetMyGames()
        {
            var myResponse = await this.httpClient.GetAsync(Base + GetMy);
            myResponse.AssertIsSuccessful();
            var myGames = await myResponse.Content.ReadAsAsync<IEnumerable<GameSummary>>();
            return myGames;
        }
    }
}
