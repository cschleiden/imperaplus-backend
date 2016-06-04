using ImperaPlus.Domain;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace ImperaPlus.Backend.Identity
{
    public class ApplicationSignInManager : SignInManager<User, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            var user = await this.UserManager.FindByNameAsync(userName);

            if (user == null)
            {
                return SignInStatus.Failure;
            }

            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                return SignInStatus.LockedOut;
            }

            if (!await UserManager.CheckPasswordAsync(user, password))
            {
                await UserManager.AccessFailedAsync(user.Id);
                if (await UserManager.IsLockedOutAsync(user.Id))
                {
                    return SignInStatus.LockedOut;
                }

                // Check for legacy password, if set
                if (string.IsNullOrEmpty(user.LegacyPasswordHash) || 
                    !this.LegacyPasswordMatch(password, user.LegacyPasswordHash))
                {
                    return SignInStatus.Failure;
                }
            }


            // TODO: CS: Move to configuration
            if (Startup.RequireUserConfirmation)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    return SignInStatus.RequiresVerification;
                }
            }

            await base.SignInAsync(user, isPersistent, false);

            return SignInStatus.Success;
        }

        private bool LegacyPasswordMatch(string password, string legacyPasswordHash)
        {
            string salt = legacyPasswordHash.Substring(0, Math.Min(9, legacyPasswordHash.Length));

            using (var sha1Provider = SHA1.Create())
            {
                return string.Equals(
                    salt + string.Join("", 
                        sha1Provider.ComputeHash(Encoding.UTF8.GetBytes(salt + password)).Select(x => x.ToString("x2"))), 
                    legacyPasswordHash);
            }
        }
    }
}