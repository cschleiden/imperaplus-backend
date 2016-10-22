using System;
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
            this.NumberOfTeams = 2;
            this.NumberOfPlayersPerTeam = 1;

            this.MinUnitsPerCountry = 1;
            this.NewUnitsPerTurn = 3;

            this.InitialCountryUnits = 1;

            this.MapDistribution = MapDistribution.Default;

            this.TimeoutInSeconds = 5 * 60;
            this.MaximumTimeoutsPerPlayer = 1;

            this.AttacksPerTurn = 5;
            this.MovesPerTurn = 4;

            this.MaximumNumberOfCards = 5;

            this.VictoryConditions = new SerializedCollection<VictoryConditionType>();
            this.VisibilityModifier = new SerializedCollection<VisibilityModifierType>();
        }

        public string SerializedVictoryConditions
        {
            get
            {
                return this.VictoryConditions.Serialize();
            }

            set
            {
                this.VictoryConditions = new SerializedCollection<VictoryConditionType>(value);
            }
        }

        public string SerializedVisibilityModifier
        {
            get
            {
                return this.VisibilityModifier.Serialize();
            }

            set
            {
                this.VisibilityModifier = new SerializedCollection<VisibilityModifierType>(value);
            }
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

        [NotMapped]
        public SerializedCollection<VictoryConditionType> VictoryConditions { get; private set; }

        [NotMapped]
        public SerializedCollection<VisibilityModifierType> VisibilityModifier { get; private set; }

        /// <summary>
        /// Maximum number of cards a player can hold at any given time. Defaults to 5
        /// </summary>
        public int MaximumNumberOfCards { get; set; }

        /// <summary>
        /// Maximum number of timeouts a player can have, 0 is disabled
        /// </summary>
        public int MaximumTimeoutsPerPlayer { get; set; }

        public int TimeoutInSeconds { get; set; }

        public int PlayerCount
        {
            get { return this.NumberOfTeams * this.NumberOfPlayersPerTeam; }
        }        
    }
}
