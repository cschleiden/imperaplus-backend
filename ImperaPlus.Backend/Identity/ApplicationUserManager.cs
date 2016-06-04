using ImperaPlus.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Threading.Tasks;
using System.Web.Security;

namespace ImperaPlus.Backend.Identity
{
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store, IIdentityMessageService emailMessageService)
            : base(store)
        {
            this.UserValidator = new UserValidator<User>(this)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true,
            };

            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8
            };

            this.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<User>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });
            this.EmailService = emailMessageService;

            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 3;

            var provider = Authentication.DataProtectionProvider;
            if (provider != null)
            {
                this.UserTokenProvider = new DataProtectorTokenProvider<User, string>(provider.Create("PR"))
                {
                    TokenLifespan = TimeSpan.FromHours(12)
                };
            }
        }

        public async Task SetLanguageAsync(string userId, string language)
        {
            var user = await this.FindByIdAsync(userId);

            user.Language = language;

            await this.Store.UpdateAsync(user);
        }
    }
}