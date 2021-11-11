using System.Linq;
using ImperaPlus.Domain;
using Microsoft.AspNetCore.Http;
using OpenIddict.Abstractions;

namespace ImperaPlus.Web.Providers
{
    /// <summary>
    /// Provides the currently authenticated user
    /// </summary>
    public class UserProvider : IUserProvider
    {
        private IHttpContextAccessor httpContextAccessor;

        public UserProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            return httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == OpenIddictConstants.Claims.Subject)
                .Value;
        }

        public virtual bool IsAdmin()
        {
            var role = httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == OpenIddictConstants.Claims.Role)
                .Value;
            return !string.IsNullOrEmpty(role) && role.Contains("admin");
        }
    }
}
