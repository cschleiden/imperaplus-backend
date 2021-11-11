using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Utilities;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Domain.Games.Distribution
{
    public class DefaultMapDistribution : IMapDistribution
    {
        public void Distribute(GameOptions gameOptions, IEnumerable<Team> teams, MapTemplate mapTemplate, Map map,
            IRandomGen random)
        {
            var shuffledCountries = map.Countries.Shuffle(random).ToArray();

            var players = teams.SelectMany(x => x.Players).Shuffle(random).ToList();

            // Remaining countries are neutral
            var countryCount = shuffledCountries.Count();
            var countriesToDistribute = countryCount - countryCount % players.Count();

            for (int i = 0, playerIndex = 0; i < countriesToDistribute; ++i)
            {
                map.UpdateOwnership(players[playerIndex], shuffledCountries[i]);

                playerIndex = (playerIndex + 1) % players.Count();
            }
        }
    }
}
