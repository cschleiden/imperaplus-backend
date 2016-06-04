using System.Web;
using System.Threading;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain;
using Microsoft.AspNet.Identity;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.Backend.Providers
{
    /// <summary>
    /// Provides the currently authenticated user
    /// </summary>
    public class UserProvider : IUserProvider
    {
        public string GetCurrentUserId()
        {
            return Thread.CurrentPrincipal.Identity.GetUserId();
        }
    }
}