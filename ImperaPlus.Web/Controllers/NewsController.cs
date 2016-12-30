using System.Collections.Generic;
using AspNet.Security.OAuth.Validation;
using ImperaPlus.Application.News;
using ImperaPlus.DTO.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Controllers
{
    [Route("api/news")]
    [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
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
        public IEnumerable<NewsItem> GetAll()
        {
            return this.newsService.GetNews();
        }        
    }
}