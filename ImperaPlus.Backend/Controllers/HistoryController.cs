using ImperaPlus.Application.Games;
using ImperaPlus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace ImperaPlus.Backend.Controllers
{
    /// <summary>
    /// Provides actions to play the game. 
    /// </summary>
    [Authorize]
    [RoutePrefix("api/games/{gameId:long:min(1)}/history")]

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
        [Route("{turnId:long:min(1)}")]
        [ResponseType(typeof(DTO.Games.History.HistoryTurn))]
        public IHttpActionResult GetTurn(long gameId, long turnId)
        {
            var historyTurn = this.gameService.Get(gameId, turnId);

            return this.Ok(historyTurn);
        }
    }
}