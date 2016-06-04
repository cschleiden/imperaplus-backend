using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity.Owin;
using ImperaPlus.Backend.Identity;
using Microsoft.Owin;

namespace ImperaPlus.Backend.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string publicClientId;
        private readonly Func<IOwinContext, ApplicationUserManager> userManagerFactory;

        public ApplicationOAuthProvider(
            Func<IOwinContext, ApplicationUserManager> userManager,
            string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            this.publicClientId = publicClientId;
            this.userManagerFactory = userManager;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (var userManager = this.userManagerFactory(context.OwinContext))
            using (var signinManager = new ApplicationSignInManager(userManager, context.OwinContext.Authentication))
            {
                signinManager.AuthenticationType = DefaultAuthenticationTypes.ExternalBearer;

                var result = await signinManager.PasswordSignInAsync(context.UserName, context.Password, false, false);
                switch (result)
                {
                    case SignInStatus.Success:
                        // Fall through... 
                        break;

                    case SignInStatus.LockedOut:
                        context.SetError(Application.ErrorCode.AccountIsLocked.ToString(), "Account is locked");
                        return;

                    case SignInStatus.RequiresVerification:
                        context.SetError(Application.ErrorCode.AccountNotConfirmed.ToString(), "Account not confirmed");
                        return;

                    case SignInStatus.Failure:
                    default:
                        context.SetError(Application.ErrorCode.UsernameOrPasswordNotCorrect.ToString(), "The user name or password is incorrect.");
                        return;
                }

                var user = await userManager.FindByNameAsync(context.UserName);

                ClaimsIdentity oAuthIdentity = await userManager.CreateIdentityAsync(user,
                    context.Options.AuthenticationType);
                ClaimsIdentity cookiesIdentity = await userManager.CreateIdentityAsync(user,
                    CookieAuthenticationDefaults.AuthenticationType);
                AuthenticationProperties properties = await CreateProperties(userManager, user);
                var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            // if (context.ClientId == this.publicClientId)
            // {
            //     Uri expectedRootUri = new Uri(context.Request.Uri, "/");
            // 
            //     if (expectedRootUri.AbsoluteUri == context.RedirectUri)
            //     {
            context.Validated();
            //     }
            // }

            return Task.FromResult<object>(null);
        }

        public static async Task<AuthenticationProperties> CreateProperties(ApplicationUserManager userManager, User user)
        {
            var userRoles = await userManager.GetRolesAsync(user.Id);

            IDictionary<string, string> data = new Dictionary<string, string>
            {
                {
                    "userId", user.Id
                },
                {
                    "userName", user.UserName
                },
                {
                    "roles", string.Join(",", userRoles.ToArray())
                },
                {
                    "language", user.Language
                }
            };

            return new AuthenticationProperties(data);
        }
    }
}