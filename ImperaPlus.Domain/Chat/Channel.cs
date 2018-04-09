using System;
using System.Collections.Generic;
using ImperaPlus.Domain.Alliances;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Games;

namespace ImperaPlus.Domain.Chat
{
    public class Channel : IIdentifiableEntity<Guid>
    {
        public Channel()
        {
            this.Id = Guid.NewGuid();
            this.Messages = new List<ChatMessage>();
        }

        public Channel(string name, ChannelType channelType, long? gameId = null, Guid? allianceId = null)
            : this()
        {
            this.Name = name;
            this.Type = channelType;

            if (gameId.HasValue)
            {
                this.GameId = gameId.Value;
            }

            if (allianceId.HasValue)
            {
                this.AllianceId = allianceId.Value;
            }
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public ChannelType Type { get; set; }

        public long? GameId { get; set; }
        public virtual Game Game { get; set; }

        public Guid? AllianceId { get; set; }
        public virtual Alliance Alliance { get; set; }

        public virtual ICollection<ChatMessage> Messages { get; private set; }

        public IEnumerable<ChatMessage> RecentMessages { get; set; }

        public ChatMessage CreateMessage(User user, string message)
        {
            return new ChatMessage(this, user, message);
        }
    }
}
