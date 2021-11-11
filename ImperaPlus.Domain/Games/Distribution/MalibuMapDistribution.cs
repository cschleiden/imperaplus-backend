﻿using System;
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

        public void Distribute(GameOptions gameOptions, IEnumerable<Team> teams, MapTemplate mapTemplate, Map map,
            IRandomGen random)
        {
            // Default all countries to 1 unit
            foreach (var country in map.Countries)
            {
                country.Units = 1;
            }

            var players = teams.SelectMany(x => x.Players).ToArray();
            var shuffledCountries = map.Countries.Shuffle(random).ToArray();
            var countryIdx = 0;

            for (var c = 0; c < countriesPerPlayer; ++c)
            {
                foreach (var player in players)
                {
                    var countryDistributed = false;

                    // Try to find a country that is not next to another player
                    for (var i = 0; i < shuffledCountries.Length; ++i)
                    {
                        // Get a random country
                        var country = shuffledCountries[countryIdx++ % shuffledCountries.Count()];
                        if (country.PlayerId != Guid.Empty)
                        {
                            continue;
                        }

                        // Check all connected countries
                        var connectedCountryIdentifiers = mapTemplate.GetConnectedCountries(country.CountryIdentifier);
                        if (connectedCountryIdentifiers.Select(x => map.GetCountry(x))
                            .Any(c => !c.IsNeutral && c.TeamId != player.TeamId))
                        {
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
