using System.Web.Mvc;
using ImperaPlus.Application.News;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Utilities;
using ImperaPlus.DTO.News;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class NewsController : Controller
    {
        private INewsService newsService;
        private IUnitOfWork unitOfWork;

        public NewsController(IUnitOfWork unitOfWork, INewsService newsService)
        {
            this.unitOfWork = unitOfWork;
            this.newsService = newsService;
        }

        // GET: Admin/News
        public ActionResult Index()
        {
            var news = this.unitOfWork.News.GetOrdered(10);

            return View(news);
        }
        
        [ValidateInput(false)]
        public ActionResult PostCreate(NewsContent[] post)
        {
            Require.NotNull(post, nameof(post));

            this.newsService.PostNews(post);

            return this.RedirectToAction("Index");
        }

        public ActionResult Delete(long id)
        {
            this.newsService.Delete(id);

            return this.RedirectToAction("Index");
        }
    }
}