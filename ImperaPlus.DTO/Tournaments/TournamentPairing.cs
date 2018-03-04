using System;

namespace ImperaPlus.DTO.Tournaments
{
    /// <summary>
    /// One pairing in tournament, represents a number of games between two teams
    /// </summary>
    public class TournamentPairing
    {
        /// <summary>
        /// Tournament pairing id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// First team in pairing
        /// </summary>
        public TournamentTeamSummary TeamA { get; set; }

        /// <summary>
        /// Second team in pairing
        /// </summary>
        public TournamentTeamSummary TeamB { get; set; }

        /// <summary>
        /// Number of games won by team A
        /// </summary>
        public int TeamAWon { get; set; }

        /// <summary>
        /// Number of games won by team B
        /// </summary>
        public int TeamBWon { get; set; }

        /// <summary>
        /// Number of games to be played in pairing
        /// </summary>
        public int NumberOfGames { get; set; }

        /// <summary>
        /// Phase of tournament this pairing belongs to
        /// </summary>
        public int Phase { get; set; }

        /// <summary>
        /// Order of pairing among others of current phase
        /// </summary>
        public int Order { get; set; }
    }
}