using System.Linq;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using ImperaPlus.Application.Users;
using ImperaPlus.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    public class UsersController : BaseAdminController
    {
        private readonly IUserService userService;

        public UsersController(IUnitOfWork unitOfWork, IUserService userService)
            : base(unitOfWork)
        {
            this.userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(string userId)
        {
            var user = unitOfWork.Users.FindById(userId);
            if (user != null)
            {
                userService.DeleteAccount(user);

                return Ok();
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Confirm(string userId)
        {
            var user = unitOfWork.Users.FindById(userId);
            if (user != null)
            {
                userService.ConfirmEmail(user);

                return Ok();
            }

            return NotFound();
        }

        [HttpPost]
        public DataTablesJsonResult Data(IDataTablesRequest request)
        {
            var data = unitOfWork.Users.Query();

            if (request.Search != null && !string.IsNullOrWhiteSpace(request.Search.Value))
            {
                data = data.Where(x =>
                    x.UserName.Contains(request.Search.Value) || x.Email.Contains(request.Search.Value));
            }

            var dataPage = data
                .OrderBy(x => x.UserName)
                .Skip(request.Start)
                .Take(request.Length)
                .Select(u => new
                {
                    u.Id,
                    Name = u.UserName,
                    u.Email,
                    u.EmailConfirmed,
                    u.IsDeleted
                });

            var response = DataTablesResponse.Create(request, data.Count(), data.Count(), dataPage);

            return new DataTablesJsonResult(response, true);
        }
    }
}
