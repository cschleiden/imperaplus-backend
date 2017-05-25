using System;
using System.Collections.Generic;
using ImperaPlus.Application.Ladder;
using ImperaPlus.DTO.Ladder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ImperaPlus.DTO;

namespace ImperaPlus.Backend.Controllers
{
    /// <summary>
    /// Ladder interaction
    /// </summary>
    [Authorize]
    [Route("api/ladder")]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(void), 200)]
    public class LadderController : Controller
    {
        private ILadderService ladderService;

        public LadderController(ILadderService ladderService)
        {
            this.ladderService = ladderService;
        }

        /// <summary>
        /// Returns active ladders
        /// </summary>
        /// <returns>List of ladders</returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<LadderSummary>), 200)]
        public IEnumerable<LadderSummary> Get()
        {
            return this.ladderService.GetAll();
        }

        /// <summary>
        /// Gets ladder identified by given id
        /// </summary>
        /// <param name="ladderId">Id of ladder</param>        
        [HttpGet("{ladderId:guid}")]
        [ProducesResponseType(typeof(Ladder), 200)]
        public IActionResult Get(Guid ladderId)
        {
            return this.Ok(this.ladderService.Get(ladderId));
        }

        /// <summary>
        /// Queue up for a new game in the given ladder
        /// </summary>
        /// <param name="ladderId">Ladder id</param>
        [HttpPost("{ladderId:guid}/queue")]
        public IActionResult PostJoin(Guid ladderId)
        {
            this.ladderService.Queue(ladderId);

            return this.Ok();
        }

        /// <summary>
        /// Leave the queue for a ladder
        /// </summary>
        /// <param name="ladderId">Ladder Id</param>
        [HttpDelete("{ladderId:guid}/queue")]
        public IActionResult DeleteJoin(Guid ladderId)
        {
            this.ladderService.LeaveQueue(ladderId);

            return this.Ok();
        }

        /// <summary>
        /// Gets ladder standings
        /// </summary>
        /// <param name="ladderId">Id of ladder</param>
        /// <param name="start">Items to skip before returning</param>
        /// <param name="count">Count of standings to return</param>
        /// <returns></returns>
        [HttpGet("{ladderId:guid}/standings")]
        [ProducesResponseType(typeof(IEnumerable<LadderStanding>), 200)]
        public IEnumerable<LadderStanding> GetStandings(Guid ladderId, int start = 0, int count = 30)
        {
            return this.ladderService.GetStandings(ladderId, start, count);
        }
    }
}
