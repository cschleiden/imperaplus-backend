namespace ImperaPlus.DTO.Notifications
{
    /// <summary>
    /// Notification when the user becomes the current player in a game
    /// </summary>
    public class PlayersTurnNotification : Notification
    {
        public PlayersTurnNotification() : base(NotificationType.PlayerTurn)
        {
        }

        /// <summary>
        /// Id of game
        /// </summary>
        public long GameId { get; set; }
    }
}