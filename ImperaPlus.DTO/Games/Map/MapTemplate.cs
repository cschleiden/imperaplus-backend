namespace ImperaPlus.DTO.Games.Map
{

    public class MapTemplate
    {
        /// <summary>
        /// Name of the map
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the image file to retrieve from the CDN
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// List of countries in this map
        /// </summary>
        public CountryTemplate[] Countries { get; set; }

        /// <summary>
        /// List of connections between countries
        /// </summary>
        public Connection[] Connections { get; set; }

        /// <summary>
        /// List of continents in this map
        /// </summary>
        public Continent[] Continents { get; set; } 
    }
}
