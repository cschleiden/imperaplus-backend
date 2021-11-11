namespace ImperaPlus.DTO.Games.Map
{
    public class CountryTemplate
    {
        /// <summary>
        /// Identifier to reference country in other actions
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Name of the country
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// X coordinate
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public int Y { get; set; }
    }
}
