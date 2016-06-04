namespace ImperaPlus.DTO.Chat
{
    /// <summary>
    /// Indicates to a client that a user has joined or left a channel
    /// </summary>
    public class UserChangeEvent
    {
        public string UserName { get; set; }

        public string ChannelIdentifier { get; set; }
    }
}
