using System.Collections.Generic;
using ImperaPlus.Application.Users;
using ImperaPlus.Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ImperaPlus.DTO;
using ImperaPlus.Domain.Repositories;
using AutoMapper;

namespace ImperaPlus.Backend.Controllers
{
    [Authorize]
    [Route("users")]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(void), 200)]
    public class UserController : BaseController
    {
        private IUserService userService;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
            : base(unitOfWork, mapper)
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

            return Ok(userService.FindUsers(query));
        }
    }
}
