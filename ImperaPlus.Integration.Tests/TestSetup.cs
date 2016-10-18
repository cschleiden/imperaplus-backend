using System.Configuration;
using Autofac;
using ImperaPlus.GeneratedClient;
using ImperaPlus.TestSupport;
using ImperaPlus.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
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
            TestSetup.TestServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            var client = ApiClient.GetClient<AccountClient>().Result;
            for (int i = 0; i < 4; ++i)
            {
                var model = new RegisterBindingModel();

                model.CallbackUrl = client.BaseUrl.ToString();
                model.Language = "de";
                model.Email = i + "test@localhost";
                model.UserName = ConfigurationManager.AppSettings["TestUser" + i];
                model.Password = ConfigurationManager.AppSettings["TestPassword" + i];
                model.ConfirmPassword = ConfigurationManager.AppSettings["TestPassword" + i];

                client.RegisterAsync(model).Wait();
            }
        }

        public static void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<SynchronousBackgroundJobClient>().AsImplementedInterfaces();
        }
    }
}
