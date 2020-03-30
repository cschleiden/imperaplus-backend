using ImperaPlus.Application.Games;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace ImperaPlus.Backend.Controllers
{
    /// <summary>
    /// Provides actions to play the game. 
    /// </summary>
    [Authorize]
    [Route("api/games/{gameId:long:min(1)}/history")]
    [ProducesResponseType(typeof(ErrorResponse), 400)]

    public class HistoryController : BaseController
    {
        private IGameService gameService;

        public HistoryController(IUnitOfWork unitOfWork, IMapper mapper, IGameService gameService)
            : base(unitOfWork, mapper)
        {
            this.gameService = gameService;
        }

        /// <summary>
        /// Gets the specified turn including the actions and current state of the map
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="turnId"></param>
        /// <returns></returns>
        [HttpGet("{turnId:long:min(0)}")]
        [ProducesResponseType(typeof(DTO.Games.History.HistoryTurn), 200)]
        public IActionResult GetTurn(long gameId, long turnId)
        {
            var historyTurn = this.gameService.Get(gameId, turnId);

            return this.Ok(historyTurn);
        }
    }
}