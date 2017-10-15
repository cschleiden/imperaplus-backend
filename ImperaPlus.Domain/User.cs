using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Alliances;
using ImperaPlus.Domain.Games;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ImperaPlus.Domain
{
    public class User : IdentityUser
    {
        public User()
        {
            this.CreatedGames = new HashSet<Game>();

            this.GameSlots = Configuration.UserInitialInternalGameSlots;

            this.Language = Configuration.UserDefaultLanguage;

            this.Standings = new HashSet<Ladders.LadderStanding>();
        }

        public Guid? AllianceId { get; set; }
        public virtual Alliance Alliance { get; set; }

        public bool IsAllianceAdmin { get; set; }

        public virtual ICollection<Game> CreatedGames { get; private set; }

        public int GameSlots { get; set; }

        public string Language { get; set; }

        public virtual ICollection<Ladders.LadderStanding> Standings { get; private set; }

        public bool IsDeleted { get; set; }

        public DateTime LastLogin { get; set; }

        /// <summary>
        /// Hash of Impera V1 password for migration
        /// </summary>
        public string LegacyPasswordHash { get; set; }

        public bool CanCreateGame
        {
            get
            {
                // Might want to move this to role
                if (this.GameSlots == int.MaxValue)
                {
                    return true;
                }

                return this.CreatedGames.Sum(x => x.RequiredSlots) < this.GameSlots;
            }
        }

        public bool IsInRole(IdentityRole role)
        {
            return this.Roles.Any(x => x.RoleId == role.Id);
        }
    }
}
