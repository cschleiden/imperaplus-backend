using System;

namespace ImperaPlus.DTO.Ladder
{
    /// <summary>
    /// Information about single player in a ladder
    /// </summary>
    public class LadderStanding
    {
        /// <summary>
        /// Id of user
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Name of user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Position in ladder
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Number of games played
        /// </summary>
        public int GamesPlayed { get; set; }

        /// <summary>
        /// Number of games won
        /// </summary>
        public int GamesWon { get; set; }

        /// <summary>
        /// Number of games lost
        /// </summary>
        public int GamesLost { get; set; }

        /// <summary>
        /// Rating in ladder
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// Date of last game played
        /// </summary>
        public DateTime LastGame { get; set; }
    }
}
