namespace ImperaPlus.DTO.Notifications
{
    /// <summary>
    /// Type of notification
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// It's player's turn in a game
        /// </summary>
        PlayerTurn,

        /// <summary>
        /// Turn has ended in game
        /// </summary>
        EndTurn,

        /// <summary>
        /// Player has surrendered in a game
        /// </summary>
        PlayerSurrender,

        /// <summary>
        /// New message in current game
        /// </summary>
        GameChatMessage,

        /// <summary>
        /// New message for player
        /// </summary>
        NewMessage
    }
}
