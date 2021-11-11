using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Alliances;
using ImperaPlus.Domain.Games;
using Microsoft.AspNetCore.Identity;

namespace ImperaPlus.Domain
{
    public class User : IdentityUser, IChangeTrackedEntity
    {
        public User()
        {
            CreatedGames = new HashSet<Game>();

            GameSlots = Configuration.UserInitialInternalGameSlots;

            Language = Configuration.UserDefaultLanguage;

            Standings = new HashSet<Ladders.LadderStanding>();
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

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        /// <summary>
        /// Navigation property for this users login accounts.
        /// </summary>
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; } = new List<IdentityUserLogin<string>>();

        /// <summary>
        /// Navigation property for this users login accounts.
        /// </summary>
        public virtual ICollection<IdentityUserRole<string>> Roles { get; } = new List<IdentityUserRole<string>>();

        /// <summary>
        /// Hash of Impera V1 password for migration
        /// </summary>
        public string LegacyPasswordHash { get; set; }

        public bool CanCreateGame
        {
            get
            {
                // Might want to move this to role
                if (GameSlots == int.MaxValue)
                {
                    return true;
                }

                return CreatedGames.Sum(x => x.RequiredSlots) < GameSlots;
            }
        }

        public bool IsInRole(IdentityRole role)
        {
            return Roles.Any(x => x.RoleId == role.Id);
        }
    }
}
