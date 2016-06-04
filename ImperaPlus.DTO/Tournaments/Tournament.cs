using System;
using ImperaPlus.DTO.Games;

namespace ImperaPlus.DTO.Tournaments
{
    /// <summary>
    /// Full tournament
    /// </summary>
    public class Tournament : TournamentSummary
    {
        /// <summary>
        /// Teams in tournament
        /// </summary>
        public TournamentTeam[] Teams { get; set; }

        /// <summary>
        /// Groups in tournament, if tournament contains group phase
        /// </summary>
        public TournamentGroup[] Groups { get; set; }

        /// <summary>
        /// Pairings for knockout phase
        /// </summary>
        public TournamentPairing[] Pairings { get; set; }

        /// <summary>
        /// Map templates that can be played in this tournament
        /// </summary>
        public string[] MapTemplates { get; set; }

        /// <summary>
        /// Winner of tournament (if determined)
        /// </summary>
        public TournamentTeam Winner { get; set; }

        /// <summary>
        /// Knockout phase
        /// </summary>
        public int Phase { get; set; }
    }
}
