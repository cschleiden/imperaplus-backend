using System;
using System.Collections.Generic;

namespace ImperaPlus.DTO.Games
{
    /// <summary>
    /// Team in game
    /// </summary>
    public class TeamSummary
    {
        /// <summary>
        /// Global identifier of team
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Order of play for this game
        /// </summary>
        public int PlayOrder { get; set; }

        /// <summary>
        /// Players in team
        /// </summary>
        public IEnumerable<PlayerSummary> Players { get; set; }
    }
}
