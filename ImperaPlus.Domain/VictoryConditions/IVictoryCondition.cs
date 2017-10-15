using ImperaPlus.Domain.Games;

namespace ImperaPlus.Domain.VictoryConditions
{
    public enum VictoryConditionResult
    {
        Inconclusive = 0,

        Victory = 1,

        Defeat = 2
    }

    public interface IVictoryCondition
    {
        VictoryConditionResult Evaluate(Player player, Games.Map map);
    }
}
