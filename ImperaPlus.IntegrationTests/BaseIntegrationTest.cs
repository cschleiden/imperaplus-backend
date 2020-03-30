using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.IntegrationTests
{
    [TestClass]
    public class BaseIntegrationTest
    {
        [TestInitialize]
        public virtual void Initialize()
        {
        }

        [TestCleanup]
        public virtual void Cleanup()
        {
        }

        public void Log(string message, params object[] args)
        {
            this.TestContext.WriteLine(message, args);
        }

        public TestContext TestContext { get; set; }
    }
}