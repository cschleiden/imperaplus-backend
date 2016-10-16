using System;
using System.Data.Entity;
using Autofac;
using ImperaPlus.Backend.App_Start;
using ImperaPlus.DataAccess;
using Microsoft.Owin.Testing;

namespace ImperaPlus.Backend
{
    /// <summary>
    /// This hosts an internal version of the website to run integration tests which directly communicate
    /// with the backend via an in-memory adapter.
    /// </summary>
    public static class IntegrationTestServer
    {
        public static TestServer Create(
            IDatabaseInitializer<ImperaContext> dbInitializer, 
            // DbMigrationsConfiguration<ImperaContext> configuration,
            Action<ContainerBuilder> registerAction)
        {
            // Exclude API help page
            Startup.IncludeAreas = false;
            Startup.RequireUserConfirmation = false;
            Startup.StartHangfire = false;
            
            Startup.DbInitializer = dbInitializer;
            // Startup.DbConfiguration = configuration;

            DependencyInjectionConfig.RegisterAction = registerAction;

#if false
            System.Diagnostics.Debugger.Launch();
#endif

            return TestServer.Create<Startup>();
        }
    }
}