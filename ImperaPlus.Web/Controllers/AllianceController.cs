using System;
using System.Collections.Generic;
using AutoMapper;
using ImperaPlus.DTO;
using ImperaPlus.DTO.Alliances;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Controllers
{
    /// <summary>
    /// General management of alliances
    /// </summary>
    [Authorize]
    [Route("api/alliances")]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(void), 200)]
    public class AllianceController : BaseController
    {
        private Domain.Alliances.IAllianceService allianceService;

        public AllianceController(Domain.Repositories.IUnitOfWork unitOfWork, Domain.Alliances.IAllianceService allianceService)
            : base(unitOfWork)
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
            return this.Map<IEnumerable<AllianceSummary>>(this.allianceService.GetAll());
        }


        /// <summary>
        /// Get detailed information about a single alliance
        /// </summary>
        /// <param name="allianceId">Id of the requested alliance</param>
        /// <returns>Information about the requested alliance</returns>
        [HttpGet("{allianceId:guid}")]
        [ProducesResponseType(typeof(Alliance), 200)]
        public IActionResult Get(Guid allianceId)
        {
            return this.Map<Alliance>(this.allianceService.Get(allianceId));
        }

        /// <summary>
        /// Create a new alliance
        /// </summary>
        /// <param name="creationOptions">Creation options</param>
        /// <returns>Summary of new alliance</returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(AllianceSummary), 200)]
        public IActionResult Create([FromBody] AllianceCreationOptions creationOptions)
        {
            return this.CommitAndMap<Alliance>(this.allianceService.Create(creationOptions.Name, creationOptions.Description));
        }

        /// <summary>
        /// Remove member from alliance
        /// </summary>
        /// <param name="allianceId">Id of alliance</param>
        /// <param name="userId">Id of user to remove</param>
        /// <returns>Summary of new alliance</returns>
        [HttpDelete("{allianceId:guid}/members/{userId}")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult RemoveMember(Guid allianceId, string userId)
        {
            this.allianceService.RemoveMember(allianceId, userId);
            return this.Commit();
        }

        /// <summary>
        /// Change member's admin status
        /// </summary>
        /// <param name="allianceId">Id of alliance</param>
        /// <param name="userId">Id of user to make admin</param>
        /// <param name="isAdmin"></param>
        /// <returns>Summary of new alliance</returns>
        [HttpPatch("{allianceId:guid}/members/{userId}")]
        [ProducesResponseType(typeof(void), 200)]
        public IActionResult ChangeAdmin(Guid allianceId, string userId, [FromBody] bool isAdmin)
        {
            this.allianceService.ChangeAdmin(allianceId, userId, isAdmin);
            return this.Commit();
        }

        /// <summary>
        /// Request to join an alliance
        /// </summary>
        /// <param name="allianceId">Id of the requested alliance</param>
        /// <param name="reason">Reason why user wants to join the alliance</param>
        /// <returns>Id of join request if created</returns>
        [HttpPost("{allianceId:guid}/requests")]
        [ProducesResponseType(typeof(AllianceJoinRequest), 200)]
        public IActionResult RequestJoin(Guid allianceId, [FromBody] string reason)
        {
            return this.CommitAndMap<AllianceJoinRequest>(this.allianceService.RequestToJoin(allianceId, reason));
        }

        /// <summary>
        /// Lists requests to join an alliance
        /// </summary>
        /// <param name="allianceId">Id of the alliance</param>
        /// <returns>List of active requests</returns>
        [HttpGet("{allianceId:guid}/requests")]
        [ProducesResponseType(typeof(IEnumerable<AllianceJoinRequest>), 200)]
        public IActionResult GetRequests(Guid allianceId)
        {
            return this.Map<IEnumerable<AllianceJoinRequest>>((this.allianceService.GetJoinRequests(allianceId)));
        }

        /// <summary>
        /// Updates a request to join an alliance. Requests can only be updated when they are in a pending state
        /// </summary>
        /// <param name="allianceId">Id of the requested alliance</param>
        /// <param name="requestId">Id of the request to change</param>
        /// <param name="state">New request state</param>
        [HttpPatch("{allianceId:guid}/requests/{requestId:guid}")]
        [ProducesResponseType(typeof(AllianceJoinRequest), 200)]
        public IActionResult ApproveRequest(Guid allianceId, Guid requestId, [FromBody] AllianceJoinRequestState state)
        {
            return this.CommitAndMap<AllianceJoinRequest>(
                this.allianceService.UpdateRequest(allianceId, requestId, Mapper.Map<Domain.Alliances.AllianceJoinRequestState>(state)));
        }
    }
}
