using System;

namespace ImperaPlus.DTO.Games
{
    public class Team
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
        public Player[] Players { get; set; }
    }
}
