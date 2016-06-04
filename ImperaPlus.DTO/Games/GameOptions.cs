using System.ComponentModel.DataAnnotations;

namespace ImperaPlus.DTO.Games
{
    /// <summary>
    /// Options for games
    /// </summary>
    public class GameOptions
    {
        /// <summary>
        /// Number of players per team
        /// </summary>
        [Required]
        [Range(1, 8)]
        public int NumberOfPlayersPerTeam { get; set; }

        /// <summary>
        /// Number of teams
        /// </summary>
        [Required]
        [Range(2, 16)]
        public int NumberOfTeams { get; set; }

        /// <summary>
        /// Minimum number of units in a given country
        /// </summary>
        [Range(0, 5)]
        public int MinUnitsPerCountry { get; set; }

        /// <summary>
        /// Number of units each player receives each turn at a minium (independent of the number of countries
        /// he owns)
        /// </summary>
        [Range(3, 10)]
        public int NewUnitsPerTurn { get; set; }

        /// <summary>
        /// Number of attacks per player per turn
        /// </summary>
        [Required]
        [Range(1, 100)]
        public int AttacksPerTurn { get; set; }

        /// <summary>
        /// Number of moves per player per turn
        /// </summary>
        [Required]
        [Range(1, 100)]
        public int MovesPerTurn { get; set; }

        /// <summary>
        /// Determines the initial unit count per country
        /// </summary>
        /// <remarks>
        /// Can be overriden in map distributions (e.g., malibu)
        /// </remarks>
        [Range(0, 5)]
        public int InitialCountryUnits { get; set; }

        /// <summary>
        /// Map distribution to be used when creating map
        /// </summary>
        [Required]
        public MapDistribution MapDistribution { get; set; }

        /// <summary>
        /// Timeout in seconds
        /// </summary>
        [Required]
        [Range(60 * 3, 60 * 60 * 24 * 5)]
        public int TimeoutInSeconds { get; set; }

        /// <summary>
        /// Allowed timeouts per player
        /// </summary>
        public int MaximumTimeoutsPerPlayer { get; set; }

        /// <summary>
        /// Maximum number of cards per player. A value of 0 disables bonus cards completely
        /// </summary>
        [Required]
        [Range(0, 10)]
        public int MaximumNumberOfCards { get; set; }

        /// <summary>
        /// List of victory conditions.
        /// </summary>
        [Required]
        [MinLength(1)]
        public VictoryConditionType[] VictoryConditions { get; set; }

        /// <summary>
        /// List of visibility modifiers.
        /// </summary>
        [Required]
        [MinLength(1)]
        public VisibilityModifierType[] VisibilityModifier { get; set; }
    }
}
