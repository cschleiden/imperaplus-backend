using ImperaPlus.Application.Ladder;
using ImperaPlus.DTO.Ladder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ImperaPlus.Backend.Controllers
{
    /// <summary>
    /// Ladder interaction
    /// </summary>
    [Authorize]
    [RoutePrefix("api/ladder")]
    public class LadderController : ApiController
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
        [Route("")]
        public IEnumerable<LadderSummary> Get()
        {
            return this.ladderService.GetAll();
        }

        /// <summary>
        /// Gets ladder identified by given id
        /// </summary>
        /// <param name="ladderId">Id of ladder</param>        
        [Route("{ladderId:guid}")]
        [ResponseType(typeof(DTO.Ladder.Ladder))]
        public IHttpActionResult Get(Guid ladderId)
        {
            return this.Ok(this.ladderService.Get(ladderId));
        }

        /// <summary>
        /// Queue up for a new game in the given ladder
        /// </summary>
        /// <param name="ladderId">Ladder id</param>
        /// <returns>Status </returns>
        [Route("{ladderId:guid}/queue")]
        public IHttpActionResult PostJoin(Guid ladderId)
        {
            this.ladderService.Queue(ladderId);

            return this.Ok();
        }

        /// <summary>
        /// Gets ladder standings
        /// </summary>
        /// <param name="ladderId">Id of ladder</param>
        /// <param name="start">Items to skip before returning</param>
        /// <param name="count">Count of standings to return</param>
        /// <returns></returns>
        [Route("{ladderId:guid}/standings")]
        public IEnumerable<DTO.Ladder.LadderStanding> GetStandings(Guid ladderId, int start = 0, int count = 30)
        {
            return this.ladderService.GetStandings(ladderId, start, count);
        }
    }
}
