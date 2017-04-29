using System.Linq;
using DataTables.AspNet.Core;
using ImperaPlus.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using DataTables.AspNet.AspNetCore;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    public class UsersController : BaseAdminController
    {
        public UsersController(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
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

            return new DataTablesJsonResult(response, true);
        }
    }
}