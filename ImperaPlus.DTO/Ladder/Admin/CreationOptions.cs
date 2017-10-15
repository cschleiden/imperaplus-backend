using System.ComponentModel.DataAnnotations;

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
