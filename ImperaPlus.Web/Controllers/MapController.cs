using System.Collections.Generic;
using ImperaPlus.Application;
using ImperaPlus.Application.Exceptions;
using ImperaPlus.DTO.Games.Map;
using Microsoft.AspNetCore.Mvc;
using ImperaPlus.DTO;

namespace ImperaPlus.Backend.Controllers
{
    [Route("map")]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(void), 200)]
    [ResponseCache(Duration = 2592000)]
    public class MapController : Controller
    {
        private readonly IMapTemplateService mapTemplateService;

        public MapController(IMapTemplateService mapTemplateService)
        {
            this.mapTemplateService = mapTemplateService;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<MapTemplateDescriptor>), 200)]
        public IEnumerable<MapTemplateDescriptor> GetAllSummary()
        {
            return mapTemplateService.QuerySummary();
        }

        /// <summary>
        /// Get map template identified by name
        /// </summary>
        [HttpGet("{name:minlength(1)}")]
        [ProducesResponseType(typeof(MapTemplate), 200)]
        public IActionResult GetMapTemplate(string name)
        {
            try
            {
                return Ok(mapTemplateService.Get(name));
            }
            catch (ApplicationException exception)
            {
                if (exception.ErrorCode == ErrorCode.CannotFindMapTemplate)
                {
                    return NotFound();
                }

                throw;
            }
        }
    }
}
