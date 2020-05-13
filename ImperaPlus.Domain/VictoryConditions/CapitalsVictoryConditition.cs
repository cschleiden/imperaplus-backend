using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Utilities;
using System.Linq;

namespace ImperaPlus.Domain.VictoryConditions
{
    public class CapitalsVictoryCondition : SurvivalVictoryCondition
    {
        public override void Initialize(Games.Game game, IRandomGen random)
        {
            // Distribute capitals
            foreach (var team in game.Teams)
            {
                foreach (var player in team.Players)
                {
                    var capitalCountry = player.Countries.RandomElement(random);
                    capitalCountry.Flags |= CountryFlags.Capital;
                }
            }
        }

        public override VictoryConditionResult Evaluate(Player player, Games.Map map)
        {
            var result = base.Evaluate(player, map);
            if (result != VictoryConditionResult.Inconclusive)
            {
                return result;
            }

            if (!map.GetCountriesForTeam(player.TeamId).Any(x => x.Flags.HasFlag(CountryFlags.Capital)))
            {
                // Team doesn't have any capitals anymore
                return VictoryConditionResult.TeamDefeat;
            }

            return VictoryConditionResult.Inconclusive;
        }
    }
}