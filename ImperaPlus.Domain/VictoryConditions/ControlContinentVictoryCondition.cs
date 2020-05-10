using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Domain.VictoryConditions
{
    public class ControlContinentVictoryCondition : IVictoryCondition
    {
        public void Initialize(Games.Game game, IRandomGen random)
        {
        }

        public VictoryConditionResult Evaluate(Player player, Games.Map map)
        {
            return VictoryConditionResult.Inconclusive;
        }
    }
}