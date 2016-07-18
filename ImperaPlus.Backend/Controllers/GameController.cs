using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using ImperaPlus.Application.Games;
using ImperaPlus.DTO.Games;

namespace ImperaPlus.Backend.Controllers
{
    /// <summary>
    /// General management of games
    /// </summary>
    [Authorize]
    [RoutePrefix("api/games")]
    public class GameController : ApiController
    {
        private readonly IGameService gameService;

        /// <summary>
        /// Initialize a new instance of the GameController
        /// </summary>
        /// <param name="gameService">Game Service</param>
        public GameController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        /// <summary>
        /// Get a list of open games, excluding games by the current player
        /// </summary>
        /// <returns>List of games</returns>
        [Route("open")]
        public IEnumerable<GameSummary> GetAll()
        {
            return this.gameService.GetOpen();
        }

        /// <summary>
        /// Get a list of the games for the current player
        /// </summary>
        /// <returns>List of games for the current user</returns>
        [Route("my")]
        public IEnumerable<GameSummary> GetMy()
        {
            return this.gameService.GetForCurrentUser();
        }

        /// <summary>
        /// Get list of games where it's the current player's team
        /// </summary>
        /// <returns>List of games where it's the current user's team</returns>
        [Route("myturn")]
        public IEnumerable<GameSummary> GetMyTurn()
        {
            return this.gameService.GetForCurrentUserTurn();
        }

        /// <summary>
        /// Create a new game
        /// </summary>
        /// <param name="creationOptions">Creation options</param>
        /// <returns>Summary of newly created game</returns>
        [Route("")]
        [ResponseType(typeof(GameSummary))]
        public IHttpActionResult Post(GameCreationOptions creationOptions)
        {
            var game = this.gameService.Create(creationOptions);

            return this.Ok(game);
        }

        /// <summary>
        /// Get detailed information about a single game
        /// </summary>
        /// <param name="gameId">Id of the requested game</param>
        /// <returns>Information about the requested game</returns>
        [Route("{gameId:long:min(1)}")]
        [ResponseType(typeof(Game))]
        public IHttpActionResult Get(long gameId)
        {
            var game = this.gameService.Get(gameId);

            return this.Ok(game);
        }

        /// <summary>
        /// Get messages for a single game
        /// </summary>
        /// <param name="gameId">Id of the requested game</param>
        /// <param name="isPublic">Value indicating whether to return only public messages, default is true</param>
        /// <returns>Messages posted in the requested game</returns>
        [Route("{gameId:long:min(1)}/messages")]
        [ResponseType(typeof(Game))]
        public IHttpActionResult GetMessages(long gameId, bool isPublic = true)
        {
            var messages = this.gameService.GetMessages(gameId, isPublic);

            return this.Ok(messages);
        }

        /// <summary>
        /// Cancel/delete the requested game, if possible. 
        /// </summary>
        /// <remarks>
        /// This is only posssible, if the requested game is in a state that
        /// can be deleted
        /// </remarks>
        /// <param name="gameId">Id of the game to delete</param>
        /// <returns>Status</returns>
        [Route("{gameId:long:min(1)}")]
        public IHttpActionResult Delete(long gameId)
        {
            this.gameService.Delete(gameId);

            return this.Ok();
        }

        /// <summary>
        /// Join the given game
        /// </summary>
        /// <param name="gameId">Id of game to join</param>
        [Route("{gameId:long:min(1)}/join")]
        public IHttpActionResult PostJoin(long gameId)
        {
            this.gameService.Join(gameId);

            return this.Ok();
        }

        /// <summary>
        /// Leave the given game, only possible if game hasn't started yet, and current player
        /// is not the creator.
        /// </summary>
        /// <param name="gameId">Id of game to leave</param>
        [Route("{gameId:long:min(1)}/leave")]
        public IHttpActionResult PostLeave(long gameId)
        {
            this.gameService.Leave(gameId);

            return this.Ok();
        }

        /// <summary>
        /// Surrender in the given game, only possible if current player
        /// and game are still active.
        /// </summary>
        /// <param name="gameId">Id of game to surrender in</param>
        [Route("{gameId:long:min(1)}/surrender")]
        [ResponseType(typeof(GameSummary))]
        public IHttpActionResult PostSurrender(long gameId)
        {
            var gameSummary = this.gameService.Surrender(gameId);

            return this.Ok(gameSummary);
        }

        /// <summary>
        /// Hides the given game for the current player
        /// </summary>
        /// <param name="gameId">Id of game to hide</param>
        [Route("{gameId:long:min(1)}/hide")]
        public IHttpActionResult PatchHide(long gameId)
        {
            this.gameService.Hide(gameId);

            return this.Ok();
        }

        /// <summary>
        /// Hide all games which can be hidden for the current player
        /// </summary>
        [Route("hide")]
        public IHttpActionResult PatchHideAll()
        {
            return this.Ok(this.gameService.HideAll());
        }
    }
}
