using ImperaPlus.Domain.Map;
using System.Collections.Generic;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Domain.Games.Distribution
{
    public interface IMapDistribution
    {
        void Distribute(GameOptions gameOptions, IEnumerable<Team> teams, MapTemplate mapTemplate, Map map,
            IRandomGen random);
    }
}
