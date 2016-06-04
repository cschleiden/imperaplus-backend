using System;
using ImperaPlus.DTO.Users;

namespace ImperaPlus.DTO.Games.Chat
{
    /// <summary>
    /// Single message posted in in-game chat
    /// </summary>
    public class GameChatMessage
    {
        /// <summary>
        /// Unique message identifier
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Id of game message belongs to 
        /// </summary>
        public long GameId { get; set; }

        /// <summary>
        /// Id of user posting message
        /// </summary>
        public UserReference User { get; set; }

        /// <summary>
        /// Id of the team this messages was posted to. If set to null, then it's a public message
        /// </summary>
        public Guid TeamId { get; set; }

        /// <summary>
        /// Date and time when the messages was posted, in UTC
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Text of the message
        /// </summary>
        public string Text { get; set; }
    }
}
