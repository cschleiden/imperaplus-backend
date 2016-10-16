using System.Collections.Generic;
using ImperaPlus.Application.Users;
using ImperaPlus.Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImperaPlus.Backend.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UserController : BaseController
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Find users starting with the given query
        /// </summary>
        /// <param name="query">Query to search for</param>
        [HttpGet("find/{query:minlength(3):maxlength(50)}")]
        [Produces(typeof(IEnumerable<DTO.Users.UserReference>))]
        public IActionResult FindUsers(string query)
        {
            Require.NotNullOrEmpty(query, nameof(query));

            return this.Ok(this.userService.FindUsers(query));
        }
    }
}
