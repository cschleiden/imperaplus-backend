using System.Collections.Generic;
using AspNet.Security.OAuth.Validation;
using ImperaPlus.Application.Games;
using ImperaPlus.Domain.Utilities;
using ImperaPlus.DTO;
using ImperaPlus.DTO.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Controllers
{
    /// <summary>
    /// General management of games
    /// </summary>
    [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/games")]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(void), 200)]
    public class GameController : Controller
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
        [HttpGet("open")]
        public IEnumerable<GameSummary> GetAll()
        {
            return this.gameService.GetOpen();
        }

        /// <summary>
        /// Get a list of the games for the current player
        /// </summary>
        /// <returns>List of games for the current user</returns>
        [HttpGet("my")]
        public IEnumerable<GameSummary> GetMy()
        {
            return this.gameService.GetForCurrentUser();
        }

        /// <summary>
        /// Get list of games where it's the current player's team
        /// </summary>
        /// <returns>List of games where it's the current user's team</returns>
        [HttpGet("myturn")]
        public IEnumerable<GameSummary> GetMyTurn()
        {
            return this.gameService.GetForCurrentUserTurn();
        }

        /// <summary>
        /// Create a new game
        /// </summary>
        /// <param name="creationOptions">Creation options</param>
        /// <returns>Summary of newly created game</returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(GameSummary), 200)]
        public IActionResult Post([FromBody] GameCreationOptions creationOptions)
        {
            Require.NotNull(creationOptions, nameof(creationOptions));

            var game = this.gameService.Create(creationOptions);

            return this.Ok(game);
        }

        /// <summary>
        /// Get detailed information about a single game
        /// </summary>
        /// <param name="gameId">Id of the requested game</param>
        /// <returns>Information about the requested game</returns>
        [HttpGet("{gameId:long:min(1)}")]
        [ProducesResponseType(typeof(Game), 200)]
        public IActionResult Get(long gameId)
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
        [HttpGet("{gameId:long:min(1)}/messages")]
        [ProducesResponseType(typeof(Game), 200)]
        public IActionResult GetMessages(long gameId, bool isPublic = true)
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
        [HttpDelete("{gameId:long:min(1)}")]
        public IActionResult Delete(long gameId)
        {
            this.gameService.Delete(gameId);

            return this.Ok();
        }

        /// <summary>
        /// Join the given game
        /// </summary>
        /// <param name="gameId">Id of game to join</param>
        [HttpPost("{gameId:long:min(1)}/join")]
        public IActionResult PostJoin(long gameId)
        {
            this.gameService.Join(gameId);

            return this.Ok();
        }

        /// <summary>
        /// Leave the given game, only possible if game hasn't started yet, and current player
        /// is not the creator.
        /// </summary>
        /// <param name="gameId">Id of game to leave</param>
        [HttpPost("{gameId:long:min(1)}/leave")]
        public IActionResult PostLeave(long gameId)
        {
            this.gameService.Leave(gameId);

            return this.Ok();
        }

        /// <summary>
        /// Surrender in the given game, only possible if current player
        /// and game are still active.
        /// </summary>
        /// <param name="gameId">Id of game to surrender in</param>
        [HttpPost("{gameId:long:min(1)}/surrender")]
        [ProducesResponseType(typeof(GameSummary), 200)]
        public IActionResult PostSurrender(long gameId)
        {
            var gameSummary = this.gameService.Surrender(gameId);

            return this.Ok(gameSummary);
        }

        /// <summary>
        /// Hides the given game for the current player
        /// </summary>
        /// <param name="gameId">Id of game to hide</param>
        [HttpPatch("{gameId:long:min(1)}/hide")]
        public IActionResult PatchHide(long gameId)
        {
            this.gameService.Hide(gameId);

            return this.Ok();
        }

        /// <summary>
        /// Hide all games which can be hidden for the current player
        /// </summary>
        [HttpPatch("hide")]
        public IActionResult PatchHideAll()
        {
            return this.Ok(this.gameService.HideAll());
        }
    }
}
