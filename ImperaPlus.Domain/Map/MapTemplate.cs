using System.Collections.Generic;
using System.Linq;

namespace ImperaPlus.Domain.Map
{
    public class MapTemplate
    {
        private ILookup<string, string> connectionDict;

        public MapTemplate(string name)
        {
            Name = name;

            Countries = new HashSet<CountryTemplate>();
            Continents = new HashSet<Continent>();
            Connections = new HashSet<Connection>();
        }

        public string Name { get; set; }

        public string Image { get; set; }

        public ICollection<CountryTemplate> Countries { get; set; }

        public ICollection<Continent> Continents { get; set; }

        public ICollection<Connection> Connections { get; set; }

        /// <summary>
        /// Returns a value indicating whether the given countries are connected
        /// </summary>
        /// <param name="origin">Origin Country Identifier</param>
        /// <param name="destination">Destination Country Identifier</param>
        /// <returns>Value indicating whether the countries are connected</returns>
        public bool AreConnected(string origin, string destination)
        {
            if (connectionDict == null)
            {
                lock (this)
                {
                    if (connectionDict == null)
                    {
                        connectionDict = Connections.ToLookup(x => x.Origin, x => x.Destination);
                    }
                }
            }

            return connectionDict[origin].Contains(destination);
        }

        /// <summary>
        /// Returns a list of connected countries for a given origin
        /// </summary>
        /// <param name="origin">Origin country identifier</param>
        /// <returns></returns>
        public IEnumerable<string> GetConnectedCountries(string origin)
        {
            if (connectionDict == null)
            {
                lock (this)
                {
                    if (connectionDict == null)
                    {
                        connectionDict = Connections.ToLookup(x => x.Origin, x => x.Destination);
                    }
                }
            }

            return connectionDict[origin];
        }

        /// <summary>
        /// Calculate the continent bonus for a list of countries
        /// </summary>
        /// <param name="countryIdentifiers">List of countries</param>
        public int CalculateBonus(IEnumerable<string> countryIdentifiers)
        {
            return
                OccupiedContinents(countryIdentifiers)
                    .Sum(continent => continent.Bonus);
        }

        /// <summary>
        /// Returns a list of continents which are completely covered by the given country identifiers
        /// </summary>
        /// <param name="countryIdentifiers">List of countries</param>
        public IEnumerable<Continent> OccupiedContinents(IEnumerable<string> countryIdentifiers)
        {
            return
                Continents
                    .Where(continent => continent.Countries.All(x => countryIdentifiers.Contains(x.Identifier)));
        }
    }
}
