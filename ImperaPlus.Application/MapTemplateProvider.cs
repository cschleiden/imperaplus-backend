using ImperaPlus.DataAccess.ConvertedMaps;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ImperaPlus.Application
{
    public sealed class MapTemplateProvider : IMapTemplateProvider
    {
        private readonly Dictionary<string, Func<MapTemplate>> mapTemplateFactory = new Dictionary<string, Func<MapTemplate>>();
        private readonly ConcurrentDictionary<string, MapTemplate> mapTemplates = new ConcurrentDictionary<string, MapTemplate>();
        
        public MapTemplateProvider()
        {
            var type = typeof(Maps);
            var methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            foreach(var method in methods)
            {
                this.mapTemplateFactory.Add(method.Name.ToLowerInvariant(), () => (MapTemplate)method.Invoke(null, null));
            }
        }

        public MapTemplate GetTemplate(string name)
        {
            Domain.Utilities.Require.NotNullOrEmpty(name, nameof(name));

            return this.mapTemplates.GetOrAdd(name, this.GetTemplateFromStore);
        }

        private MapTemplate GetTemplateFromStore(string name)
        {
            var transformedName = name.ToLowerInvariant().Replace(" ", "_");

            if (!this.mapTemplateFactory.ContainsKey(transformedName))
            {
                throw new Exceptions.ApplicationException("Cannot find map template", ErrorCode.CannotFindMapTemplate);
            }

            return this.mapTemplateFactory[transformedName]();
        }        
    }
}