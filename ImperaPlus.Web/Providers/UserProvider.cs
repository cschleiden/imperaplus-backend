using System.Linq;
using AspNet.Security.OpenIdConnect.Primitives;
using ImperaPlus.Domain;
using Microsoft.AspNetCore.Http;

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
            return this.httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == OpenIdConnectConstants.Claims.Subject).Value;
        }

        public virtual bool IsAdmin()
        {
            var role = this.httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == OpenIdConnectConstants.Claims.Role).Value;
            return !string.IsNullOrEmpty(role) && role.Contains("admin");
        }
    }
}