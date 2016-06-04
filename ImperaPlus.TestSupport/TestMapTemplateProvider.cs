using System;
using System.Linq;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.TestSupport
{
    public class TestMapTemplateProvider : IMapTemplateProvider
    {
        private readonly Func<IUnitOfWork> unitOfWorkFactory;

        public TestMapTemplateProvider(Func<IUnitOfWork> unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public MapTemplate GetTemplate(string name)
        {
            var unitOfWork = this.unitOfWorkFactory();

            var template = unitOfWork.MapTemplates.Query().FirstOrDefault(x => x.Name == name);

            if (template == null)
            {
                throw new ArgumentException("MapTemplate not found", "name");
            }

            return template;
        }
    }
}
