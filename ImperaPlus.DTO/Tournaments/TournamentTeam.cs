using System.Collections.Generic;
using ImperaPlus.DTO.Users;

namespace ImperaPlus.DTO.Tournaments
{
    /// <summary>
    /// Team in tournament
    /// </summary>
    public class TournamentTeam : TournamentTeamSummary
    {
        /// <summary>
        /// Players in team
        /// </summary>
        public IEnumerable<UserReference> Participants { get; set; }
    }
}
