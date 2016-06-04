using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.DTO.Games.Map
{
    public class Map
    {
        /// <summary>
        /// State of countries
        /// </summary>
        public Country[] Countries { get; set; }
    }

    /// <summary>
    /// State of a country
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Id of the owner
        /// </summary>
        public Guid PlayerId { get; set; }

        /// <summary>
        /// Id of the owning team
        /// </summary>
        public Guid TeamId { get; set; }

        /// <summary>
        /// Number of units in the country
        /// </summary>
        public int Units { get; set; }
    }
}
