using System.ComponentModel.DataAnnotations;

namespace ImperaPlus.DTO.Games.Play
{
    /// <summary>
    /// 
    /// </summary>
    public class MoveOptions
    {
        /// <summary>
        /// Identifier for origin country
        /// </summary>
        [Required]
        [StringLength(64, MinimumLength = 1)]
        public string OriginCountryIdentifier { get; set; }

        /// <summary>
        /// Identifier for destination country
        /// </summary>
        [Required]
        [StringLength(64, MinimumLength = 1)]
        public string DestinationCountryIdentifier { get; set; }

        /// <summary>
        /// Number of units to move from origin to destination
        /// </summary>
        [Range(1, 64000)]
        public int NumberOfUnits { get; set; }
    }
}