using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Map;
using System.Collections.Generic;
using System.Linq;

namespace ImperaPlus.TestSupport
{
    public class TestHelper
    {
        public static IEnumerable<Team> GetEnemyTeams(Game game, Player player)
        {
            return game.Teams.Where(x => !x.Players.Contains(player));
        }

        public static IEnumerable<Country> GetEnemyCountries(Game game, Player player)
        {
            var enemyTeams = GetEnemyTeams(game, player);

            return enemyTeams.SelectMany(x => x.Players).SelectMany(x => x.Countries);
        }

        public static Country GetCountryWithFriendlyConnection(Game game, Player player, MapTemplate mapTemplate)
        {            
            return player.Countries.First(
                    c => player.Countries.Any(c2 => mapTemplate.AreConnected(c.CountryIdentifier, c2.CountryIdentifier)));
        }

        public static Country GetConnectedFriendlyCountry(Game game, Player player, Country origin, MapTemplate mapTemplate)
        {
            return game.Teams.Where(x => x.Players.Contains(player))
                    .SelectMany(t => t.Players)
                    .SelectMany(p => p.Countries).First(c => mapTemplate.AreConnected(origin.CountryIdentifier, c.CountryIdentifier));
        }

        public static Country GetCountryWithEnemyConnection(Game game, Player player, MapTemplate mapTemplate)
        {
            var enemyCountries = GetEnemyCountries(game, player);

            return player.Countries.First(
                    c => enemyCountries.Any(c2 => mapTemplate.AreConnected(c.CountryIdentifier, c2.CountryIdentifier)));
        }

        public static Country GetConnectedEnemyCountry(Game game, Player player, Country origin, MapTemplate mapTemplate)
        {
            return game.Teams.Where(x => !x.Players.Contains(player))
                    .SelectMany(t => t.Players)
                    .SelectMany(p => p.Countries).First(c => mapTemplate.AreConnected(origin.CountryIdentifier, c.CountryIdentifier));
        }
    }
}
