using System.Collections.Generic;

namespace ImperaPlus.DTO.Games.Map
{
    /// <summary>
    /// Represents a single continent on a map
    /// </summary>
    public class Continent
    {
        /// <summary>
        /// Unique identifier of continent
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Name of continent
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Bonus units awarded by controlling this continent
        /// </summary>
        public int Bonus { get; set; }

        /// <summary>
        /// List of identifiers representing the countries which make up this continent
        /// </summary>
        public string[] Countries { get; set; }
    }
}