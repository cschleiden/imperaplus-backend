using Microsoft.Owin.Builder;
using Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;

namespace ImperaPlus.Web
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseAppBuilder(
            this IApplicationBuilder app, 
            Action<IAppBuilder> configure)
        {
            app.UseOwin(addToPipeline =>
            {
                addToPipeline(next =>
                {
                    var appBuilder = new AppBuilder();
                    appBuilder.Properties["builder.DefaultApp"] = next;

                    configure(appBuilder);

                    return appBuilder.Build<AppFunc>();
                });
            });

            return app;
        }

        public static void UseSignalR2(this IApplicationBuilder app, HubConfiguration hubConfiguration)
        {
            app.UseAppBuilder(appBuilder => appBuilder.Map("/signalr", map =>
            {
                //map.UseWebSockets();
                map.UseCors(CorsOptions.AllowAll);
                map.RunSignalR(hubConfiguration);
            }));
        }
    }
}
