using Hangfire.Dashboard;

namespace ImperaPlus.Web
{
    public class HangfireAuthorizationFilter : Hangfire.Dashboard.IDashboardAuthorizationFilter
    {
        public bool Authorize(Hangfire.Dashboard.DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            return httpContext.User.Identity.IsAuthenticated &&
                        httpContext.User.IsInRole("admin");
        }
    }
}
