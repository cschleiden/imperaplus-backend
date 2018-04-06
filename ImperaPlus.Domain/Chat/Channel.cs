using System;
using System.Collections.Generic;
using ImperaPlus.Domain.Alliances;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Games;

namespace ImperaPlus.Domain.Chat
{
    public class Channel : IIdentifiableEntity<Guid>, IOwnedEntity
    {
        public Channel()
        {
            this.Id = Guid.NewGuid();
            this.Messages = new List<ChatMessage>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public ChannelType Type { get; set; }

        public string CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public long? GameId { get; set; }
        public virtual Game Game { get; set; }

        public long? AllianceId { get; set; }
        public virtual Alliance Alliance { get; set; }

        public virtual ICollection<ChatMessage> Messages { get; private set; }

        public IEnumerable<ChatMessage> RecentMessages { get; set; }

        public ChatMessage CreateMessage(User user, string message)
        {
            return new ChatMessage(this, user, message);
        }
    }
}
