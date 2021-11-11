using ImperaPlus.Application.News;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Utilities;
using ImperaPlus.DTO.News;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    public class NewsController : BaseAdminController
    {
        private readonly INewsService newsService;

        public NewsController(IUnitOfWork unitOfWork, INewsService newsService)
            : base(unitOfWork)
        {
            this.newsService = newsService;
        }

        public ActionResult Index()
        {
            var news = unitOfWork.News.GetOrdered(10);

            return View(news);
        }

        [HttpPost]
        public ActionResult PostCreate(NewsContent[] post)
        {
            Require.NotNull(post, nameof(post));

            newsService.PostNews(post);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(long id)
        {
            newsService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
