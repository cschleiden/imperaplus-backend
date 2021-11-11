using ImperaPlus.Application.News;
using ImperaPlus.DTO;
using ImperaPlus.DTO.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ImperaPlus.Backend.Controllers
{
    [Route("news")]
    [Authorize]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(void), 200)]
    public class NewsController : Controller
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
        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<NewsItem>), 200)]
        public IEnumerable<NewsItem> GetAll()
        {
            return newsService.GetNews();
        }
    }
}
