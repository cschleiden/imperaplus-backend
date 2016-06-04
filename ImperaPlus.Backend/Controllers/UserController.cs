using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using ImperaPlus.Application.Users;
using ImperaPlus.Domain.Utilities;

namespace ImperaPlus.Backend.Controllers
{
    [Authorize]
    [RoutePrefix("api/users")]
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
        [Route("find/{query:minlength(3):maxlength(50)}")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<DTO.Users.UserReference>))]
        public IHttpActionResult FindUsers(string query)
        {
            Require.NotNullOrEmpty(query, nameof(query));

            return this.Ok(this.userService.FindUsers(query));
        }
    }
}
