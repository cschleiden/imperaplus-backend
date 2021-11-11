using ImperaPlus.DataAccess.ConvertedMaps;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ImperaPlus.Application
{
    public class MapTemplateProvider : IMapTemplateProvider
    {
        private readonly Dictionary<string, Func<MapTemplate>> mapTemplateFactory = new();
        private readonly ConcurrentDictionary<string, MapTemplate> mapTemplates = new();

        public MapTemplateProvider()
        {
            var type = typeof(Maps);
            var methods =
                type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            foreach (var method in methods)
            {
                var mapName = method.Name.ToLowerInvariant();
                if (!IncludeMap(mapName))
                {
                    continue;
                }

                mapTemplateFactory.Add(mapName, () => (MapTemplate)method.Invoke(null, null));
            }
        }

        public MapTemplate GetTemplate(string name)
        {
            Domain.Utilities.Require.NotNullOrEmpty(name, nameof(name));

            return mapTemplates.GetOrAdd(name, GetTemplateFromStore);
        }

        protected virtual bool IncludeMap(string mapName)
        {
            return mapName != "testmap";
        }

        private MapTemplate GetTemplateFromStore(string name)
        {
            var transformedName = name.ToLowerInvariant().Replace(" ", "_");

            if (!mapTemplateFactory.ContainsKey(transformedName))
            {
                throw new Exceptions.ApplicationException("Cannot find map template", ErrorCode.CannotFindMapTemplate);
            }

            return mapTemplateFactory[transformedName]();
        }
    }
}
