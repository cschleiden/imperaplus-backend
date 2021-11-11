using System;

namespace ImperaPlus.DTO.Games
{
    /// <summary>
    /// Summary of a player in a game
    /// </summary>
    public class PlayerSummary
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User identifier
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Current state of the player
        /// </summary>
        public PlayerState State { get; set; }

        /// <summary>
        /// State of the player after the game has ended
        /// </summary>
        public PlayerOutcome Outcome { get; set; }

        /// <summary>
        /// Id of the player's team
        /// </summary>
        public Guid TeamId { get; set; }

        /// <summary>
        /// Playorder 
        /// </summary>
        public int PlayOrder { get; set; }

        /// <summary>
        /// Number of timeouts the player has had so far
        /// </summary>
        public int Timeouts { get; set; }
    }
}
