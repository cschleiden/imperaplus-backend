using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ImperaPlus.Domain.Chat;

namespace ImperaPlus.Domain
{
    public class Alliance
    {
        public Alliance()
        {
            this.Id = Guid.NewGuid();

            this.Members = new HashSet<User>();
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
    }
}