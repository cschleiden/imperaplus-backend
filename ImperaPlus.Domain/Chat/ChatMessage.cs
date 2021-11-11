using System;
using ImperaPlus.Domain.Annotations;

namespace ImperaPlus.Domain.Chat
{
    public class ChatMessage : IOwnedEntity, IChangeTrackedEntity
    {
        /// <summary>
        /// DataLayer constructor
        /// </summary>
        [UsedImplicitly]
        protected ChatMessage()
        {
        }

        internal ChatMessage(Channel channel, User user, string text)
        {
            Channel = channel;

            CreatedAt = DateTime.UtcNow;
            CreatedBy = user;

            Text = text;
        }

        public long Id { get; set; }

        public string Text { get; private set; }

        public Guid ChannelId { get; set; }
        public virtual Channel Channel { get; private set; }

        public string CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }
}
