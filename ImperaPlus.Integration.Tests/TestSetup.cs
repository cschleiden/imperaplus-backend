using System.Configuration;
using ImperaPlus.Backend;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using ImperaPlus.TestSupport;
using ImperaPlus.GeneratedClient;

namespace ImperaPlus.Integration.Tests
{
    [TestClass]
    public static class TestSetup
    {
        public static TestServer TestServer;

        [AssemblyInitialize]
        public static void Init(TestContext testContext)
        {
            TestServer = IntegrationTestServer.Create(new TestDbInitializer(), RegisterTypes);

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
