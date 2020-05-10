using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Utilities;
using System.Linq;

namespace ImperaPlus.Domain.VictoryConditions
{
    public class CapitalsVictoryCondition : IVictoryCondition
    {
        public void Initialize(Games.Game game, IRandomGen random)
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

        public VictoryConditionResult Evaluate(Player player, Games.Map map)
        {
            if (!player.Countries.Any(x => x.Flags.HasFlag(CountryFlags.Capital)))
            {
                return VictoryConditionResult.Defeat;
            }

            return VictoryConditionResult.Inconclusive;
        }
    }
}