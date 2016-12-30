using AspNet.Security.OAuth.Validation;
using ImperaPlus.Application.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Controllers
{
    /// <summary>
    /// Provides actions to play the game. 
    /// </summary>
    [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("api/games/{gameId:long:min(1)}/history")]

    public class HistoryController : BaseController
    {
        private IGameService gameService;

        public HistoryController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        /// <summary>
        /// Gets the specified turn including the actions and current state of the map
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="turnId"></param>
        /// <returns></returns>
        [HttpGet("{turnId:long:min(1)}")]
        [ProducesResponseType(typeof(DTO.Games.History.HistoryTurn), 200)]
        public IActionResult GetTurn(long gameId, long turnId)
        {
            var historyTurn = this.gameService.Get(gameId, turnId);

            return this.Ok(historyTurn);
        }
    }
}