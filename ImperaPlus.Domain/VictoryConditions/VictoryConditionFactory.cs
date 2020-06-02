using System;
using ImperaPlus.Domain.Enums;

namespace ImperaPlus.Domain.VictoryConditions
{
    internal static class VictoryConditionFactory
    {
        public static IVictoryCondition Create(VictoryConditionType victoryCondition) => victoryCondition switch
        {
            VictoryConditionType.Survival => new SurvivalVictoryCondition(),
            VictoryConditionType.ControlContinent => new ControlContinentVictoryCondition(),
            VictoryConditionType.Capitals => new CapitalsVictoryCondition(),
            VictoryConditionType.Rush => new RushVictoryCondition(),
            _ => throw new ArgumentOutOfRangeException("victoryCondition"),
        };
    }
}
