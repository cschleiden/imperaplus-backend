using System;
using System.Collections.Generic;
using ImperaPlus.Application.Tournaments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ImperaPlus.DTO;
using ImperaPlus.Domain.Repositories;
using AutoMapper;

namespace ImperaPlus.Backend.Controllers
{
    [Authorize]
    [Route("tournaments")]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(void), 200)]
    public class TournamentController : Controller
    {
        private ITournamentService tournamentService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TournamentController(ITournamentService tournamentService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.tournamentService = tournamentService;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        /// <summary>
        /// Returns tournaments
        /// </summary>
        /// <returns>List of tournaments</returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Tournaments.TournamentSummary>), 200)]
        public IActionResult GetAll()
        {
            return this.Ok(
                this.mapper.Map<IEnumerable<DTO.Tournaments.Tournament>>(
                    this.unitOfWork.Tournaments.Get(true)
                )
            );
        }

        /// <summary>
        /// Get tournament identified by Id
        /// </summary>
        /// <param name="tournamentId">Id of tournament</param>
        [HttpGet("{tournamentId:guid}")]
        [ProducesResponseType(typeof(DTO.Tournaments.Tournament), 200)]
        public IActionResult GetById(Guid tournamentId)
        {
            return this.Ok(
                this.mapper.Map<DTO.Tournaments.Tournament>(
                    this.unitOfWork.Tournaments.GetById(tournamentId, readOnly: true)
                )
            );
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
        /// Get teams for tournament pairing
        /// </summary>
        /// <param name="pairingId">Id of tournament pairing</param>
        [HttpGet("pairings/{pairingId:guid}/")]
        [ProducesResponseType(typeof(IEnumerable<DTO.Games.GameSummary>), 200)]
        public IActionResult GetGamesForPairing(Guid pairingId)
        {
            return this.Ok(this.tournamentService.GetGamesForPairing(pairingId));
        }

        /// <summary>
        /// Create new team for a tournament
        /// </summary>
        /// <param name="tournamentId">Id of tournament</param>
        /// <param name="name">Name of team</param>
        /// /// <param name="password">Optional password for team</param>
        /// <returns>Summary of newly created team</returns>
        [HttpPost("{tournamentId:guid}/teams")]
        [ProducesResponseType(typeof(DTO.Tournaments.TournamentTeam), 200)]
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
        [ProducesResponseType(typeof(DTO.Tournaments.TournamentTeam), 200)]
        public IActionResult PostJoinTeam(Guid tournamentId, Guid teamId, string password = null)
        {
            return this.Ok(this.tournamentService.JoinTeam(tournamentId, teamId, password));
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
