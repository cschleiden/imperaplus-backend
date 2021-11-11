using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ImperaPlus.Domain.Alliances;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Games;

namespace ImperaPlus.Domain.Chat
{
    public class Channel : IIdentifiableEntity<Guid>
    {
        public Channel()
        {
            Id = Guid.NewGuid();
            Messages = new List<ChatMessage>();
        }

        public Channel(string name, ChannelType channelType, long? gameId = null)
            : this()
        {
            Name = name;
            Type = channelType;

            if (gameId.HasValue)
            {
                GameId = gameId.Value;
            }
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ChannelType Type { get; set; }

        public long? GameId { get; set; }
        public virtual Game Game { get; set; }

        public virtual ICollection<ChatMessage> Messages { get; private set; }

        public IEnumerable<ChatMessage> RecentMessages { get; set; }

        public ChatMessage CreateMessage(User user, string message)
        {
            return new ChatMessage(this, user, message);
        }
    }
}
