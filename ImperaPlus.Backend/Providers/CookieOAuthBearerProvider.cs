using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace ImperaPlus.Backend.Providers
{
    public class CookieOAuthBearerProvider : OAuthBearerAuthenticationProvider
    {
        readonly string _name;

        public CookieOAuthBearerProvider(string name)
        {
            _name = name;
        }

        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var value = context.Request.Cookies[_name];
            if (!string.IsNullOrEmpty(value))
            {
                context.Token = value;
            }

            return Task.FromResult<object>(null);
        }
    }
}