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
    [Route("ladder")]
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
            return ladderService.GetAll();
        }

        /// <summary>
        /// Gets ladder identified by given id
        /// </summary>
        /// <param name="ladderId">Id of ladder</param>        
        [HttpGet("{ladderId:guid}")]
        [ProducesResponseType(typeof(Ladder), 200)]
        public IActionResult Get(Guid ladderId)
        {
            return Ok(ladderService.Get(ladderId));
        }

        /// <summary>
        /// Queue up for a new game in the given ladder
        /// </summary>
        /// <param name="ladderId">Ladder id</param>
        [HttpPost("{ladderId:guid}/queue")]
        public IActionResult PostJoin(Guid ladderId)
        {
            ladderService.Queue(ladderId);

            return Ok();
        }

        /// <summary>
        /// Leave the queue for a ladder
        /// </summary>
        /// <param name="ladderId">Ladder Id</param>
        [HttpDelete("{ladderId:guid}/queue")]
        public IActionResult DeleteJoin(Guid ladderId)
        {
            ladderService.LeaveQueue(ladderId);

            return Ok();
        }
    }
}
