using System.Collections.Generic;
using ImperaPlus.Application;
using ImperaPlus.Application.Exceptions;
using ImperaPlus.DTO.Games.Map;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Controllers
{
    [Route("api/map")]
    public class MapController : Controller
    {
        private readonly IMapTemplateService mapTemplateService;

        public MapController(IMapTemplateService mapTemplateService)
        {
            this.mapTemplateService = mapTemplateService;
        }

        [HttpGet("")]
        public IEnumerable<MapTemplateDescriptor> GetAllSummary()
        {
            return this.mapTemplateService.QuerySummary();
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
                return this.Ok(this.mapTemplateService.Get(name));
            }
            catch (ApplicationException exception)
            {
                if (exception.ErrorCode == ErrorCode.CannotFindMapTemplate)
                {
                    return this.NotFound();
                }
                
                throw;
            }
        }
    }
}
