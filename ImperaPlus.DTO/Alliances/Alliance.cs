using System.Collections.Generic;
using ImperaPlus.DTO.Users;

namespace ImperaPlus.DTO.Alliances
{
    public class Alliance : AllianceSummary
    {
        /// <summary>
        /// All members of the alliance
        /// </summary>
        public IEnumerable<UserReference> Members { get; set; }
    }
}
