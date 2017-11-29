using System.ComponentModel.DataAnnotations;

namespace ImperaPlus.DTO.Alliances
{
    public class AllianceCreationOptions
    {
        [Required]
        [StringLength(64, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string Description { get; set; }
    }
}
