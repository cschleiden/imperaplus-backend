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
        private ChatMessage()
        {
        }

        internal ChatMessage(Channel channel, User user, string text)
        {
            this.Channel = channel;

            this.CreatedAt = DateTime.UtcNow;
            this.CreatedBy = user;

            this.Text = text;
        }

        public long Id { get; set; }

        public string Text { get; private set; }
        
        public Guid ChannelId { get; set; }
        public virtual Channel Channel { get; private set; }
        
        public string CreatedById { get; set; }
        public User CreatedBy { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime LastModifiedAt { get; set; }
    }
}