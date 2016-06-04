using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Games;
using System.Linq;

namespace ImperaPlus.Domain.VictoryConditions
{
    public class SurvivalVictoryCondition : IVictoryCondition
    {
        public VictoryConditionResult Evaluate(Player player, Games.Map map)
        {
            if (!player.Countries.Any())
            {
                return VictoryConditionResult.Defeat;
            }

            if (player.Countries.Count() == map.Countries.Count())
            {
                return VictoryConditionResult.Victory;
            }

            return VictoryConditionResult.Inconclusive;
        }
    }
}