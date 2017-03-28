using ImperaPlus.Application.News;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Utilities;
using ImperaPlus.DTO.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class NewsController : BaseAdminController
    {
        private INewsService newsService;

        public NewsController(IUnitOfWork unitOfWork, INewsService newsService)
            : base(unitOfWork)
        {
            this.newsService = newsService;
        }
        
        public ActionResult Index()
        {
            var news = this.unitOfWork.News.GetOrdered(10);

            return View(news);
        }
        
        [HttpPost]
        public ActionResult PostCreate(NewsContent[] post)
        {
            Require.NotNull(post, nameof(post));

            this.newsService.PostNews(post);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(long id)
        {
            this.newsService.Delete(id);

            return this.RedirectToAction("Index");
        }
    }
}