using System;
using ImperaPlus.Domain.Enums;

namespace ImperaPlus.Domain.Games.Distribution
{
    public static class MapDistributionFactory
    {
        public static IMapDistribution Create(MapDistribution mapDistribution)
        {
            switch (mapDistribution)
            {
                case MapDistribution.Default:
                    return new DefaultMapDistribution();

                case MapDistribution.Malibu:
                    return new MalibuMapDistribution();

                case MapDistribution.Malibu3:
                    return new MalibuMapDistribution(3);

                //case MapDistribution.TeamCluster:
                    // return new TeamClusterDistribution();

                default:
                    throw new ArgumentOutOfRangeException("mapDistribution");
            }
        }
    }
}
