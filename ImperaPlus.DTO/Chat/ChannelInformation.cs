using System.Collections.Generic;

namespace ImperaPlus.DTO.Chat
{
    /// <summary>
    /// Information about a single channel
    /// </summary>
    public class ChannelInformation
    {
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the title of the channel
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the current messages in the channel
        /// </summary>
        public Message[] Messages { get; set; }

        /// <summary>
        /// Gets or sets the current users in the channel
        /// </summary>
        public User[] Users { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the channel is persisted to the database or if is a transient channel (e.g., private chat)
        /// </summary>
        public bool Persistant { get; set; }
    }
}