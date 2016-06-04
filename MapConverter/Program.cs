using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MapConverter
{
    class Program
    {
        private class ContryRep
        {
            public string Identifier;
            public string Name;
            public int X;
            public int Y;
        }

        private class ContinentRep
        {
            public string Identifier;
            public int Bonus;
            public List<string> Countries = new List<string>();
        }

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Converts ImperaV4 maps. Usage: xxx.exe <inputMap.php>");

                return;
            }

            string mapName = null;
            string image = null;
            int countryCount;
            var countries = new Dictionary<string, ContryRep>();
            var connections = new List<Tuple<string, string>>();
            var continents = new Dictionary<string, ContinentRep>();

            var file = File.OpenRead(args[0]);
            var textReader = new StreamReader(file);
                
            string line = textReader.ReadLine();
            while (line != null)
            {
                if (line.Contains("class"))
                {
                    mapName = GetValue<string>(line, @"class (.*) extends");
                }
                else if (line.Contains("$laender ="))
                {
                    countryCount = GetValue<int>(line, @"\$laender = (\d+);");
                }
                else if (line.Contains("$Image ="))
                {
                    image = GetValue<string>(line, @"\$Image = \""(.*)\"";");
                }
                else if (line.Contains("$L["))
                {
                    var countryId = GetValue<string>(line, @"\$L\[(\d+)\]");
                    var countryName = GetValue<string>(line, @"""(.+)""");

                    countries.Add(countryId, new ContryRep
                    {
                        Identifier = countryId,
                        Name = countryName
                    });
                }
                else if (line.Contains("$P["))
                {
                    // Coordinates
                    var coords = GetValues<int>(line, @"\$P\[(\d+)\]\[0\] = (\d+); \$P\[\d+\]\[1\] = (\d+);");
                    countries[coords[0].ToString()].X = coords[1];
                    countries[coords[0].ToString()].Y = coords[2];
                }
                else if (line.Contains("$A["))
                {
                    // Connection
                    var conn = GetValues<string>(line, @"\$A\[(\d+)\] = array\((?:\s?(\d+)\s*,?\s?)+\);");

                    for (int i = 1; i < conn.Count; ++i)
                    {
                        connections.Add(Tuple.Create(conn[0], conn[i]));
                    }
                }
                else if (line.Contains("$k") && line.Contains("][0] = "))
                {
                    var cont = GetValues<string>(line, @"\$k\[(\d+)\]\[0\] = '?(\d)+'?;");

                    if (cont[0] != "0")
                    {
                        continents.Add(cont[0], new ContinentRep
                        {
                            Identifier = cont[0],
                            Bonus = int.Parse(cont[1])
                        });
                    }
                }
                else if (line.Contains("$k") && line.Contains("][1] = "))
                {
                    var cont = GetValues<string>(line, @"\$k\[(\d+)\]\[1\] = [\'\""]\((?:\s*landid\s?=\s?(\d+)[\sOR]*)+\)[\'\""];");

                    for (int i = 1; i < cont.Count; ++i)
                    {
                        continents[cont[0]].Countries.Add(cont[i]);
                    }
                }

                line = textReader.ReadLine();
            }

            #region Debug

#if false
            sw.WriteLine("Read {0} countries", countries.Count);
            foreach (var country in countries.Values)
            {
                sw.WriteLine("\t{0} {1} {2}:{3}", country.Identifier, country.Name, country.X, country.Y);
            }

            sw.WriteLine();
            sw.WriteLine();

            sw.WriteLine("Read {0} connections", connections.Count);
            foreach (var connection in connections)
            {
                sw.WriteLine("{0} -> {1}", countries[connection.Item1].Name, countries[connection.Item2].Name);
            }

            sw.WriteLine();
            sw.WriteLine();

            sw.WriteLine("Read {0} continents", continents.Count);
            foreach (var continent in continents.Values)
            {
                sw.WriteLine("{0} - {1}", continent.Identifier, continent.Bonus);
                foreach (var country in continent.Countries)
                {
                    sw.WriteLine("\t{0}", countries[country].Name);
                }
            }

            sw.WriteLine(mapName);
            sw.WriteLine(image);

            var mapTemplate = new MapTemplate(mapName)
            {                
                Image = image,
            };

            foreach (var country in countries.Values)
            {
                mapTemplate.Countries.Add(new CountryTemplate(country.Identifier.ToString(), country.Name)
                {
                    Name = country.Name,
                    X = country.X,
                    Y = country.Y
                });
            }

            sw.WriteLine();
#endif
            #endregion

            if (connections.Count == 0)
            {
                throw new Exception("No connections!");
            }

            if (continents.Count == 0)
            {
                throw new Exception("No continents!");
            }

            if (countries.Count == 0)
            {
                throw new Exception("No countries!");
            }

            foreach(var continent in continents)
            {
                if (continent.Value.Countries.Count == 0)
                {
                    throw new Exception("Continent has no countries!");
                }
            }

            using (var outputFile = File.OpenWrite(string.Format("Maps.{0}.cs", mapName)))
            using (var sw = new StreamWriter(outputFile))
            {
                sw.WriteLine(@"
using ImperaPlus.Domain.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.DataAccess.ConvertedMaps
{{
    public static partial class Maps
    {{
        public static MapTemplate {0}()
        {{
", mapName);

                sw.WriteLine("var mapTemplate = new MapTemplate(\"{0}\") {{ Image = \"{1}\" }};", mapName, image);

                foreach (var country in countries.Values)
                {
                    sw.WriteLine("var country{0} = new CountryTemplate(\"{1}\", \"{4}\") {{ X = {2}, Y = {3} }};", country.Identifier, country.Identifier, country.X, country.Y, country.Name);
                    sw.WriteLine("mapTemplate.Countries.Add(country{0});", country.Identifier);
                }

                foreach (var continent in continents.Values)
                {
                    sw.WriteLine("var continent{0} = new Continent(\"{1}\", {2});", continent.Identifier, continent.Identifier,
                        continent.Bonus);

                    foreach (var country in continent.Countries)
                    {
                        sw.WriteLine("continent{0}.Countries.Add(country{1});", continent.Identifier, country);
                    }

                    sw.WriteLine("mapTemplate.Continents.Add(continent{0});", continent.Identifier);
                }

                foreach (var connection in connections)
                {
                    sw.WriteLine("mapTemplate.Connections.Add(new Connection(\"{0}\", \"{1}\"));", connection.Item1,
                        connection.Item2);
                }

                sw.WriteLine(@"
            return mapTemplate;
		}
    }
}
");
            }
        }

        private static T GetValue<T>(string input, string pattern)
        {
            return GetValues<T>(input, pattern).First();
        }

        private static IList<T> GetValues<T>(string src, string pattern)
        {
            var match = Regex.Match(src, pattern);

            var result = new List<T>();

            for (int i = 1; i < match.Groups.Count; ++i)
            {
                foreach (var capture in match.Groups[i].Captures)
                {
                    result.Add((T) Convert.ChangeType(capture.ToString(), typeof (T)));
                }
            }

            return result;
        }
    }
}
