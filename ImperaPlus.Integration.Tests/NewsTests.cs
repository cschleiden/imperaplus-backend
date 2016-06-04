using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ImperaPlus.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.Integration.Tests
{
    [TestClass]    
    public class NewsTests : BaseIntegrationTest
    {        
        private const string GetAll = "api/news";

        [TestMethod]
        [Integration]
        [TestProperty("Controller", "News")]
        public async Task News_GetAll()
        {
            var response = await this.HttpClientDefault.GetAsync(GetAll);
            var news = await response.Content.ReadAsAsync<IEnumerable<DTO.News.NewsItem>>();

            Assert.IsNotNull(news);
            Assert.IsTrue(news.Any());
        }
    }
}
