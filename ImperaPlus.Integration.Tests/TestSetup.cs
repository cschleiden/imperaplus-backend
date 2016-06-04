using System.Configuration;
using System.Net.Http;
using ImperaPlus.Backend;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using ImperaPlus.TestSupport;
using ImperaPlus.DTO.Account;
using ImperaPlus.Integration.Tests.Support;

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

            using (var client = ApiClient.GetClient().Result)
            {
                for (int i = 0; i < 4; ++i)
                {
                    var model = new RegisterBindingModel();

                    model.CallbackUrl = client.BaseAddress.ToString();
                    model.Language = "de";
                    model.Email = i + "test@localhost";
                    model.UserName = ConfigurationManager.AppSettings["TestUser" + i];
                    model.Password = ConfigurationManager.AppSettings["TestPassword" + i];
                    model.ConfirmPassword = ConfigurationManager.AppSettings["TestPassword" + i];

                    var result = client.PostAsJsonAsync("api/Account/Register", model).Result;
                    result.AssertIsSuccessful();
                }
            }
        }
       
        public static void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<SynchronousBackgroundJobClient>().AsImplementedInterfaces();
        }
    }
}
