using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ImperaPlus.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using ImperaPlus.Integration.Tests.Support;

namespace ImperaPlus.Integration.Tests
{
    [TestClass]
    public class MapEndpointTests : BaseIntegrationTest
    {
        private const string GetAll = "api/map";

        [TestMethod]
        [Integration]
        [TestProperty("Controller", "Map")]
        public async Task MapTemplate_GetAllSummary()
        {
            var response = await this.HttpClientDefault.GetAsync(GetAll);
            var mapTemplateSummaries =
                await response.Content.ReadAsAsync<IEnumerable<DTO.Games.Map.MapTemplateSummary>>();

            Assert.IsNotNull(mapTemplateSummaries);
            Assert.IsTrue(mapTemplateSummaries.Any());
        }

        [TestMethod]
        [Integration]
        [TestProperty("Controller", "Map")]
        public async Task MapTemplate_GetSingle()
        {
            var summaries = await
                (await this.HttpClientDefault.GetAsync(GetAll)).Content
                    .ReadAsAsync<IEnumerable<DTO.Games.Map.MapTemplateSummary>>();

            var response = await this.HttpClientDefault.GetAsync(GetAll + "/" + summaries.First().Name);
            var mapTemplate =
                await response.Content.ReadAsAsync<DTO.Games.Map.MapTemplate>();

            response.AssertIsSuccessful();
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