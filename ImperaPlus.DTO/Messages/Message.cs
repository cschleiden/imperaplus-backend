using System;
using ImperaPlus.DTO.Users;

namespace ImperaPlus.DTO.Messages
{
    public class Message : SendMessage
    {
        public Guid Id { get; set; }

        public UserReference From { get; set; }

        public MessageFolder Folder { get; set; }

        public DateTime SentAt { get; set; }

        public bool IsRead { get; set; }
    }
}