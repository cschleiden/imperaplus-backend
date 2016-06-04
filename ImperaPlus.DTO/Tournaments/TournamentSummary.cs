using System;
using System.ComponentModel.DataAnnotations;
using ImperaPlus.DTO.Games;

namespace ImperaPlus.DTO.Tournaments
{
    /// <summary>
    /// Summary of tournament
    /// </summary>
    public class TournamentSummary
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of tournament
        /// </summary>
        [Required]
        [MinLength(3)]
        [StringLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// State of tournament
        /// </summary>
        public TournamentState State { get; set; }

        /// <summary>
        /// Options for games in tournament
        /// </summary>
        public GameOptions Options { get; set; }

        /// <summary>
        /// Number of teams allowed in tournament. Has to be power of 2
        /// </summary>
        public int NumberOfTeams { get; set; }

        /// <summary>
        /// How many group games should be played, has to be odd
        /// </summary>
        public int NumberOfGroupGames { get; set; }

        /// <summary>
        /// How many group games should be played, has to be odd
        /// </summary>
        public int NumberOfKnockoutGames { get; set; }

        /// <summary>
        /// How many group games should be played, has to be odd
        /// </summary>
        public int NumberOfFinalGames { get; set; }

        /// <summary>
        /// Start of registration period
        /// </summary>
        public DateTime StartOfRegistration { get; set; }

        /// <summary>
        /// Start of tournament
        /// </summary>
        public DateTime StartOfTournament { get; set; }

        /// <summary>
        /// End of tournament
        /// </summary>
        public DateTime EndOfTournament { get; set; }

        /// <summary>
        /// Completion of tournament in percent
        /// </summary>
        public int Completion { get; set; }
    }
}
