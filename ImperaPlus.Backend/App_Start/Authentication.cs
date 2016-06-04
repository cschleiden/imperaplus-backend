using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using ImperaPlus.Backend.Providers;
using Microsoft.Owin.Security.DataProtection;
using System.Web;
using System.Linq;
using ImperaPlus.Domain;
using ImperaPlus.Backend.Identity;
using Autofac.Integration.Owin;
using Autofac;
using System.Configuration;

namespace ImperaPlus.Backend
{
    public static class Authentication
    {
        public static string PublicClientId { get; private set; }

        public static IDataProtectionProvider DataProtectionProvider { get; private set; }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static void ConfigureAuth(IAppBuilder app)
        {
            PublicClientId = "self";
            
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider((IOwinContext context) =>
                {
                    var requestScope = context.GetAutofacLifetimeScope();
                    return requestScope.Resolve<ApplicationUserManager>();

                }, PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2), // TODO: CS: Refresh?
                AllowInsecureHttp = true // TODO: CS: Disallow
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
            // Parse token from query string for websocket connections
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                Provider = new QueryStringOAuthBearerProvider("bearer_token")
            });
            // Parse token from cookie for admin interface/MVC
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                Provider = new CookieOAuthBearerProvider("bearer_token")
            });

            var microsoftOptions = new Microsoft.Owin.Security.MicrosoftAccount.MicrosoftAccountAuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["AuthMicrosoftClientId"],
                ClientSecret = ConfigurationManager.AppSettings["AuthMicrosoftClientSecret"]
            };
            microsoftOptions.Scope.Add("wl.basic");
            microsoftOptions.Scope.Add("wl.emails");
            app.UseMicrosoftAccountAuthentication(microsoftOptions);


            var facebookOptions = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions
            {
                AppId = ConfigurationManager.AppSettings["AuthFacebookAppId"],
                AppSecret = ConfigurationManager.AppSettings["AuthFacebookAppSecret"]
            };
            facebookOptions.Scope.Add("email");
            app.UseFacebookAuthentication(facebookOptions);

            // Store protection provider for later use, e.g., UserToken generation
            DataProtectionProvider = app.GetDataProtectionProvider();
        }
    }
}
