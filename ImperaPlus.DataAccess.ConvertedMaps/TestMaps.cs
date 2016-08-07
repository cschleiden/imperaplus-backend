using System;
using ImperaPlus.Domain.Map;

namespace ImperaPlus.DataAccess.ConvertedMaps
{
    public static partial class Maps    
    {
        public static MapTemplate TestMap()
        {
            /*

            A - B
            | / |
            C - D
            */

            var mapTemplate = new MapTemplate("TestMap")
            {
                Image = "testmap.jpg"
            };

            var countryA = new CountryTemplate("A", "A") { X = 0, Y = 0 };
            mapTemplate.Countries.Add(countryA);
            var countryB = new CountryTemplate("B", "B") { X = 100, Y = 0 };
            mapTemplate.Countries.Add(countryB);
            var countryC = new CountryTemplate("C", "C") { X = 0, Y = 100 };
            mapTemplate.Countries.Add(countryC);
            var countryD = new CountryTemplate("D", "D") { X = 100, Y = 100 };
            mapTemplate.Countries.Add(countryD);

            var continent1 = new Continent("1", 2);
            continent1.Countries.Add(countryA);
            continent1.Countries.Add(countryB);
            mapTemplate.Continents.Add(continent1);
            var continent2 = new Continent("2", 2);
            continent1.Countries.Add(countryC);
            continent1.Countries.Add(countryD);
            mapTemplate.Continents.Add(continent2);

            mapTemplate.Connections.Add(new Connection("A", "B"));
            mapTemplate.Connections.Add(new Connection("B", "A"));
            mapTemplate.Connections.Add(new Connection("A", "C"));
            mapTemplate.Connections.Add(new Connection("C", "A"));
            mapTemplate.Connections.Add(new Connection("C", "D"));
            mapTemplate.Connections.Add(new Connection("D", "C"));
            mapTemplate.Connections.Add(new Connection("B", "D"));
            mapTemplate.Connections.Add(new Connection("D", "B"));
            mapTemplate.Connections.Add(new Connection("B", "C"));
            mapTemplate.Connections.Add(new Connection("C", "B"));

            return mapTemplate;
        }
    }
}
