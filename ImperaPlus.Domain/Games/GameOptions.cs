using System.ComponentModel.DataAnnotations.Schema;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Utilities;

namespace ImperaPlus.Domain.Games
{
    public class GameOptions : IIdentifiableEntity
    {
        public GameOptions()
        {
            // Minimum for a game
            NumberOfTeams = 2;
            NumberOfPlayersPerTeam = 1;

            MinUnitsPerCountry = 1;
            NewUnitsPerTurn = 3;

            InitialCountryUnits = 1;

            MapDistribution = MapDistribution.Default;

            TimeoutInSeconds = 5 * 60;
            MaximumTimeoutsPerPlayer = 1;

            AttacksPerTurn = 5;
            MovesPerTurn = 4;

            MaximumNumberOfCards = 5;

            VictoryConditions = new SerializedCollection<VictoryConditionType>();
            VisibilityModifier = new SerializedCollection<VisibilityModifierType>();
        }

        public string SerializedVictoryConditions
        {
            get => VictoryConditions.Serialize();

            set => VictoryConditions = new SerializedCollection<VictoryConditionType>(value);
        }

        public string SerializedVisibilityModifier
        {
            get => VisibilityModifier.Serialize();

            set => VisibilityModifier = new SerializedCollection<VisibilityModifierType>(value);
        }

        public long Id { get; set; }

        public int NumberOfPlayersPerTeam { get; set; }

        public int NumberOfTeams { get; set; }

        /// <summary>
        /// Gets or sets a value defining how many units there need to stay in a country and 
        /// cannot be moved
        /// </summary>
        public int MinUnitsPerCountry { get; set; }

        /// <summary>
        /// Gets or sets value defining how many units a player gets each turn regardless of 
        /// conquered countries
        /// </summary>
        public int NewUnitsPerTurn { get; set; }

        /// <summary>
        /// Gets or sets a value defining how many attacks a player may perform during a single 
        /// turn
        /// </summary>
        public int AttacksPerTurn { get; set; }

        /// <summary>
        /// Gets or sets a value defining how many moves a player may perform during a single 
        /// turn
        /// </summary>
        public int MovesPerTurn { get; set; }

        /// <summary>
        /// Gets or sets a value defining how many units should be placed initially in a country
        /// TODO: CS: Move to map options (see Malibu etc.)?
        /// </summary>
        public int InitialCountryUnits { get; set; }

        /// <summary>
        /// Map distribution
        /// </summary>
        public MapDistribution MapDistribution { get; set; }

        [NotMapped] public SerializedCollection<VictoryConditionType> VictoryConditions { get; private set; }

        [NotMapped] public SerializedCollection<VisibilityModifierType> VisibilityModifier { get; private set; }

        /// <summary>
        /// Maximum number of cards a player can hold at any given time. Defaults to 5
        /// </summary>
        public int MaximumNumberOfCards { get; set; }

        /// <summary>
        /// Maximum number of timeouts a player can have, 0 is disabled
        /// </summary>
        public int MaximumTimeoutsPerPlayer { get; set; }

        public int TimeoutInSeconds { get; set; }

        public int PlayerCount => NumberOfTeams * NumberOfPlayersPerTeam;
    }
}
