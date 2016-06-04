namespace ImperaPlus.DTO.Messages
{
    /// <summary>
    /// Information about a message folder
    /// </summary>
    public class FolderInformation
    {
        /// <summary>
        /// Folder identifier
        /// </summary>
        public MessageFolder Folder { get; set; }

        /// <summary>
        /// Number of messages
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Number of unread messages
        /// </summary>
        public int UnreadCount { get; set; }
    }
}
