namespace ImperaPlus.DTO.Notifications
{
    /// <summary>
    /// Initial notification state delivered to clients
    /// </summary>
    public class NotificationSummary
    {
        /// <summary>
        /// Number of games where it's player's turn
        /// </summary>
        public int NumberOfGames { get; set; }

        /// <summary>
        /// Number of unread messages
        /// </summary>
        public int NumberOfMessages { get; set; }
    }
}
