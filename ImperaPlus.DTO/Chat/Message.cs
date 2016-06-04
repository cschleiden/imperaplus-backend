using System;

namespace ImperaPlus.DTO.Chat
{
    public class Message
    {
        public DateTime DateTime { get; set; }

        public string UserName { get; set; }

        public string Text { get; set; }

        public string ChannelIdentifier { get; set; }
    }
}