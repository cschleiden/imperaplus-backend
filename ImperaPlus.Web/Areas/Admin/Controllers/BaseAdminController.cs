using System;
using System.Linq;
using ImperaPlus.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BaseAdminController : Controller
    {
        protected IUnitOfWork unitOfWork;

        public BaseAdminController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        protected void AddLookups()
        {
            ViewBag.MapTemplates = unitOfWork.MapTemplateDescriptors.Query().Select(x => x.Name).ToList();
            ViewBag.VictoryConditionValues = Enum.GetNames(typeof(Domain.Enums.VictoryConditionType));
            ViewBag.VisibilityModifierValues = Enum.GetNames(typeof(Domain.Enums.VisibilityModifierType));
        }
    }
}
