using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using ImperaPlus.Application;
using ImperaPlus.Application.Exceptions;
using ImperaPlus.DTO.Games.Map;

namespace ImperaPlus.Backend.Controllers
{
    [RoutePrefix("api/map")]
    public class MapController : ApiController
    {
        private readonly IMapTemplateService mapTemplateService;

        public MapController(IMapTemplateService mapTemplateService)
        {
            this.mapTemplateService = mapTemplateService;
        }

        [Route("")]
        public IEnumerable<MapTemplateSummary> GetAllSummary()
        {
            return this.mapTemplateService.QuerySummary();
        }

        /// <summary>
        /// Get map template identified by name
        /// </summary>
        [Route("{name:minlength(1)}")]
        [ResponseType(typeof(MapTemplate))]
        public IHttpActionResult GetMapTemplate(string name)
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
