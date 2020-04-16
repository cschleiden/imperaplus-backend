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
            this.Id = Guid.NewGuid();

            this.Subject = subject;
            this.Text = text;

            this.Owner = owner;
            this.OwnerId = owner.Id;

            this.From = from;
            this.FromId = from.Id;

            this.Recipient = to;
            this.RecipientId = to.Id;

            this.Folder = folder;
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
        public User Owner { get; internal set; }

        public string FromId { get; internal set; }
        public User From { get; internal set; }

        public string RecipientId { get; internal set; }
        public User Recipient { get; internal set; }
    }
}
