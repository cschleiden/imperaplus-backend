using System;
using System.Collections.Generic;
using ImperaPlus.Application.Alliances;
using ImperaPlus.DTO;
using ImperaPlus.DTO.Alliances;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Controllers
{
    /// <summary>
    /// General management of games
    /// </summary>
    [Authorize]
    [Route("api/alliances")]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(void), 200)]
    public class AllianceController : BaseController
    {
        private IAllianceService allianceService;

        public AllianceController(IAllianceService allianceService)
        {
            this.allianceService = allianceService;
        }

        /// <summary>
        /// Get a list of all alliances
        /// </summary>
        /// <returns>Alliance summaries</returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<AllianceSummary>), 200)]
        public IActionResult GetAll()
        {
            var alliances = this.allianceService.GetAll();

            return this.Ok(alliances);
        }


        /// <summary>
        /// Get detailed information about a single alliance
        /// </summary>
        /// <param name="allianceId">Id of the requested alliance</param>
        /// <returns>Information about the requested alliance</returns>
        [HttpGet("{allianceId:long:min(1)}")]
        [ProducesResponseType(typeof(Alliance), 200)]
        public IActionResult Get(Guid allianceId)
        {
            var alliance = this.allianceService.Get(allianceId);

            return this.Ok(alliance);
        }

        /// <summary>
        /// Create a new alliance
        /// </summary>
        /// <param name="creationOptions">Creation options</param>
        /// <returns>Summary of new alliance</returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(AllianceSummary), 200)]
        public IActionResult Post([FromBody] AllianceCreationOptions creationOptions)
        {
            var alliance = this.allianceService.Create(creationOptions);

            return this.Ok(alliance);
        }
    }
}
