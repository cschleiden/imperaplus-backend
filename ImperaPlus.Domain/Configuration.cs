namespace ImperaPlus.Domain
{
    internal static class Configuration
    {
        /// <summary>
        /// Number of game slots each player is awarded upon registration
        /// </summary>
        public const int UserInitialInternalGameSlots = 100;

        /// <summary>
        /// Default language to set for user
        /// </summary>
        public static readonly string UserDefaultLanguage = "en";
    }
}
