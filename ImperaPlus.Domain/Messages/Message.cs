using System;
using System.ComponentModel.DataAnnotations.Schema;
using ImperaPlus.Domain.Annotations;

namespace ImperaPlus.Domain.Messages
{
    public class Message : IChangeTrackedEntity
    {
        [UsedImplicitly]
        protected Message()
        {
        }

        public Message(User owner, User from, User to, string subject, string text, MessageFolder folder)
        {
            Id = Guid.NewGuid();

            Subject = subject;
            Text = text;

            Owner = owner;
            OwnerId = owner.Id;

            From = from;
            FromId = from.Id;

            Recipient = to;
            RecipientId = to.Id;

            Folder = folder;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public MessageFolder Folder { get; set; }

        /// <summary>
        /// Indicate whether message was read by owner
        /// </summary>
        public bool IsRead { get; set; }

        public string Subject { get; internal set; }

        public string Text { get; internal set; }

        /// <summary>
        /// Message shows up in owner's folders
        /// </summary>
        public string OwnerId { get; internal set; }

        public virtual User Owner { get; internal set; }

        public string FromId { get; internal set; }
        public virtual User From { get; internal set; }

        public string RecipientId { get; internal set; }
        public virtual User Recipient { get; internal set; }
    }
}
