using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace ImperaPlus.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            logger.Debug("Init main");

            try
            {
                BuildWebHost(args).Run();
            }
            catch (Exception exception)
            {
                logger.Fatal(exception, "Could not init app");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHost BuildWebHost(string[] args) => new WebHostBuilder()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            })
            .UseNLog()
#if !DEBUG
            .UseApplicationInsights()
#endif
            .CaptureStartupErrors(true)
            .UseSetting("detailedErrors", "true")
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();

            })
            .ConfigureServices(services => services.AddAutofac())
            .UseIISIntegration()
            .UseKestrel()
            .UseStartup<Startup>()
            .Build();
    }
}
