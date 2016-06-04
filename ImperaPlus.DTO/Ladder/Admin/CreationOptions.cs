using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.DTO.Ladder.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public class CreationOptions
    {
        /// <summary>
        /// Name of new ladder
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Number of teams per game
        /// </summary>
        [Required]
        public int NumberOfTeams { get; set; }

        /// <summary>
        /// Number of players per game
        /// </summary>
        [Required]
        public int NumberOfPlayers { get; set; }
    }
}
