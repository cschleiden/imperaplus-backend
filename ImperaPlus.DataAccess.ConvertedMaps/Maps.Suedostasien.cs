using ImperaPlus.Domain.Map;

namespace ImperaPlus.DataAccess.ConvertedMaps
{
    public static partial class Maps
    {
        public static MapTemplate Suedostasien()
        {
            var mapTemplate = new MapTemplate("Suedostasien") { Image = "soa.jpg" };
            var country1 = new CountryTemplate("1", "Bhutan") { X = 54, Y = 44 };
            mapTemplate.Countries.Add(country1);
            var country2 = new CountryTemplate("2", "Indien") { X = 99, Y = 86 };
            mapTemplate.Countries.Add(country2);
            var country3 = new CountryTemplate("3", "Bangladesch") { X = 84, Y = 134 };
            mapTemplate.Countries.Add(country3);
            var country4 = new CountryTemplate("4", "Andamanen") { X = 113, Y = 253 };
            mapTemplate.Countries.Add(country4);
            var country5 = new CountryTemplate("5", "Nikobaren") { X = 125, Y = 310 };
            mapTemplate.Countries.Add(country5);
            var country6 = new CountryTemplate("6", "Yunnan") { X = 195, Y = 112 };
            mapTemplate.Countries.Add(country6);
            var country7 = new CountryTemplate("7", "Guizhou") { X = 237, Y = 84 };
            mapTemplate.Countries.Add(country7);
            var country8 = new CountryTemplate("8", "Guangxi") { X = 266, Y = 122 };
            mapTemplate.Countries.Add(country8);
            var country9 = new CountryTemplate("9", "Hunan") { X = 286, Y = 78 };
            mapTemplate.Countries.Add(country9);
            var country10 = new CountryTemplate("10", "Guangdong") { X = 315, Y = 126 };
            mapTemplate.Countries.Add(country10);
            var country11 = new CountryTemplate("11", "Fujian") { X = 355, Y = 90 };
            mapTemplate.Countries.Add(country11);
            var country12 = new CountryTemplate("12", "Jiangxi") { X = 319, Y = 72 };
            mapTemplate.Countries.Add(country12);
            var country13 = new CountryTemplate("13", "Zehjiang") { X = 370, Y = 56 };
            mapTemplate.Countries.Add(country13);
            var country14 = new CountryTemplate("14", "Taiwan") { X = 405, Y = 118 };
            mapTemplate.Countries.Add(country14);
            var country15 = new CountryTemplate("15", "Hainan") { X = 301, Y = 174 };
            mapTemplate.Countries.Add(country15);
            var country16 = new CountryTemplate("16", "Myanmar") { X = 143, Y = 162 };
            mapTemplate.Countries.Add(country16);
            var country17 = new CountryTemplate("17", "Laos") { X = 208, Y = 165 };
            mapTemplate.Countries.Add(country17);
            var country18 = new CountryTemplate("18", "Vietnam") { X = 240, Y = 151 };
            mapTemplate.Countries.Add(country18);
            var country19 = new CountryTemplate("19", "Kambodscha") { X = 240, Y = 243 };
            mapTemplate.Countries.Add(country19);
            var country20 = new CountryTemplate("20", "Thailand") { X = 197, Y = 207 };
            mapTemplate.Countries.Add(country20);
            var country21 = new CountryTemplate("21", "Luzon") { X = 422, Y = 212 };
            mapTemplate.Countries.Add(country21);
            var country22 = new CountryTemplate("22", "Visayas") { X = 433, Y = 268 };
            mapTemplate.Countries.Add(country22);
            var country23 = new CountryTemplate("23", "Palawan") { X = 396, Y = 279 };
            mapTemplate.Countries.Add(country23);
            var country24 = new CountryTemplate("24", "Mindanao") { X = 463, Y = 306 };
            mapTemplate.Countries.Add(country24);
            var country25 = new CountryTemplate("25", "Palau") { X = 545, Y = 309 };
            mapTemplate.Countries.Add(country25);
            var country26 = new CountryTemplate("26", "Brunei") { X = 352, Y = 303 };
            mapTemplate.Countries.Add(country26);
            var country27 = new CountryTemplate("27", "OstMalaysia") { X = 332, Y = 358 };
            mapTemplate.Countries.Add(country27);
            var country28 = new CountryTemplate("28", "WestMalaysia") { X = 222, Y = 338 };
            mapTemplate.Countries.Add(country28);
            var country29 = new CountryTemplate("29", "Singapur") { X = 251, Y = 375 };
            mapTemplate.Countries.Add(country29);
            var country30 = new CountryTemplate("30", "Sumatra") { X = 226, Y = 406 };
            mapTemplate.Countries.Add(country30);
            var country31 = new CountryTemplate("31", "Borneo") { X = 349, Y = 403 };
            mapTemplate.Countries.Add(country31);
            var country32 = new CountryTemplate("32", "Java") { X = 310, Y = 474 };
            mapTemplate.Countries.Add(country32);
            var country33 = new CountryTemplate("33", "Bali") { X = 372, Y = 493 };
            mapTemplate.Countries.Add(country33);
            var country34 = new CountryTemplate("34", "Flores") { X = 417, Y = 492 };
            mapTemplate.Countries.Add(country34);
            var country35 = new CountryTemplate("35", "Celebes") { X = 417, Y = 414 };
            mapTemplate.Countries.Add(country35);
            var country36 = new CountryTemplate("36", "Ternate") { X = 500, Y = 380 };
            mapTemplate.Countries.Add(country36);
            var country37 = new CountryTemplate("37", "Ambon") { X = 506, Y = 430 };
            mapTemplate.Countries.Add(country37);
            var country38 = new CountryTemplate("38", "Timor") { X = 463, Y = 494 };
            mapTemplate.Countries.Add(country38);
            var country39 = new CountryTemplate("39", "WestPapua") { X = 615, Y = 439 };
            mapTemplate.Countries.Add(country39);
            var country40 = new CountryTemplate("40", "PapuaNeuguinea") { X = 660, Y = 464 };
            mapTemplate.Countries.Add(country40);
            var country41 = new CountryTemplate("41", "Neuirland") { X = 759, Y = 425 };
            mapTemplate.Countries.Add(country41);
            var country42 = new CountryTemplate("42", "Neubritannien") { X = 728, Y = 468 };
            mapTemplate.Countries.Add(country42);
            var country43 = new CountryTemplate("43", "Salomonen") { X = 805, Y = 494 };
            mapTemplate.Countries.Add(country43);
            var continent1 = new Continent("1", 2);
            continent1.Countries.Add(country40);
            continent1.Countries.Add(country41);
            continent1.Countries.Add(country42);
            continent1.Countries.Add(country43);
            mapTemplate.Continents.Add(continent1);
            var continent2 = new Continent("2", 3);
            continent2.Countries.Add(country1);
            continent2.Countries.Add(country2);
            continent2.Countries.Add(country3);
            continent2.Countries.Add(country4);
            continent2.Countries.Add(country5);
            mapTemplate.Continents.Add(continent2);
            var continent3 = new Continent("3", 3);
            continent3.Countries.Add(country26);
            continent3.Countries.Add(country27);
            continent3.Countries.Add(country28);
            continent3.Countries.Add(country29);
            mapTemplate.Continents.Add(continent3);
            var continent4 = new Continent("4", 3);
            continent4.Countries.Add(country21);
            continent4.Countries.Add(country22);
            continent4.Countries.Add(country23);
            continent4.Countries.Add(country24);
            continent4.Countries.Add(country25);
            mapTemplate.Continents.Add(continent4);
            var continent5 = new Continent("5", 5);
            continent5.Countries.Add(country30);
            continent5.Countries.Add(country31);
            continent5.Countries.Add(country32);
            continent5.Countries.Add(country33);
            continent5.Countries.Add(country34);
            continent5.Countries.Add(country35);
            continent5.Countries.Add(country36);
            continent5.Countries.Add(country37);
            continent5.Countries.Add(country38);
            continent5.Countries.Add(country39);
            mapTemplate.Continents.Add(continent5);
            var continent6 = new Continent("6", 3);
            continent6.Countries.Add(country16);
            continent6.Countries.Add(country17);
            continent6.Countries.Add(country18);
            continent6.Countries.Add(country19);
            continent6.Countries.Add(country20);
            mapTemplate.Continents.Add(continent6);
            var continent7 = new Continent("7", 5);
            continent7.Countries.Add(country6);
            continent7.Countries.Add(country7);
            continent7.Countries.Add(country8);
            continent7.Countries.Add(country9);
            continent7.Countries.Add(country10);
            continent7.Countries.Add(country11);
            continent7.Countries.Add(country12);
            continent7.Countries.Add(country13);
            continent7.Countries.Add(country14);
            continent7.Countries.Add(country15);
            mapTemplate.Continents.Add(continent7);
            mapTemplate.Connections.Add(new Connection("1", "2"));
            mapTemplate.Connections.Add(new Connection("2", "1"));
            mapTemplate.Connections.Add(new Connection("2", "3"));
            mapTemplate.Connections.Add(new Connection("2", "16"));
            mapTemplate.Connections.Add(new Connection("2", "4"));
            mapTemplate.Connections.Add(new Connection("3", "2"));
            mapTemplate.Connections.Add(new Connection("3", "16"));
            mapTemplate.Connections.Add(new Connection("4", "2"));
            mapTemplate.Connections.Add(new Connection("4", "5"));
            mapTemplate.Connections.Add(new Connection("4", "16"));
            mapTemplate.Connections.Add(new Connection("5", "20"));
            mapTemplate.Connections.Add(new Connection("5", "4"));
            mapTemplate.Connections.Add(new Connection("5", "30"));
            mapTemplate.Connections.Add(new Connection("6", "16"));
            mapTemplate.Connections.Add(new Connection("6", "7"));
            mapTemplate.Connections.Add(new Connection("6", "8"));
            mapTemplate.Connections.Add(new Connection("6", "18"));
            mapTemplate.Connections.Add(new Connection("6", "17"));
            mapTemplate.Connections.Add(new Connection("7", "6"));
            mapTemplate.Connections.Add(new Connection("7", "9"));
            mapTemplate.Connections.Add(new Connection("7", "8"));
            mapTemplate.Connections.Add(new Connection("8", "10"));
            mapTemplate.Connections.Add(new Connection("8", "15"));
            mapTemplate.Connections.Add(new Connection("8", "9"));
            mapTemplate.Connections.Add(new Connection("8", "7"));
            mapTemplate.Connections.Add(new Connection("8", "6"));
            mapTemplate.Connections.Add(new Connection("8", "18"));
            mapTemplate.Connections.Add(new Connection("9", "7"));
            mapTemplate.Connections.Add(new Connection("9", "12"));
            mapTemplate.Connections.Add(new Connection("9", "10"));
            mapTemplate.Connections.Add(new Connection("9", "8"));
            mapTemplate.Connections.Add(new Connection("10", "11"));
            mapTemplate.Connections.Add(new Connection("10", "12"));
            mapTemplate.Connections.Add(new Connection("10", "9"));
            mapTemplate.Connections.Add(new Connection("10", "8"));
            mapTemplate.Connections.Add(new Connection("10", "15"));
            mapTemplate.Connections.Add(new Connection("10", "21"));
            mapTemplate.Connections.Add(new Connection("11", "13"));
            mapTemplate.Connections.Add(new Connection("11", "12"));
            mapTemplate.Connections.Add(new Connection("11", "14"));
            mapTemplate.Connections.Add(new Connection("11", "10"));
            mapTemplate.Connections.Add(new Connection("12", "9"));
            mapTemplate.Connections.Add(new Connection("12", "13"));
            mapTemplate.Connections.Add(new Connection("12", "11"));
            mapTemplate.Connections.Add(new Connection("12", "10"));
            mapTemplate.Connections.Add(new Connection("13", "12"));
            mapTemplate.Connections.Add(new Connection("13", "11"));
            mapTemplate.Connections.Add(new Connection("13", "14"));
            mapTemplate.Connections.Add(new Connection("14", "11"));
            mapTemplate.Connections.Add(new Connection("14", "13"));
            mapTemplate.Connections.Add(new Connection("14", "21"));
            mapTemplate.Connections.Add(new Connection("15", "10"));
            mapTemplate.Connections.Add(new Connection("15", "8"));
            mapTemplate.Connections.Add(new Connection("15", "18"));
            mapTemplate.Connections.Add(new Connection("16", "2"));
            mapTemplate.Connections.Add(new Connection("16", "3"));
            mapTemplate.Connections.Add(new Connection("16", "6"));
            mapTemplate.Connections.Add(new Connection("16", "20"));
            mapTemplate.Connections.Add(new Connection("16", "4"));
            mapTemplate.Connections.Add(new Connection("16", "17"));
            mapTemplate.Connections.Add(new Connection("17", "18"));
            mapTemplate.Connections.Add(new Connection("17", "20"));
            mapTemplate.Connections.Add(new Connection("17", "6"));
            mapTemplate.Connections.Add(new Connection("17", "16"));
            mapTemplate.Connections.Add(new Connection("17", "19"));
            mapTemplate.Connections.Add(new Connection("18", "8"));
            mapTemplate.Connections.Add(new Connection("18", "6"));
            mapTemplate.Connections.Add(new Connection("18", "17"));
            mapTemplate.Connections.Add(new Connection("18", "15"));
            mapTemplate.Connections.Add(new Connection("18", "19"));
            mapTemplate.Connections.Add(new Connection("18", "21"));
            mapTemplate.Connections.Add(new Connection("18", "26"));
            mapTemplate.Connections.Add(new Connection("18", "28"));
            mapTemplate.Connections.Add(new Connection("19", "18"));
            mapTemplate.Connections.Add(new Connection("19", "20"));
            mapTemplate.Connections.Add(new Connection("19", "17"));
            mapTemplate.Connections.Add(new Connection("20", "19"));
            mapTemplate.Connections.Add(new Connection("20", "17"));
            mapTemplate.Connections.Add(new Connection("20", "16"));
            mapTemplate.Connections.Add(new Connection("20", "5"));
            mapTemplate.Connections.Add(new Connection("20", "28"));
            mapTemplate.Connections.Add(new Connection("21", "18"));
            mapTemplate.Connections.Add(new Connection("21", "23"));
            mapTemplate.Connections.Add(new Connection("21", "10"));
            mapTemplate.Connections.Add(new Connection("21", "14"));
            mapTemplate.Connections.Add(new Connection("21", "22"));
            mapTemplate.Connections.Add(new Connection("22", "23"));
            mapTemplate.Connections.Add(new Connection("22", "24"));
            mapTemplate.Connections.Add(new Connection("22", "25"));
            mapTemplate.Connections.Add(new Connection("22", "21"));
            mapTemplate.Connections.Add(new Connection("23", "27"));
            mapTemplate.Connections.Add(new Connection("23", "22"));
            mapTemplate.Connections.Add(new Connection("23", "21"));
            mapTemplate.Connections.Add(new Connection("24", "27"));
            mapTemplate.Connections.Add(new Connection("24", "22"));
            mapTemplate.Connections.Add(new Connection("24", "25"));
            mapTemplate.Connections.Add(new Connection("24", "36"));
            mapTemplate.Connections.Add(new Connection("24", "35"));
            mapTemplate.Connections.Add(new Connection("25", "22"));
            mapTemplate.Connections.Add(new Connection("25", "24"));
            mapTemplate.Connections.Add(new Connection("25", "41"));
            mapTemplate.Connections.Add(new Connection("25", "39"));
            mapTemplate.Connections.Add(new Connection("26", "18"));
            mapTemplate.Connections.Add(new Connection("26", "27"));
            mapTemplate.Connections.Add(new Connection("27", "28"));
            mapTemplate.Connections.Add(new Connection("27", "26"));
            mapTemplate.Connections.Add(new Connection("27", "23"));
            mapTemplate.Connections.Add(new Connection("27", "24"));
            mapTemplate.Connections.Add(new Connection("27", "31"));
            mapTemplate.Connections.Add(new Connection("28", "18"));
            mapTemplate.Connections.Add(new Connection("28", "30"));
            mapTemplate.Connections.Add(new Connection("28", "29"));
            mapTemplate.Connections.Add(new Connection("28", "20"));
            mapTemplate.Connections.Add(new Connection("28", "27"));
            mapTemplate.Connections.Add(new Connection("29", "30"));
            mapTemplate.Connections.Add(new Connection("29", "28"));
            mapTemplate.Connections.Add(new Connection("30", "5"));
            mapTemplate.Connections.Add(new Connection("30", "28"));
            mapTemplate.Connections.Add(new Connection("30", "29"));
            mapTemplate.Connections.Add(new Connection("30", "32"));
            mapTemplate.Connections.Add(new Connection("30", "31"));
            mapTemplate.Connections.Add(new Connection("31", "32"));
            mapTemplate.Connections.Add(new Connection("31", "35"));
            mapTemplate.Connections.Add(new Connection("31", "30"));
            mapTemplate.Connections.Add(new Connection("31", "27"));
            mapTemplate.Connections.Add(new Connection("32", "33"));
            mapTemplate.Connections.Add(new Connection("32", "31"));
            mapTemplate.Connections.Add(new Connection("32", "30"));
            mapTemplate.Connections.Add(new Connection("33", "34"));
            mapTemplate.Connections.Add(new Connection("33", "32"));
            mapTemplate.Connections.Add(new Connection("34", "38"));
            mapTemplate.Connections.Add(new Connection("34", "35"));
            mapTemplate.Connections.Add(new Connection("34", "33"));
            mapTemplate.Connections.Add(new Connection("35", "38"));
            mapTemplate.Connections.Add(new Connection("35", "34"));
            mapTemplate.Connections.Add(new Connection("35", "31"));
            mapTemplate.Connections.Add(new Connection("35", "24"));
            mapTemplate.Connections.Add(new Connection("35", "36"));
            mapTemplate.Connections.Add(new Connection("35", "37"));
            mapTemplate.Connections.Add(new Connection("36", "24"));
            mapTemplate.Connections.Add(new Connection("36", "39"));
            mapTemplate.Connections.Add(new Connection("36", "37"));
            mapTemplate.Connections.Add(new Connection("36", "35"));
            mapTemplate.Connections.Add(new Connection("37", "36"));
            mapTemplate.Connections.Add(new Connection("37", "39"));
            mapTemplate.Connections.Add(new Connection("37", "38"));
            mapTemplate.Connections.Add(new Connection("37", "35"));
            mapTemplate.Connections.Add(new Connection("38", "39"));
            mapTemplate.Connections.Add(new Connection("38", "37"));
            mapTemplate.Connections.Add(new Connection("38", "35"));
            mapTemplate.Connections.Add(new Connection("38", "34"));
            mapTemplate.Connections.Add(new Connection("39", "25"));
            mapTemplate.Connections.Add(new Connection("39", "36"));
            mapTemplate.Connections.Add(new Connection("39", "37"));
            mapTemplate.Connections.Add(new Connection("39", "38"));
            mapTemplate.Connections.Add(new Connection("39", "40"));
            mapTemplate.Connections.Add(new Connection("39", "41"));
            mapTemplate.Connections.Add(new Connection("40", "39"));
            mapTemplate.Connections.Add(new Connection("40", "42"));
            mapTemplate.Connections.Add(new Connection("40", "43"));
            mapTemplate.Connections.Add(new Connection("41", "25"));
            mapTemplate.Connections.Add(new Connection("41", "42"));
            mapTemplate.Connections.Add(new Connection("41", "39"));
            mapTemplate.Connections.Add(new Connection("41", "43"));
            mapTemplate.Connections.Add(new Connection("42", "40"));
            mapTemplate.Connections.Add(new Connection("42", "41"));
            mapTemplate.Connections.Add(new Connection("42", "43"));
            mapTemplate.Connections.Add(new Connection("43", "41"));
            mapTemplate.Connections.Add(new Connection("43", "42"));
            mapTemplate.Connections.Add(new Connection("43", "40"));

            return mapTemplate;
        }
    }
}
