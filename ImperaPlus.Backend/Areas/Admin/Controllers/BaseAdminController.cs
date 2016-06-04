using System;
using System.Linq;
using System.Web.Mvc;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class BaseAdminController : Controller
    {
        protected IUnitOfWork unitOfWork;

        public BaseAdminController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        protected void AddLookups()
        {
            this.ViewBag.MapTemplates = this.unitOfWork.MapTemplates.Query().Select(x => x.Name).ToList();
            this.ViewBag.VictoryConditionValues = Enum.GetNames(typeof(Domain.Enums.VictoryConditionType));
            this.ViewBag.VisibilityModifierValues = Enum.GetNames(typeof(Domain.Enums.VisibilityModifierType));
        }
    }
}