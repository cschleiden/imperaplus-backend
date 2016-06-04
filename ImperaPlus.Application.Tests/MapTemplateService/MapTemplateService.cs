using System.Linq;
using Autofac;
using ImperaPlus.DTO.Games.Map;
using ImperaPlus.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.Application.Tests.MapTemplateService
{
    [TestClass]
    public class MapTemplateServiceTests : TestBase
    {
        private const string FirstTemplateName = "mapTpl";

        private Domain.Map.MapTemplate firstTemplate;
        private IMapTemplateService mapTemplateService;

        public MapTemplateServiceTests()
        {
            this.firstTemplate = new Domain.Map.MapTemplate(FirstTemplateName);
        }

        [TestInitialize]
        public void TestSetup()
        {
            var mapRepository = this.UnitOfWork.MapTemplates;
            mapRepository.Add(this.firstTemplate);
            this.UnitOfWork.Commit();

            this.mapTemplateService = this.Scope.Resolve<IMapTemplateService>();
        }

        [TestMethod]
        [LayerApplication]
        public void GetMapTemplate()
        {
            // Arrange
            

            // Act
            var template = this.mapTemplateService.Get(this.firstTemplate.Name);

            // Assert
            Assert.IsNotNull(template);
        }

        [TestMethod]
        [LayerApplication]
        public void QueryMaps()
        {
            var maps = this.mapTemplateService.QuerySummary();

            Assert.IsNotNull(maps);
            Assert.IsTrue(maps.Any());
            Assert.IsTrue(maps.Any(x => x.Id == this.firstTemplate.Id));
        }
    }
}
