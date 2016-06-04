using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using ImperaPlus.Application.Games;
using ImperaPlus.Application.Play;
using ImperaPlus.DTO.Games.Play;

namespace ImperaPlus.Backend.Controllers
{
    /// <summary>
    /// Provides actions to play the game. 
    /// </summary>
    [Authorize]
    [RoutePrefix("api/games/{gameId:long:min(1)}/play")]
    public class PlayController : BaseController
    {
        private readonly IPlayService playService;
        private IGameService gameService;

        public PlayController(IGameService gameService, IPlayService playService) 
        {
            this.gameService = gameService;
            this.playService = playService;
        }

        /// <summary>
        /// Place units to countries.
        /// </summary>
        /// <param name="gameId">Id of the game</param>
        /// <param name="placeUnitsOptions">List of country/unit count pairs</param>
        /// <returns>GameActionResult of action</returns>
        [Route("place")]
        [ResponseType(typeof(DTO.Games.GameActionResult))]
        public IHttpActionResult PostPlace(long gameId, IEnumerable<PlaceUnitsOptions> placeUnitsOptions)
        {
            var gameActionResult = this.playService.Place(gameId, placeUnitsOptions);

            return this.Ok(gameActionResult);
        }

        /// <summary>
        /// Exchange cards for the current player. Which cards to exchange is automatically chosen to gain the most bonus for the player.
        /// </summary>
        /// <param name="gameId">Id of the game</param>
        /// <returns>GameActionResult of action</returns>
        [Route("exchange")]
        [ResponseType(typeof(DTO.Games.GameActionResult))]
        public IHttpActionResult PostExchange(long gameId)
        {
            var gameResult = this.playService.Exchange(gameId);

            return this.Ok(gameResult);
        }

        /// <summary>
        /// Attack from one to another country.
        /// </summary>
        /// <param name="gameId">Id of the game</param>
        /// <param name="options">Options for the command</param>
        /// <returns>GameActionResult of action</returns>
        [Route("attack")]
        [ResponseType(typeof(DTO.Games.GameActionResult))]
        public IHttpActionResult PostAttack(long gameId, AttackOptions options)
        {
            var gameActionResult = this.playService.Attack(gameId, options.OriginCountryIdentifier, options.DestinationCountryIdentifier, options.NumberOfUnits);

            return this.Ok(gameActionResult);
        }

        /// <summary>
        /// Switch to moving.
        /// </summary>
        /// <param name="gameId">Id of the game</param>
        /// <returns>GameActionResult of action</returns>
        [Route("endattack")]
        [ResponseType(typeof(DTO.Games.GameActionResult))]
        public IHttpActionResult PostEndAttack(long gameId)
        {
            var gameActionResult = this.playService.EndAttack(gameId);

            return this.Ok(gameActionResult);
        }

        /// <summary>
        /// Move units between countries. Only allowed after placing. Cancels any attacks that the player had left before. Attacking is not
        /// possible anymore after moving.
        /// </summary>
        /// <param name="gameId">Id of the game</param>
        /// <param name="options">Options for the command</param>
        /// <returns>GameActionResult of action</returns>        
        [Route("move")]
        [ResponseType(typeof(DTO.Games.GameActionResult))]
        public IHttpActionResult PostMove(long gameId, MoveOptions options)
        {
            var gameActionResult = this.playService.Move(gameId, options.OriginCountryIdentifier, options.DestinationCountryIdentifier, options.NumberOfUnits);

            return this.Ok(gameActionResult);
        }

        /// <summary>
        /// End the current turn
        /// </summary>
        /// <param name="gameId">Id of the game</param>
        /// <returns>GameActionResult of action</returns>
        [Route("endturn")]
        [ResponseType(typeof(DTO.Games.Game))]
        public IHttpActionResult PostEndTurn(long gameId)
        {
            var gameResult = this.playService.EndTurn(gameId);

            return this.Ok(gameResult);
        }
    }
}
