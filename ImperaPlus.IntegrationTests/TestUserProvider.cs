using ImperaPlus.Web.Providers;
using Microsoft.AspNetCore.Http;

namespace ImperaPlus.IntegrationTests
{
    public class TestUserProvider : UserProvider
    {
        public TestUserProvider(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
        }

        public override bool IsAdmin()
        {
            return true;
        }
    }
}
