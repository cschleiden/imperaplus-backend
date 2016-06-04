using ImperaPlus.Domain.Games;

namespace ImperaPlus.Domain.VictoryConditions
{
    public class ControlContinentVictoryCondition : IVictoryCondition
    {        

        public VictoryConditionResult Evaluate(Player player, Games.Map map)
        {
            return VictoryConditionResult.Inconclusive;
        }
    }
}