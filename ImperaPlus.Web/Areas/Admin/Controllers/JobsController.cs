using ImperaPlus.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    public class JobsController : BaseAdminController
    {
        public JobsController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        // GET: Admin/Hangfire
        public ActionResult Index()
        {
            return View();
        }
    }
}
