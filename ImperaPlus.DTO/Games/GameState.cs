namespace ImperaPlus.DTO.Games
{
    /// <summary>
    /// Current state of the game
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// Default value
        /// </summary>
        None = 0,

        /// <summary>
        /// Game is open and players can join
        /// </summary>
        Open,

        /// <summary>
        /// Game is active and ongoing
        /// </summary>
        Active, 

        /// <summary>
        /// Game has ended and playing is done
        /// </summary>
        Ended
    }
}