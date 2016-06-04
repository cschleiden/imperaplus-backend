namespace ImperaPlus.DTO.Games
{
    /// <summary>
    /// Extended information about a single player
    /// </summary>
    public class Player : PlayerSummary
    {        
        /// <summary>
        /// Comma separated list of bonus cards
        /// </summary>
        public BonusCard[] Cards { get; set; }

        /// <summary>
        /// Indicates whether the user has placed the initial units
        /// </summary>
        public bool PlacedInitialUnits { get; set; }

        /// <summary>
        /// Overall number of units of this player
        /// </summary>
        public int NumberOfUnits { get; set; }

        /// <summary>
        /// Overall number of countries of this player
        /// </summary>
        public int NumberOfCountries { get; set; }
    }
}