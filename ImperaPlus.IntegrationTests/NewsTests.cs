using ImperaPlus.GeneratedClient;
using ImperaPlus.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace ImperaPlus.Integration.Tests
{
    [TestClass]    
    public class NewsTests : BaseIntegrationTest
    {
        [TestMethod]
        [TestProperty("Controller", "News")]
        public async Task News_GetAll()
        {
            var client = await ApiClient.GetAuthenticatedClientDefaultUser<NewsClient>();
            var news = await client.GetAllAsync();

            Assert.IsNotNull(news);
            Assert.IsTrue(news.Any());
        }
    }
}
