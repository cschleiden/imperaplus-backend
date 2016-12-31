using System.Collections.Generic;
using AspNet.Security.OAuth.Validation;
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
        [ProducesResponseType(typeof(IEnumerable<DTO.Users.UserReference>), 200)]
        public IActionResult FindUsers(string query)
        {
            Require.NotNullOrEmpty(query, nameof(query));

            return this.Ok(this.userService.FindUsers(query));
        }
    }
}
