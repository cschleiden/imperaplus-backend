using ImperaPlus.DataAccess;
using ImperaPlus.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ImperaPlus.Web.Providers
{
    /// <summary>
    /// Provides the currently authenticated user
    /// </summary>
    public class UserProvider : IUserProvider
    {
        private IHttpContextAccessor httpContextAccessor;
        private UserManager<User> userManager;

        public UserProvider(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public string GetCurrentUserId()
        {
            return this.userManager.GetUserId(this.httpContextAccessor.HttpContext.User);
        }
    }
}