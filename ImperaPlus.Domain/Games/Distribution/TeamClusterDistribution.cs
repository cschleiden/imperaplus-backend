using System.Collections.Generic;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Domain.Games.Distribution
{
    public class TeamClusterDistribution : IMapDistribution
    {
        public void Distribute(GameOptions gameOptions, IEnumerable<Team> teams, Domain.Map.MapTemplate mapTemplate, Map map, IRandomGen random)
        {
            // var players = teams.SelectMany(x => x.Players).ToArray();
            // var numberOfPlayers = players.Count();
            //
            // var shuffledCountries = map.Countries.Shuffle().ToArray();
            // var countryIdx = 0;
            //
            // foreach (var team in teams)
            // {
            //     // Pick team starting country
            //     Country teamCountry = null;
            //     var teamCountries = new List<Country>();
            //
            //     for (int i = 0; i < 10; ++i)
            //     {
            //         var country = shuffledCountries[countryIdx++];
            //
            //         var connectedCountryIdentifiers = mapTemplate.GetConnectedCountries(country.CountryIdentifier);
            //         bool tryNext = false;
            //         foreach (var connectedCountry in connectedCountryIdentifiers.Select(x => map.GetCountry(x)))
            //         {
            //             if (connectedCountry.Player != null)
            //             {
            //                 // Try again with another country
            //                 tryNext = true;
            //                 break;
            //             }
            //         }
            //
            //         if (tryNext)
            //         {
            //             continue;
            //         }
            //
            //         teamCountry = country;
            //         break;
            //     }
            //
            //     if (teamCountry == null)
            //     {
            //         Log.Fatal().Message("Could not distribute countries to all players for TeamCluster").Write();
            //     }
            // }
        }
    }
}
