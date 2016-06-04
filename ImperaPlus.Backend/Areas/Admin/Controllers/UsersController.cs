using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using System.Web.Mvc;
using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private IUnitOfWork unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public DataTablesJsonResult Data(IDataTablesRequest request)
        {
            var data = this.unitOfWork.Users.Query();

            if (request.Search != null && !string.IsNullOrWhiteSpace(request.Search.Value))
            {
                data = data.Where(x => x.UserName.Contains(request.Search.Value));
            }

            var dataPage = data
                .OrderBy(x => x.UserName)
                .Skip(request.Start)
                .Take(request.Length)
                .Select(u => new
                {
                    Id = u.Id,
                    Name = u.UserName,
                    Email = u.Email
                });

            var response = DataTablesResponse.Create(request, data.Count(), data.Count(), dataPage);

            return new DataTablesJsonResult(response, JsonRequestBehavior.AllowGet);
        }
    }
}