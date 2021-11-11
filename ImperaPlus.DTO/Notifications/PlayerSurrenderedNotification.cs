namespace ImperaPlus.DTO.Notifications
{
    public class PlayerSurrenderedNotification : Notification
    {
        public PlayerSurrenderedNotification()
            : base(NotificationType.PlayerSurrender)
        {
        }

        /// <summary>
        /// Id of game
        /// </summary>
        public long GameId { get; set; }

        /// <summary>
        /// Player who has surrendered
        /// </summary>
        public Games.PlayerSummary Player { get; set; }
    }
}
