using System.Linq;
using System.Threading.Tasks;
using ImperaPlus.GeneratedClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.IntegrationTests
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
