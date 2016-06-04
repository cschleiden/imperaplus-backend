namespace ImperaPlus.DTO.Ladder.Admin
{
    /// <summary>
    /// Update the map templates for a ladder
    /// </summary>
    public class MapTemplateUpdate
    {
        /// <summary>
        /// List of map template names to set for ladder
        /// </summary>        
        public string[] MapTemplateNames { get; set; }
    }
}
