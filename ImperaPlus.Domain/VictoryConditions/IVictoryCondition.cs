using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Domain.VictoryConditions
{
    public enum VictoryConditionResult
    {
        Inconclusive = 0,

        Victory = 1,

        Defeat = 2,

        TeamDefeat = 3
    }

    public interface IVictoryCondition
    {
        void Initialize(Games.Game game, IRandomGen random);

        VictoryConditionResult Evaluate(Player player, Games.Map map);
    }
}
