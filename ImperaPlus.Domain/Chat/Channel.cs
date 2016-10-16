using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.Domain.Chat
{
    public class Channel : IIdentifiableEntity<Guid>, IOwnedEntity
    {
        public Channel()
        {
            this.Id = Guid.NewGuid();

            this.Messages = new List<ChatMessage>();
        }

        [NotMapped]
        public IChatMessageRepository MessageRepository { get; set; }

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

        public IEnumerable<ChatMessage> RecentMessages
        {
            get
            {
                return this.MessageRepository
                    .Query()
                    .Where(x => x.ChannelId == this.Id)
                    .Include(x => x.CreatedBy)
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(20)
                    .OrderBy(x => x.CreatedAt);
            }
        }

        public ChatMessage CreateMessage(User user, string message)
        {
            return new ChatMessage(this, user, message);
        }
    }
}
