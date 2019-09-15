using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Autofac.Extensions.DependencyInjection;
using NLog.Web;

namespace ImperaPlus.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) => new WebHostBuilder()
            .UseNLog()
#if !DEBUG
            .UseApplicationInsights()
#endif
            .CaptureStartupErrors(true)
            .UseSetting("detailedErrors", "true")
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureServices(services => services.AddAutofac())
            .UseIISIntegration()
            .UseKestrel()
            .UseStartup<Startup>()
            .Build();
    }
}
