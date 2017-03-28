using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class JobsController : Controller
    {
        // GET: Admin/Hangfire
        public ActionResult Index()
        {
            return View();
        }
    }
}