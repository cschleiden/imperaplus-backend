using System.ComponentModel.DataAnnotations;

namespace ImperaPlus.DTO.Games.Play
{
    public class PlaceUnitsOptions
    {
        /// <summary>
        /// Identifier of country to place to
        /// </summary>
        [Required]
        [StringLength(64, MinimumLength = 1)]
        public string CountryIdentifier { get; set; }

        /// <summary>
        /// Number of units to place
        /// </summary>
        [Range(1, 64000)]
        public int NumberOfUnits { get; set; }
    }
}
