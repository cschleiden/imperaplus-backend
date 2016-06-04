using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImperaPlus.Backend;
using Microsoft.Owin.Testing;
using ImperaPlus.DTO.Account;
using System.Configuration;
using Autofac;
using ImperaPlus.TestSupport;
using ImperaPlus.Integration.Tests.Support;

namespace ImperaPlus.Integration.Tests
{
    [TestClass]
    public class BaseIntegrationTest
    {
        protected HttpClient HttpClientDefault;
        protected HttpClient HttpClientAdmin;

        [TestInitialize]
        public virtual void Initialize()
        {
            this.HttpClientDefault = ApiClient.GetAuthenticatedClientDefaultUser().Result;
            this.HttpClientAdmin = ApiClient.GetAuthenticatedClientAdminUser().Result;
        }

        [TestCleanup]
        public virtual void Cleanup()
        {
            this.HttpClientDefault.Dispose();
        }

        public void Log(string message, params object[] args)
        {
            this.TestContext.WriteLine(message, args);
        }

        public TestContext TestContext { get; set; }
    }
}