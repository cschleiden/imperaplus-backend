using ImperaPlus.DTO.Games.Chat;

namespace ImperaPlus.DTO.Notifications
{
    public class GameChatMessageNotification : Notification
    {
        public GameChatMessageNotification() : base(NotificationType.GameChatMessage)
        {
        }

        /// <summary>
        /// Id of game
        /// </summary>
        public long GameId { get; set; }

        /// <summary>
        /// Message received
        /// </summary>
        public GameChatMessage Message { get; set; }
    }
}
