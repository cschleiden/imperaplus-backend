using System;

namespace ImperaPlus.DTO.Notifications
{
    public class TurnEndedNotification : Notification
    {
        public TurnEndedNotification(): base (NotificationType.EndTurn)
        {
        }

        /// <summary>
        /// Id of game
        /// </summary>
        public long GameId { get; set; }

        /// <summary>
        /// Id of current player
        /// </summary>
        public Guid NewPlayerId { get; set; }
    }
}