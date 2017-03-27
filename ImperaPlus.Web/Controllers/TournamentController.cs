using System;
using System.Collections.Generic;
using ImperaPlus.Application.Tournaments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ImperaPlus.DTO;

namespace ImperaPlus.Backend.Controllers
{
    [Authorize]
    [Route("api/tournaments")]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(void), 200)]
    public class TournamentController : Controller
    {
        private ITournamentService tournamentService;

        public TournamentController(ITournamentService tournamentService)
        {
            this.tournamentService = tournamentService;
        }

        /// <summary>
        /// Returns tournaments
        /// </summary>
        /// <returns>List of tournaments</returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Tournaments.Tournament>), 200)]
        public IActionResult GetAll()
        {
            return this.Ok(this.tournamentService.GetAll());
        }
        
        /// <summary>
        /// Get tournament identified by Id
        /// </summary>
        /// <param name="tournamentId">Id of tournament</param>
        [HttpGet("{tournamentId:guid}")]
        [ProducesResponseType(typeof(DTO.Tournaments.Tournament), 200)]
        public IActionResult GetById(Guid tournamentId)
        {
            return this.Ok(this.tournamentService.Get(tournamentId));
        }

        /// <summary>
        /// Join tournament
        /// </summary>
        /// <param name="tournamentId">Id of tournament</param>
        [HttpPost("{tournamentId:guid}")]
        [ProducesResponseType(typeof(DTO.Tournaments.TournamentTeam), 200)]
        public IActionResult PostJoin(Guid tournamentId)
        {
            return this.Ok(this.tournamentService.Join(tournamentId));
        }

        /// <summary>
        /// Get teams for tournament
        /// </summary>
        /// <param name="tournamentId">Id of tournament</param>
        [HttpGet("{tournamentId:guid}/teams")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Tournaments.TournamentTeam>), 200)]
        public IActionResult GetTeams(Guid tournamentId)
        {
            return this.Ok(this.tournamentService.GetTeams(tournamentId));
        }

        /// <summary>
        /// Create new team for a tournament
        /// </summary>
        /// <param name="tournamentId">Id of tournament</param>
        /// <param name="name">Name of team</param>
        /// /// <param name="password">Optional password for team</param>
        /// <returns>Summary of newly created team</returns>
        [HttpPost("{tournamentId:guid}/teams")]
        [ProducesResponseType(typeof(DTO.Tournaments.TournamentTeamSummary), 200)]
        public IActionResult PostCreateTeam(Guid tournamentId, string name, string password = null)
        {
            return this.Ok(this.tournamentService.CreateTeam(tournamentId, name, password));
        }

        /// <summary>
        /// Join existing team
        /// </summary>
        /// <param name="tournamentId">Id of tournament</param>
        /// <param name="teamId">Id of team</param>
        /// <param name="password">Optional password for team to join</param>
        [HttpPost("{tournamentId:guid}/teams/{teamId:guid}")]
        public IActionResult PostJoinTeam(Guid tournamentId, Guid teamId, string password = null)
        {
            this.tournamentService.JoinTeam(tournamentId, teamId, password);

            return this.Ok();
        }

        /// <summary>
        /// Delete a team. Only allowed if user created it
        /// </summary>
        /// <param name="tournamentId">Id of tournament</param>
        /// <param name="teamId">Id of team to delete</param>
        [HttpDelete("{tournamentId:guid}/teams/{teamId:guid}")]
        public IActionResult DeleteTeam(Guid tournamentId, Guid teamId)
        {
            this.tournamentService.DeleteTeam(tournamentId, teamId);

            return this.Ok();
        }

        /// <summary>
        /// Leave a team and tournament
        /// </summary>
        /// <param name="tournamentId">Id of tournament</param>
        [HttpDelete("{tournamentId:guid}/teams/me")]
        public IActionResult LeaveTournament(Guid tournamentId)
        {
            this.tournamentService.Leave(tournamentId);

            return this.Ok();
        }
    }
}
