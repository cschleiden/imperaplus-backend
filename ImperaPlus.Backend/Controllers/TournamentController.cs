using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using ImperaPlus.Application.Tournaments;

namespace ImperaPlus.Backend.Controllers
{
    [Authorize]
    [RoutePrefix("api/tournaments")]
    public class TournamentController : ApiController
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
        [Route("")]
        [ResponseType(typeof(IEnumerable<DTO.Tournaments.Tournament>))]
        public IHttpActionResult GetAll()
        {
            return this.Ok(this.tournamentService.GetAll());
        }
        
        /// <summary>
        /// Get tournament identified by Id
        /// </summary>
        /// <param name="tournamentId">Id of tournament</param>
        [Route("{tournamentId:guid}")]
        [ResponseType(typeof(DTO.Tournaments.Tournament))]
        public IHttpActionResult GetById(Guid tournamentId)
        {
            return this.Ok(this.tournamentService.Get(tournamentId));
        }

        /// <summary>
        /// Join tournament
        /// </summary>
        /// <param name="tournamentId">Id of tournament</param>
        [Route("{tournamentId:guid}")]
        [ResponseType(typeof(DTO.Tournaments.TournamentTeam))]
        public IHttpActionResult PostJoin(Guid tournamentId)
        {
            return this.Ok(this.tournamentService.Join(tournamentId));
        }

        /// <summary>
        /// Get teams for tournament
        /// </summary>
        /// <param name="tournamentId">Id of tournament</param>
        [Route("{tournamentId:guid}/teams")]
        [ResponseType(typeof(IEnumerable<DTO.Tournaments.TournamentTeam>))]
        public IHttpActionResult GetTeams(Guid tournamentId)
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
        [Route("{tournamentId:guid}/teams")]
        [ResponseType(typeof(DTO.Tournaments.TournamentTeamSummary))]
        public IHttpActionResult PostCreateTeam(Guid tournamentId, string name, string password = null)
        {
            return this.Ok(this.tournamentService.CreateTeam(tournamentId, name, password));
        }

        /// <summary>
        /// Join existing team
        /// </summary>
        /// <param name="tournamentId">Id of tournament</param>
        /// <param name="teamId">Id of team</param>
        /// <param name="password">Optional password for team to join</param>
        [Route("{tournamentId:guid}/teams/{teamId:guid}")]
        public IHttpActionResult PostJoinTeam(Guid tournamentId, Guid teamId, string password = null)
        {
            this.tournamentService.JoinTeam(tournamentId, teamId, password);

            return this.Ok();
        }

        /// <summary>
        /// Delete a team. Only allowed if user created it
        /// </summary>
        /// <param name="tournamentId">Id of tournament</param>
        /// <param name="teamId">Id of team to delete</param>
        [Route("{tournamentId:guid}/teams/{teamId:guid}")]
        public IHttpActionResult DeleteTeam(Guid tournamentId, Guid teamId)
        {
            this.tournamentService.DeleteTeam(tournamentId, teamId);

            return this.Ok();
        }

        /// <summary>
        /// Leave a team and tournament
        /// </summary>
        /// <param name="tournamentId">Id of tournament</param>
        [Route("{tournamentId:guid}/teams/me")]
        public IHttpActionResult LeaveTournament(Guid tournamentId)
        {
            this.tournamentService.Leave(tournamentId);

            return this.Ok();
        }
    }
}
