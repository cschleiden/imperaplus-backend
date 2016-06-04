using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using ImperaPlus.Application;
using ImperaPlus.Application.Jobs;
using ImperaPlus.Backend.App_Start;
using ImperaPlus.Backend.Providers;
using ImperaPlus.DataAccess;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Owin;

[assembly: OwinStartup(typeof(ImperaPlus.Backend.Startup))]
namespace ImperaPlus.Backend
{
    public class Startup
    {
        public static JsonSerializerSettings JsonSerializerSettings { get; set; }

        public static HttpConfiguration HttpConfiguration { get; set; }
        public static DbMigrationsConfiguration<ImperaContext> DbConfiguration { get; set; }

        /// <summary>
        /// Test support: Register help page 
        /// </summary>            
        internal static bool IncludeAreas = true;

        /// <summary>
        /// Test support: Work on background threads
        /// </summary>
        public static bool StartHangfire = true;

        /// <summary>
        /// Test support: Require user confirmation
        /// </summary>
        internal static bool RequireUserConfirmation = true;

        public static IDatabaseInitializer<ImperaContext> DbInitializer = new DbInitializer();

        public void Configuration(IAppBuilder app)
        {
            InitJsonSerializationSettings();

            HttpConfiguration = new HttpConfiguration();
            var hubConfiguration = new HubConfiguration();

            // Enable cross origin access
            app.UseCors(CorsOptions.AllowAll);
            HttpConfiguration.EnableCors(new EnableCorsAttribute("*", "*", "*")
            {
                PreflightMaxAge = 86400,
                ExposedHeaders = { "X-MiniProfiler-Ids" }
            });

            if (IncludeAreas)
            {
                AreaRegistration.RegisterAllAreas();
            }

            DependencyInjectionConfig.Init(app, HttpConfiguration, hubConfiguration, IncludeAreas);

            Authentication.ConfigureAuth(app);

            // Setup mini profiler
            StackExchange.Profiling.EntityFramework6.MiniProfilerEF6.Initialize();
            StackExchange.Profiling.MiniProfiler.Settings.SqlFormatter = new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();

            DbConfig.Init(DbInitializer);

            WebApiConfig.Register(HttpConfiguration);

            app.UseWebApi(HttpConfiguration);

            // Configure SignalR
            hubConfiguration.EnableJavaScriptProxies = false;
            hubConfiguration.EnableDetailedErrors = true;
            app.MapSignalR(hubConfiguration);

            AutoMapperConfig.Configure();

            // Json serializer settings for help page (TODO: Unify)
            JsonConvert.DefaultSettings = () => JsonSerializerSettings;

            if (StartHangfire)
            {
                Hangfire.GlobalConfiguration.Configuration
                    .UseAutofacActivator(DependencyInjectionConfig.Container, false)
                    .UseFilter(new JobExpirationTimeAttribute())
                    .UseSqlServerStorage("DefaultConnection", new SqlServerStorageOptions
                    {
                        PrepareSchemaIfNecessary = true
                    })
                    .UseNLogLogProvider();

                // Enable dashboard
                app.UseHangfireDashboard("/admin/hangfire", new DashboardOptions
                {
                    AuthorizationFilters = new[] { new AuthorizationFilter { Roles = "admin" } }
                });

                app.UseHangfireServer(new BackgroundJobServerOptions
                {
                    Queues = new[] { JobQueues.Critical, JobQueues.Normal }
                });

                Hangfire.Common.JobHelper.SetSerializerSettings(new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });


                // Configure Impera jobs
                JobConfig.Configure();
            }

            // Miniprofiler
            StackExchange.Profiling.MiniProfiler.Settings.Results_List_Authorize = (request) => true;
            StackExchange.Profiling.MiniProfiler.Settings.Storage = new StackExchange.Profiling.Storage.HttpRuntimeCacheStorage(TimeSpan.FromDays(1));

            this.ConfigureLogging();
        }

        private static void InitJsonSerializationSettings()
        {
            JsonSerializerSettings = new JsonSerializerSettings()
            {
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc,
                DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat,
                ContractResolver = new SignalRContractResolver()
            };
            JsonSerializerSettings.Converters.Add(new StringEnumConverter
            {
                CamelCaseText = false,
                AllowIntegerValues = false
            });
        }

        private void ConfigureLogging()
        {
            var loggingConfig = new NLog.Config.LoggingConfiguration();

            var traceTarget = new NLog.Targets.TraceTarget();
            loggingConfig.AddTarget("trace", traceTarget);
            var debuggerTarget = new NLog.Targets.DebuggerTarget();
            loggingConfig.AddTarget("debugger", debuggerTarget);

            loggingConfig.LoggingRules.Add(new NLog.Config.LoggingRule("*", NLog.LogLevel.Debug, traceTarget));
            loggingConfig.LoggingRules.Add(new NLog.Config.LoggingRule("*", NLog.LogLevel.Debug, debuggerTarget));

            NLog.LogManager.Configuration = loggingConfig;            
        }        
    }
}
