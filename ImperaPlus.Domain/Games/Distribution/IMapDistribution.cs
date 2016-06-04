using ImperaPlus.Domain.Map;
using System.Collections.Generic;

namespace ImperaPlus.Domain.Games.Distribution
{
    public interface IMapDistribution
    {
        void Distribute(IEnumerable<Team> teams, MapTemplate mapTemplate, Map map);
    }
}