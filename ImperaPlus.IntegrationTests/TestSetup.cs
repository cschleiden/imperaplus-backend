using System.IO;
using System.Reflection;
using Autofac;
using ImperaPlus.GeneratedClient;
using ImperaPlus.TestSupport;
using ImperaPlus.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.Integration.Tests
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

            TestSetup.TestServer = new TestServer(new WebHostBuilder()
                .UseEnvironment(EnvironmentName.Development)
                .UseContentRoot(contentRoot)
                .UseStartup<Startup>());
            TestSetup.TestServer.BaseAddress = new System.Uri("http://localhost", System.UriKind.Absolute);

            var client = ApiClient.GetClient<AccountClient>().Result;
            for (int i = 0; i < 4; ++i)
            {
                var model = new RegisterBindingModel();

                model.CallbackUrl = client.BaseUrl.ToString();
                model.Language = "de";
                model.Email = i + "test@localhost";
                model.UserName = "TestUser" + i;
                model.Password = "TestPassword" + i;
                model.ConfirmPassword = "TestPassword" + i;

                client.RegisterAsync(model).Wait();
            }
        }

        public static void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<SynchronousBackgroundJobClient>().AsImplementedInterfaces();
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
