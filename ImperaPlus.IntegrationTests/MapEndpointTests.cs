using ImperaPlus.GeneratedClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace ImperaPlus.IntegrationTests
{
    [TestClass]
    public class MapEndpointTests : BaseIntegrationTest
    {
        [TestMethod]
        [TestProperty("Controller", "Map")]
        public async Task MapTemplate_GetAllSummary()
        {
            var client = await ApiClient.GetAuthenticatedClientDefaultUser<MapClient>();
            var mapTemplateSummaries = await client.GetAllSummaryAsync();

            Assert.IsNotNull(mapTemplateSummaries);
            Assert.IsTrue(mapTemplateSummaries.Any());
        }

        [TestMethod]
        [TestProperty("Controller", "Map")]
        public async Task MapTemplate_GetSingle()
        {
            var client = await ApiClient.GetAuthenticatedClientDefaultUser<MapClient>();
            var mapTemplateSummaries = await client.GetAllSummaryAsync();

            var mapTemplate = await client.GetMapTemplateAsync(mapTemplateSummaries.First().Name);

            Assert.IsNotNull(mapTemplate);
            Assert.IsNotNull(mapTemplate.Name);
            Assert.IsNotNull(mapTemplate.Countries);
            Assert.IsTrue(mapTemplate.Countries.Any());
            Assert.IsNotNull(mapTemplate.Connections);
            Assert.IsTrue(mapTemplate.Connections.Any());
            Assert.IsNotNull(mapTemplate.Continents);
            Assert.IsTrue(mapTemplate.Continents.Any());
        }
    }  
}