using System.Collections.Generic;

namespace ImperaPlus.DTO.Chat
{
    /// <summary>
    /// Returned when a user joins the chat with information about channels and messages
    /// </summary>
    public class ChatInformation
    {
        /// <summary>
        /// List of active channels
        /// </summary>
        public ChannelInformation[] Channels { get; set; }
    }
}