using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImperaPlus.DTO.Games
{
    /// <summary>
    /// Options for creating a game
    /// </summary>
    public class GameCreationOptions : GameOptions
    {
        public GameCreationOptions()
        {            
            this.MinUnitsPerCountry = 0;
            this.NewUnitsPerTurn = 3;
        }

        /// <summary>
        /// Name of the game, has to be unique
        /// </summary>
        [Required]
        [StringLength(64, MinimumLength = 3)]
        public string Name { get; set; }

        /// <summary>
        /// Value indicating whether the bot will join the game
        /// </summary>
        public bool AddBot { get; set; }

        /// <summary>
        /// Name of the map to use
        /// </summary>
        [Required]
        [StringLength(64, MinimumLength = 1)]
        public string MapTemplate { get; set; }
    }
}