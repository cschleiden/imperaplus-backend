using System.Linq;
using System.Threading.Tasks;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using ImperaPlus.Application.Users;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Areas.Admin.Controllers
{
    public class UsersController : BaseAdminController
    {
        private readonly IUserService userService;
        private readonly UserManager<User> userManager;

        public UsersController(IUnitOfWork unitOfWork, IUserService userService, UserManager<User> userManager)
            : base(unitOfWork)
        {
            this.userService = userService;
            this.userManager = userManager;
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

        [HttpGet]
        public ActionResult GetRoles()
        {
            var availableRoles = unitOfWork.Roles.Query()
                .Select(r => new { r.Id, r.Name })
                .ToList();
            return Json(availableRoles);
        }

        [HttpPost]
        public async Task<ActionResult> AddRole(string userId, string roleName)
        {
            var user = unitOfWork.Users.FindById(userId);
            if (user == null)
            {
                return NotFound();
            }

            var identityResult = await userManager.AddToRoleAsync(user, roleName);
            if (identityResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest(identityResult.Errors);
        }

        [HttpPost]
        public async Task<ActionResult> RemoveRole(string userId, string roleName)
        {
            var user = unitOfWork.Users.FindById(userId);
            if (user == null)
            {
                return NotFound();
            }

            var identityResult = await userManager.RemoveFromRoleAsync(user, roleName);
            if (identityResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest(identityResult.Errors);
        }

        [HttpPost]
        public DataTablesJsonResult Data(IDataTablesRequest request)
        {
            var roleFilter = HttpContext.Request.Form["roleFilter"].FirstOrDefault() ?? string.Empty;
            var rolesLookup = unitOfWork.Roles.Query().ToList();
            var data = unitOfWork.Users.Query();

            if (request.Search != null && !string.IsNullOrWhiteSpace(request.Search.Value))
            {
                data = data.Where(x =>
                    x.UserName.Contains(request.Search.Value) || x.Email.Contains(request.Search.Value));
            }

            if (!string.IsNullOrWhiteSpace(roleFilter))
            {
                var targetRole = rolesLookup.FirstOrDefault(r => r.Name == roleFilter);
                if (targetRole != null)
                {
                    data = data.Where(x => x.Roles.Any(r => r.RoleId == targetRole.Id));
                }
            }

            var filteredTotal = data.Count();

            var pageItems = data
                .OrderBy(x => x.UserName)
                .Skip(request.Start)
                .Take(request.Length)
                .Select(u => new
                {
                    u.Id,
                    Name = u.UserName,
                    u.Email,
                    u.EmailConfirmed,
                    u.IsDeleted,
                    RoleIds = u.Roles.Select(r => r.RoleId).ToList()
                })
                .ToList();

            // Map role IDs to role names in memory using dictionary for O(1) lookups
            var rolesDict = rolesLookup.ToDictionary(r => r.Id, r => r.Name);
            var resultItems = pageItems.Select(u => new
            {
                u.Id,
                u.Name,
                u.Email,
                u.EmailConfirmed,
                u.IsDeleted,
                Roles = u.RoleIds
                    .Where(rid => rolesDict.ContainsKey(rid))
                    .Select(rid => rolesDict[rid])
                    .ToList()
            }).ToList();

            var response = DataTablesResponse.Create(request, filteredTotal, filteredTotal, resultItems);

            return new DataTablesJsonResult(response, true);
        }
    }
}
