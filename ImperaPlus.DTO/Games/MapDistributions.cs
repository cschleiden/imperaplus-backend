namespace ImperaPlus.DTO.Games
{
    /// <summary>
    /// Types of map distributions when a game is created
    /// </summary>
    public enum MapDistribution
    {
        /// <summary>
        /// Default map distribution
        /// </summary>
        Default,

        /// <summary>
        /// Malibu map distribution, one country per player
        /// </summary>
        Malibu,

        /// <summary>
        /// Malibu map distribution, three countries per player
        /// </summary>
        Malibu3

        /// <summary>
        /// TODO: CS Maybe teams start right next to each other?
        /// </summary>
        // TeamCluster
    }
}
