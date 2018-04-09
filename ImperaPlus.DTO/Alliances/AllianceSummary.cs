using System;
using System.Collections.Generic;
using ImperaPlus.DTO.Users;

namespace ImperaPlus.DTO.Alliances
{
    public class AllianceSummary
    {
        /// <summary>
        /// Alliance id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Alliance name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Alliance description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Number of members in alliance
        /// </summary>
        public int NumberOfMembers { get; set; }

        /// <summary>
        /// List of admins
        /// </summary>
        public IEnumerable<UserReference> Admins { get; set; }
    }
}
