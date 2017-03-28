using System;
using System.Linq;
using ImperaPlus.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    [Area("admin")]
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
            this.ViewBag.MapTemplates = this.unitOfWork.MapTemplateDescriptors.Query().Select(x => x.Name).ToList();
            this.ViewBag.VictoryConditionValues = Enum.GetNames(typeof(Domain.Enums.VictoryConditionType));
            this.ViewBag.VisibilityModifierValues = Enum.GetNames(typeof(Domain.Enums.VisibilityModifierType));
        }
    }
}