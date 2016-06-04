namespace ImperaPlus.Domain.Enums
{
    public enum MapDistribution
    {
        /// <summary>
        /// Default map distribution
        /// </summary>
        Default = 0,

        /// <summary>
        /// Malibu map distribution, one country per player
        /// </summary>
        Malibu,

        /// <summary>
        /// TODO: CS Maybe teams start right next to each other?
        /// </summary>
        TeamCluster
    }
}