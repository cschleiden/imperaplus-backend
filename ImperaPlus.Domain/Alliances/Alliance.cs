using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ImperaPlus.Domain.Chat;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Utilities;

namespace ImperaPlus.Domain.Alliances
{
    public class Alliance
    {
        protected Alliance()
        {
            this.Id = Guid.NewGuid();
            this.Members = new HashSet<User>();
        }

        public Alliance(string name, string description)
            : this()
        {
            this.Name = name;
            this.Description = description;
        }

        public Guid Id { get; protected set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [NotMapped]
        public IEnumerable<User> Administrators
        {
            get
            {
                return this.Members.Where(x => x.IsAllianceAdmin);
            }
        }

        public virtual ICollection<User> Members { get; private set; }

        public Guid ChannelId { get; set; }
        public virtual Channel Channel { get; set; }

        public void AddMember(User user)
        {
            bool isMemberAlready = this.Members.Any(m => m.Id == user.Id);
            if (isMemberAlready)
            {
                throw new DomainException(ErrorCode.UserAlreadyInAlliance, "User {0} is already a member of alliance {1}", user.Id, this.Id);
            }

            this.Members.Add(user);
        }

        /// <summary>
        /// Returns a value indicating whether the given user is an alliance admin
        /// </summary>
        public bool IsAdmin(User currentAdmin)
        {
            Require.NotNull(currentAdmin, nameof(currentAdmin));

            return this.Administrators.Any(x => x.Id == currentAdmin.Id);
        }
    }
}