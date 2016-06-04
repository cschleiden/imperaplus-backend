namespace ImperaPlus.Backend.Areas.Admin.Helpers
{
    public class GameOptionsHelper
    {
        public static void SetDefaultGameOptions(DTO.Games.GameOptions options)
        {
            options.AttacksPerTurn = 3;
            options.MovesPerTurn = 3;

            options.InitialCountryUnits = 1;
            options.MinUnitsPerCountry = 1;
            options.NewUnitsPerTurn = 3;

            options.MaximumNumberOfCards = 5;
            options.MaximumTimeoutsPerPlayer = 2;

            options.MapDistribution = DTO.Games.MapDistribution.Default;
            options.VictoryConditions = new[] { DTO.Games.VictoryConditionType.Survival };
            options.VisibilityModifier = new[] { DTO.Games.VisibilityModifierType.None };

            options.TimeoutInSeconds = 86400;

            options.NumberOfPlayersPerTeam = 1;
        }
    }
}