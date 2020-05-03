using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Utilities;
using ImperaPlus.Domain.Map;
using NLog.Fluent;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Domain.Games.Distribution
{
    public class MalibuMapDistribution : IMapDistribution
    {
        private int countriesPerPlayer;

        public MalibuMapDistribution(int countriesPerPlayer = 1)
        {
            this.countriesPerPlayer = countriesPerPlayer;
        }

        public void Distribute(GameOptions gameOptions, IEnumerable<Team> teams, MapTemplate mapTemplate, Map map, IRandomGen random)
        {
            // Default all countries to 1 unit
            foreach(var country in map.Countries)
            {
                country.Units = 1;
            }

            var players = teams.SelectMany(x => x.Players).ToArray();
            var numberOfPlayers = players.Count();

            var shuffledCountries = map.Countries.Shuffle(random).ToArray();
            var countryIdx = 0;

            for (int c = 0; c < this.countriesPerPlayer; ++c)
            {
                foreach (var player in players)
                {
                    bool countryDistributed = false;
                    for (int i = 0; i < 10; ++i)
                    {
                        // Get a random country
                        var country = shuffledCountries[countryIdx++ % shuffledCountries.Count()];

                        // Check all connected countries
                        var connectedCountryIdentifiers = mapTemplate.GetConnectedCountries(country.CountryIdentifier);
                        if (connectedCountryIdentifiers.Select(x => map.GetCountry(x)).Any(c => !c.IsNeutral && c.PlayerId != player.Id)) {
                            // One of the connected countries already belongs to another player, try the next country
                            continue;
                        }

                        // All connected countries are without a player, claim this one.
                        map.UpdateOwnership(player, country);
                        country.Units = gameOptions.InitialCountryUnits;
                        countryDistributed = true;
                        break;
                    }

                    // Wasn't able to distribute a country in 10 attempts, pick a random one
                    if (!countryDistributed)
                    {
                        var backupCountry = shuffledCountries.FirstOrDefault(x => x.PlayerId == Guid.Empty);
                        if (backupCountry != null)
                        {
                            // Reached here because we couldn't find a country in the number of allowed iterations. Just take this one
                            map.UpdateOwnership(player, backupCountry);
                            backupCountry.Units = gameOptions.InitialCountryUnits;
                        }
                        else
                        {
                            Log.Fatal().Message("Could not distribute countries to all players in malibu").Write();
                        }
                    }
                }
            }
        }
    }
}