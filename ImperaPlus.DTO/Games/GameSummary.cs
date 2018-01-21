using System;
using System.Collections.Generic;

namespace ImperaPlus.DTO.Games
{
    /// <summary>
    /// Summary information about a single game
    /// </summary>
    public class GameSummary
    {
        /// <summary>
        /// Identity of game
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Type of the game
        /// </summary>
        public GameType Type { get; set; }

        /// <summary>
        /// Name of this game
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether the game has a password
        /// </summary>
        public bool HasPassword { get; set; }

        /// <summary>
        /// Id of the ladder if type is Ranking
        /// </summary>
        public Guid? LadderId { get; set; }

        /// <summary>
        /// Name of the ladder if type is ranking
        /// </summary>
        public string LadderName { get; set; }

        /// <summary>
        /// Options set for this game
        /// </summary>
        public GameOptions Options { get; set; }

        /// <summary>
        /// Id of user who created the game
        /// </summary>
        public string CreatedByUserId { get; set; }

        /// <summary>
        /// Name of user who created the game
        /// </summary>
        public string CreatedByName { get; set; }

        /// <summary>
        /// Date and time this game was started (in UTC)
        /// </summary>
        public DateTime? StartedAt { get; set; }

        /// <summary>
        /// Date and time last action was performed (in UTC)
        /// </summary>
        public DateTime LastActionAt { get; set; } 

        /// <summary>
        /// Seconds until timeout of current player
        /// </summary>
        public int TimeoutSecondsLeft { get; set; }

        /// <summary>
        /// Name of the map template
        /// </summary>
        public string MapTemplate { get; set; }

        /// <summary>
        /// Current state of the game
        /// </summary>
        public GameState State { get; set; }

        /// <summary>
        /// Current player
        /// </summary>
        public PlayerSummary CurrentPlayer { get; set; }

        /// <summary>
        /// Teams in the game
        /// </summary>
        public IEnumerable<TeamSummary> Teams { get; set; }
    }
}
