using System.Collections.Generic;
using System.Web.Http;
using ImperaPlus.Application.News;
using ImperaPlus.DTO.News;
using System.Web.Http.Description;

namespace ImperaPlus.Backend.Controllers
{
    [RoutePrefix("api/news")]
    [Authorize]
    public class NewsController : ApiController
    {
        private readonly INewsService newsService;

        public NewsController(INewsService newsService)
        {
            this.newsService = newsService;
        }

        /// <summary>
        /// Returns the last 10 news items for all languages
        /// </summary>
        /// <returns>List of news items</returns>
        [Route("")]
        public IEnumerable<NewsItem> GetAll()
        {
            return this.newsService.GetNews();
        }        
    }
}