using System;
using System.ComponentModel.DataAnnotations;
using ImperaPlus.DTO.Games;

namespace ImperaPlus.DTO.Ladder
{
    /// <summary>
    /// Summary of ladder
    /// </summary>
    public class LadderSummary
    {
        /// <summary>
        /// Unique Ladder identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of ladder
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Game creation options
        /// </summary>
        public GameOptions Options { get; set; }

        /// <summary>
        /// Standing in this ladder for the current user, optional if no game has been played
        /// </summary>
        public LadderStanding Standing { get; set; }

        /// <summary>
        /// Indicates whether the current user is already in queue for this league
        /// </summary>
        public bool IsQueued { get; set; }

        /// <summary>
        /// Indicates how many players are currently waiting in queue
        /// </summary>
        public int QueueCount { get; set; }

        /// <summary>
        /// Pool of map templates
        /// </summary>
        [Required]
        public string[] MapTemplates { get; set; }
    }
}
