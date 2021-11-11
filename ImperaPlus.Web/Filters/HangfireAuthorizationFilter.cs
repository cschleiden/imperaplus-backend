using Hangfire.Dashboard;

namespace ImperaPlus.Web
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            return httpContext.User.Identity.IsAuthenticated &&
                   httpContext.User.IsInRole("admin");
        }
    }
}
