namespace ImperaPlus.Domain.Enums
{
    public enum GameState
    {
        /// <summary>
        /// Default
        /// </summary>
        None = 0,

        /// <summary>
        /// Game has been created, players can join
        /// </summary>
        Open,

        /// <summary>
        /// Game is active and ongoing
        /// </summary>
        Active,

        /// <summary>
        /// Game has ended
        /// </summary>
        Ended
    }
}
