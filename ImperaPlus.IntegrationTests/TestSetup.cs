using System;
using System.IO;
using System.Reflection;
using Autofac;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain.Services;
using ImperaPlus.DTO.Account;
using ImperaPlus.GeneratedClient;
using ImperaPlus.TestSupport;
using ImperaPlus.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ImperaPlus.IntegrationTests
{
    [TestClass]
    public static class TestSetup
    {
        public static TestServer TestServer;

        [AssemblyInitialize]
        public static void Init(TestContext testContext)
        {
            var startupAssembly = typeof(Startup).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(string.Empty, startupAssembly);

            // User account do not need verification
            Startup.RequireUserConfirmation = false;
            Startup.RunningUnderTest = true;
            Application.TestSupport.RunningUnderTest = true;

            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings();
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy(), false));
                return settings;
            };

            TestSetup.TestServer = new TestServer(new WebHostBuilder()
                .UseEnvironment(EnvironmentName.Development)
                .UseContentRoot(contentRoot)
                .ConfigureServices(services => TestSetup.RegisterTypes(services))
                .UseStartup<Startup>());
            TestSetup.TestServer.BaseAddress = new Uri("http://localhost", UriKind.Absolute);
            
            for (int i = 0; i < 4; ++i)
            {
                TestSetup.RegisterClient(i);
            }
        }

        public static void RegisterClient(int i)
        {
            var client = ApiClient.GetClient<AccountClient>().Result;
            var model = new RegisterBindingModel
            {
                CallbackUrl = client.BaseUrl.ToString(),
                Language = "de",
                Email = i + "test@localhost",
                UserName = "TestUser" + i,
                Password = "TestPassword" + i,
                ConfirmPassword = "TestPassword" + i
            };

            try
            {
                client.RegisterAsync(model).Wait();
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions != null && e.InnerExceptions.Count > 0 && e.InnerExceptions[0] is ImperaPlusException)
                {
                    if ((e.InnerExceptions[0] as ImperaPlusException).StatusCode == 400)
                    {
                        // ignore
                    }
                }
                else
                {
                    throw;
                }
            }
        }

        public static void RegisterTypes(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<DbSeed, TestDbSeed>();

            var builder = new ContainerBuilder();
            Startup.TestContainerBuilder = builder;

            builder.RegisterType<SynchronousBackgroundJobClient>().AsImplementedInterfaces();
            builder.RegisterType<FakeEmailService>().AsImplementedInterfaces();
           
            builder.RegisterType<ImperaPlus.IntegrationTests.TestUserProvider>().AsImplementedInterfaces();
            builder.RegisterType<ImperaPlus.IntegrationTests.TestMapTemplateProvider>().As<IMapTemplateProvider>();
        }

        public static string GetProjectPath(string solutionRelativePath, Assembly assembly)
        {
            var projectName = assembly.GetName().Name;
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, "ImperaPlus.sln"));
                if (solutionFileInfo.Exists)
                {
                    return Path.GetFullPath(Path.Combine(directoryInfo.FullName, solutionRelativePath, projectName));
                }

                directoryInfo = directoryInfo.Parent;
            }
            while (directoryInfo.Parent != null);

            throw new System.Exception();
        }
    }
}
