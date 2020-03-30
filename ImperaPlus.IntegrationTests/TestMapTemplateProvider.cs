using ImperaPlus.Application;

namespace ImperaPlus.IntegrationTests
{
    public class TestMapTemplateProvider : MapTemplateProvider
    {
        public TestMapTemplateProvider()
            : base()
        {
        }

        protected override bool IncludeMap(string mapName)
        {
            return true;
        }
    }
}
