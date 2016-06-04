using Autofac;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using ApplicationException = ImperaPlus.Application.Exceptions.ApplicationException;
using System.Collections.Concurrent;

namespace ImperaPlus.Application
{
    public sealed class MapTemplateProvider : IMapTemplateProvider
    {
        private readonly ConcurrentDictionary<string, MapTemplate> mapTemplates = new ConcurrentDictionary<string, MapTemplate>();
        private readonly Autofac.ILifetimeScope scope;

        public MapTemplateProvider(Autofac.ILifetimeScope scope)
        {
            this.scope = scope;
        }

        public MapTemplate GetTemplate(string name)
        {
            return this.mapTemplates.GetOrAdd(name, this.GetTemplateFromStore);
        }

        private MapTemplate GetTemplateFromStore(string name)
        {
            using (var lifetimeScope = scope.BeginLifetimeScope("AutofacWebRequest"))
            using (var unitOfWork = lifetimeScope.Resolve<IUnitOfWork>())
            {
                var template = unitOfWork.MapTemplates.Get(name);
                if (template == null)
                {
                    throw new ApplicationException("Cannot find map template", ErrorCode.CannotFindMapTemplate);
                }

                return template;
            }
        }        
    }
}