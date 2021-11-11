using ImperaPlus.Domain.Map;

namespace ImperaPlus.DataAccess.ConvertedMaps
{
    public static partial class Maps
    {
        public static MapTemplate Rumaenien()
        {
            var mapTemplate = new MapTemplate("Rumaenien") { Image = "rumaenien.png" };
            var country1 = new CountryTemplate("1", "Timis") { X = 133, Y = 360 };
            mapTemplate.Countries.Add(country1);
            var country2 = new CountryTemplate("2", "Caras-Severin") { X = 156, Y = 479 };
            mapTemplate.Countries.Add(country2);
            var country3 = new CountryTemplate("3", "Arad") { X = 164, Y = 302 };
            mapTemplate.Countries.Add(country3);
            var country4 = new CountryTemplate("4", "Bihor") { X = 200, Y = 215 };
            mapTemplate.Countries.Add(country4);
            var country5 = new CountryTemplate("5", "Satu-Mare") { X = 247, Y = 102 };
            mapTemplate.Countries.Add(country5);
            var country6 = new CountryTemplate("6", "Maramures") { X = 368, Y = 97 };
            mapTemplate.Countries.Add(country6);
            var country7 = new CountryTemplate("7", "Suceava") { X = 546, Y = 116 };
            mapTemplate.Countries.Add(country7);
            var country8 = new CountryTemplate("8", "Botosani") { X = 659, Y = 75 };
            mapTemplate.Countries.Add(country8);
            var country9 = new CountryTemplate("9", "Tulcea") { X = 850, Y = 484 };
            mapTemplate.Countries.Add(country9);
            var country10 = new CountryTemplate("10", "Constanta") { X = 820, Y = 573 };
            mapTemplate.Countries.Add(country10);
            var country11 = new CountryTemplate("11", "Ilfov") { X = 595, Y = 531 };
            mapTemplate.Countries.Add(country11);
            var country12 = new CountryTemplate("12", "Braila") { X = 752, Y = 479 };
            mapTemplate.Countries.Add(country12);
            var country13 = new CountryTemplate("13", "Ialomita") { X = 743, Y = 523 };
            mapTemplate.Countries.Add(country13);
            var country14 = new CountryTemplate("14", "Calarasi") { X = 657, Y = 587 };
            mapTemplate.Countries.Add(country14);
            var country15 = new CountryTemplate("15", "Buzau") { X = 650, Y = 445 };
            mapTemplate.Countries.Add(country15);
            var country16 = new CountryTemplate("16", "Prahova") { X = 572, Y = 448 };
            mapTemplate.Countries.Add(country16);
            var country17 = new CountryTemplate("17", "Dambovita") { X = 522, Y = 518 };
            mapTemplate.Countries.Add(country17);
            var country18 = new CountryTemplate("18", "Giurgiu") { X = 571, Y = 599 };
            mapTemplate.Countries.Add(country18);
            var country19 = new CountryTemplate("19", "Teleorma") { X = 493, Y = 610 };
            mapTemplate.Countries.Add(country19);
            var country20 = new CountryTemplate("20", "Arges") { X = 462, Y = 479 };
            mapTemplate.Countries.Add(country20);
            var country21 = new CountryTemplate("21", "Olt") { X = 423, Y = 593 };
            mapTemplate.Countries.Add(country21);
            var country22 = new CountryTemplate("22", "Dolj") { X = 354, Y = 613 };
            mapTemplate.Countries.Add(country22);
            var country23 = new CountryTemplate("23", "Valcea") { X = 390, Y = 465 };
            mapTemplate.Countries.Add(country23);
            var country24 = new CountryTemplate("24", "Gorj") { X = 311, Y = 496 };
            mapTemplate.Countries.Add(country24);
            var country25 = new CountryTemplate("25", "Mehedinti") { X = 254, Y = 562 };
            mapTemplate.Countries.Add(country25);
            var country26 = new CountryTemplate("26", "Covasina") { X = 583, Y = 360 };
            mapTemplate.Countries.Add(country26);
            var country27 = new CountryTemplate("27", "Brasov") { X = 490, Y = 343 };
            mapTemplate.Countries.Add(country27);
            var country28 = new CountryTemplate("28", "Sibiju") { X = 413, Y = 336 };
            mapTemplate.Countries.Add(country28);
            var country29 = new CountryTemplate("29", "Alba") { X = 327, Y = 319 };
            mapTemplate.Countries.Add(country29);
            var country30 = new CountryTemplate("30", "Hunedora") { X = 256, Y = 351 };
            mapTemplate.Countries.Add(country30);
            var country31 = new CountryTemplate("31", "Cluj") { X = 342, Y = 223 };
            mapTemplate.Countries.Add(country31);
            var country32 = new CountryTemplate("32", "Salaj") { X = 297, Y = 171 };
            mapTemplate.Countries.Add(country32);
            var country33 = new CountryTemplate("33", "Bistrata-Nasaud") { X = 421, Y = 170 };
            mapTemplate.Countries.Add(country33);
            var country34 = new CountryTemplate("34", "Mures") { X = 473, Y = 218 };
            mapTemplate.Countries.Add(country34);
            var country35 = new CountryTemplate("35", "Harghita") { X = 527, Y = 249 };
            mapTemplate.Countries.Add(country35);
            var country36 = new CountryTemplate("36", "Neamt") { X = 606, Y = 201 };
            mapTemplate.Countries.Add(country36);
            var country37 = new CountryTemplate("37", "Iasi") { X = 718, Y = 158 };
            mapTemplate.Countries.Add(country37);
            var country38 = new CountryTemplate("38", "Bacau") { X = 660, Y = 288 };
            mapTemplate.Countries.Add(country38);
            var country39 = new CountryTemplate("39", "Vaslui") { X = 765, Y = 263 };
            mapTemplate.Countries.Add(country39);
            var country40 = new CountryTemplate("40", "Vrancea") { X = 682, Y = 370 };
            mapTemplate.Countries.Add(country40);
            var country41 = new CountryTemplate("41", "Galati") { X = 760, Y = 367 };
            mapTemplate.Countries.Add(country41);
            var continent1 = new Continent("1", 1);
            continent1.Countries.Add(country1);
            continent1.Countries.Add(country2);
            mapTemplate.Continents.Add(continent1);
            var continent2 = new Continent("2", 1);
            continent2.Countries.Add(country3);
            continent2.Countries.Add(country4);
            mapTemplate.Continents.Add(continent2);
            var continent3 = new Continent("3", 1);
            continent3.Countries.Add(country5);
            continent3.Countries.Add(country6);
            mapTemplate.Continents.Add(continent3);
            var continent4 = new Continent("4", 1);
            continent4.Countries.Add(country7);
            continent4.Countries.Add(country8);
            mapTemplate.Continents.Add(continent4);
            var continent5 = new Continent("5", 1);
            continent5.Countries.Add(country9);
            continent5.Countries.Add(country10);
            mapTemplate.Continents.Add(continent5);
            var continent6 = new Continent("6", 1);
            continent6.Countries.Add(country11);
            mapTemplate.Continents.Add(continent6);
            var continent7 = new Continent("7", 6);
            continent7.Countries.Add(country12);
            continent7.Countries.Add(country13);
            continent7.Countries.Add(country14);
            continent7.Countries.Add(country15);
            continent7.Countries.Add(country16);
            continent7.Countries.Add(country17);
            continent7.Countries.Add(country18);
            continent7.Countries.Add(country19);
            continent7.Countries.Add(country20);
            mapTemplate.Continents.Add(continent7);
            var continent8 = new Continent("8", 3);
            continent8.Countries.Add(country21);
            continent8.Countries.Add(country22);
            continent8.Countries.Add(country23);
            continent8.Countries.Add(country24);
            continent8.Countries.Add(country25);
            mapTemplate.Continents.Add(continent8);
            var continent9 = new Continent("9", 7);
            continent9.Countries.Add(country26);
            continent9.Countries.Add(country27);
            continent9.Countries.Add(country28);
            continent9.Countries.Add(country29);
            continent9.Countries.Add(country30);
            continent9.Countries.Add(country31);
            continent9.Countries.Add(country32);
            continent9.Countries.Add(country33);
            continent9.Countries.Add(country34);
            continent9.Countries.Add(country35);
            mapTemplate.Continents.Add(continent9);
            var continent10 = new Continent("10", 4);
            continent10.Countries.Add(country36);
            continent10.Countries.Add(country37);
            continent10.Countries.Add(country38);
            continent10.Countries.Add(country39);
            continent10.Countries.Add(country40);
            continent10.Countries.Add(country41);
            mapTemplate.Continents.Add(continent10);
            mapTemplate.Connections.Add(new Connection("1", "3"));
            mapTemplate.Connections.Add(new Connection("1", "2"));
            mapTemplate.Connections.Add(new Connection("1", "30"));
            mapTemplate.Connections.Add(new Connection("2", "1"));
            mapTemplate.Connections.Add(new Connection("2", "30"));
            mapTemplate.Connections.Add(new Connection("2", "24"));
            mapTemplate.Connections.Add(new Connection("2", "25"));
            mapTemplate.Connections.Add(new Connection("3", "4"));
            mapTemplate.Connections.Add(new Connection("3", "1"));
            mapTemplate.Connections.Add(new Connection("3", "30"));
            mapTemplate.Connections.Add(new Connection("3", "29"));
            mapTemplate.Connections.Add(new Connection("4", "5"));
            mapTemplate.Connections.Add(new Connection("4", "3"));
            mapTemplate.Connections.Add(new Connection("4", "29"));
            mapTemplate.Connections.Add(new Connection("4", "31"));
            mapTemplate.Connections.Add(new Connection("4", "32"));
            mapTemplate.Connections.Add(new Connection("5", "4"));
            mapTemplate.Connections.Add(new Connection("5", "32"));
            mapTemplate.Connections.Add(new Connection("5", "6"));
            mapTemplate.Connections.Add(new Connection("6", "5"));
            mapTemplate.Connections.Add(new Connection("6", "32"));
            mapTemplate.Connections.Add(new Connection("6", "31"));
            mapTemplate.Connections.Add(new Connection("6", "33"));
            mapTemplate.Connections.Add(new Connection("6", "7"));
            mapTemplate.Connections.Add(new Connection("7", "6"));
            mapTemplate.Connections.Add(new Connection("7", "33"));
            mapTemplate.Connections.Add(new Connection("7", "34"));
            mapTemplate.Connections.Add(new Connection("7", "35"));
            mapTemplate.Connections.Add(new Connection("7", "8"));
            mapTemplate.Connections.Add(new Connection("7", "36"));
            mapTemplate.Connections.Add(new Connection("7", "37"));
            mapTemplate.Connections.Add(new Connection("8", "7"));
            mapTemplate.Connections.Add(new Connection("8", "37"));
            mapTemplate.Connections.Add(new Connection("9", "10"));
            mapTemplate.Connections.Add(new Connection("9", "12"));
            mapTemplate.Connections.Add(new Connection("9", "41"));
            mapTemplate.Connections.Add(new Connection("10", "9"));
            mapTemplate.Connections.Add(new Connection("10", "14"));
            mapTemplate.Connections.Add(new Connection("10", "13"));
            mapTemplate.Connections.Add(new Connection("10", "12"));
            mapTemplate.Connections.Add(new Connection("11", "16"));
            mapTemplate.Connections.Add(new Connection("11", "17"));
            mapTemplate.Connections.Add(new Connection("11", "18"));
            mapTemplate.Connections.Add(new Connection("11", "14"));
            mapTemplate.Connections.Add(new Connection("11", "13"));
            mapTemplate.Connections.Add(new Connection("12", "40"));
            mapTemplate.Connections.Add(new Connection("12", "41"));
            mapTemplate.Connections.Add(new Connection("12", "15"));
            mapTemplate.Connections.Add(new Connection("12", "13"));
            mapTemplate.Connections.Add(new Connection("12", "10"));
            mapTemplate.Connections.Add(new Connection("12", "9"));
            mapTemplate.Connections.Add(new Connection("13", "12"));
            mapTemplate.Connections.Add(new Connection("13", "15"));
            mapTemplate.Connections.Add(new Connection("13", "14"));
            mapTemplate.Connections.Add(new Connection("13", "11"));
            mapTemplate.Connections.Add(new Connection("13", "10"));
            mapTemplate.Connections.Add(new Connection("13", "16"));
            mapTemplate.Connections.Add(new Connection("14", "13"));
            mapTemplate.Connections.Add(new Connection("14", "18"));
            mapTemplate.Connections.Add(new Connection("14", "11"));
            mapTemplate.Connections.Add(new Connection("14", "10"));
            mapTemplate.Connections.Add(new Connection("15", "27"));
            mapTemplate.Connections.Add(new Connection("15", "26"));
            mapTemplate.Connections.Add(new Connection("15", "16"));
            mapTemplate.Connections.Add(new Connection("15", "40"));
            mapTemplate.Connections.Add(new Connection("15", "12"));
            mapTemplate.Connections.Add(new Connection("15", "13"));
            mapTemplate.Connections.Add(new Connection("16", "27"));
            mapTemplate.Connections.Add(new Connection("16", "17"));
            mapTemplate.Connections.Add(new Connection("16", "15"));
            mapTemplate.Connections.Add(new Connection("16", "11"));
            mapTemplate.Connections.Add(new Connection("16", "13"));
            mapTemplate.Connections.Add(new Connection("17", "19"));
            mapTemplate.Connections.Add(new Connection("17", "20"));
            mapTemplate.Connections.Add(new Connection("17", "18"));
            mapTemplate.Connections.Add(new Connection("17", "27"));
            mapTemplate.Connections.Add(new Connection("17", "16"));
            mapTemplate.Connections.Add(new Connection("17", "11"));
            mapTemplate.Connections.Add(new Connection("18", "19"));
            mapTemplate.Connections.Add(new Connection("18", "17"));
            mapTemplate.Connections.Add(new Connection("18", "14"));
            mapTemplate.Connections.Add(new Connection("18", "11"));
            mapTemplate.Connections.Add(new Connection("19", "21"));
            mapTemplate.Connections.Add(new Connection("19", "20"));
            mapTemplate.Connections.Add(new Connection("19", "18"));
            mapTemplate.Connections.Add(new Connection("19", "17"));
            mapTemplate.Connections.Add(new Connection("20", "21"));
            mapTemplate.Connections.Add(new Connection("20", "23"));
            mapTemplate.Connections.Add(new Connection("20", "28"));
            mapTemplate.Connections.Add(new Connection("20", "27"));
            mapTemplate.Connections.Add(new Connection("20", "19"));
            mapTemplate.Connections.Add(new Connection("20", "17"));
            mapTemplate.Connections.Add(new Connection("21", "22"));
            mapTemplate.Connections.Add(new Connection("21", "23"));
            mapTemplate.Connections.Add(new Connection("21", "19"));
            mapTemplate.Connections.Add(new Connection("21", "20"));
            mapTemplate.Connections.Add(new Connection("22", "25"));
            mapTemplate.Connections.Add(new Connection("22", "24"));
            mapTemplate.Connections.Add(new Connection("22", "21"));
            mapTemplate.Connections.Add(new Connection("22", "23"));
            mapTemplate.Connections.Add(new Connection("23", "30"));
            mapTemplate.Connections.Add(new Connection("23", "24"));
            mapTemplate.Connections.Add(new Connection("23", "21"));
            mapTemplate.Connections.Add(new Connection("23", "29"));
            mapTemplate.Connections.Add(new Connection("23", "28"));
            mapTemplate.Connections.Add(new Connection("23", "20"));
            mapTemplate.Connections.Add(new Connection("23", "22"));
            mapTemplate.Connections.Add(new Connection("24", "30"));
            mapTemplate.Connections.Add(new Connection("24", "2"));
            mapTemplate.Connections.Add(new Connection("24", "25"));
            mapTemplate.Connections.Add(new Connection("24", "22"));
            mapTemplate.Connections.Add(new Connection("24", "23"));
            mapTemplate.Connections.Add(new Connection("25", "2"));
            mapTemplate.Connections.Add(new Connection("25", "24"));
            mapTemplate.Connections.Add(new Connection("25", "22"));
            mapTemplate.Connections.Add(new Connection("26", "27"));
            mapTemplate.Connections.Add(new Connection("26", "35"));
            mapTemplate.Connections.Add(new Connection("26", "38"));
            mapTemplate.Connections.Add(new Connection("26", "40"));
            mapTemplate.Connections.Add(new Connection("26", "15"));
            mapTemplate.Connections.Add(new Connection("27", "34"));
            mapTemplate.Connections.Add(new Connection("27", "28"));
            mapTemplate.Connections.Add(new Connection("27", "35"));
            mapTemplate.Connections.Add(new Connection("27", "26"));
            mapTemplate.Connections.Add(new Connection("27", "20"));
            mapTemplate.Connections.Add(new Connection("27", "17"));
            mapTemplate.Connections.Add(new Connection("27", "16"));
            mapTemplate.Connections.Add(new Connection("27", "15"));
            mapTemplate.Connections.Add(new Connection("28", "23"));
            mapTemplate.Connections.Add(new Connection("28", "29"));
            mapTemplate.Connections.Add(new Connection("28", "34"));
            mapTemplate.Connections.Add(new Connection("28", "27"));
            mapTemplate.Connections.Add(new Connection("28", "20"));
            mapTemplate.Connections.Add(new Connection("29", "3"));
            mapTemplate.Connections.Add(new Connection("29", "4"));
            mapTemplate.Connections.Add(new Connection("29", "30"));
            mapTemplate.Connections.Add(new Connection("29", "23"));
            mapTemplate.Connections.Add(new Connection("29", "28"));
            mapTemplate.Connections.Add(new Connection("29", "31"));
            mapTemplate.Connections.Add(new Connection("29", "34"));
            mapTemplate.Connections.Add(new Connection("30", "2"));
            mapTemplate.Connections.Add(new Connection("30", "1"));
            mapTemplate.Connections.Add(new Connection("30", "3"));
            mapTemplate.Connections.Add(new Connection("30", "29"));
            mapTemplate.Connections.Add(new Connection("30", "23"));
            mapTemplate.Connections.Add(new Connection("30", "24"));
            mapTemplate.Connections.Add(new Connection("31", "4"));
            mapTemplate.Connections.Add(new Connection("31", "29"));
            mapTemplate.Connections.Add(new Connection("31", "32"));
            mapTemplate.Connections.Add(new Connection("31", "6"));
            mapTemplate.Connections.Add(new Connection("31", "33"));
            mapTemplate.Connections.Add(new Connection("31", "34"));
            mapTemplate.Connections.Add(new Connection("32", "5"));
            mapTemplate.Connections.Add(new Connection("32", "4"));
            mapTemplate.Connections.Add(new Connection("32", "6"));
            mapTemplate.Connections.Add(new Connection("32", "31"));
            mapTemplate.Connections.Add(new Connection("33", "6"));
            mapTemplate.Connections.Add(new Connection("33", "31"));
            mapTemplate.Connections.Add(new Connection("33", "34"));
            mapTemplate.Connections.Add(new Connection("33", "7"));
            mapTemplate.Connections.Add(new Connection("34", "33"));
            mapTemplate.Connections.Add(new Connection("34", "31"));
            mapTemplate.Connections.Add(new Connection("34", "28"));
            mapTemplate.Connections.Add(new Connection("34", "35"));
            mapTemplate.Connections.Add(new Connection("34", "27"));
            mapTemplate.Connections.Add(new Connection("34", "7"));
            mapTemplate.Connections.Add(new Connection("34", "29"));
            mapTemplate.Connections.Add(new Connection("35", "34"));
            mapTemplate.Connections.Add(new Connection("35", "27"));
            mapTemplate.Connections.Add(new Connection("35", "26"));
            mapTemplate.Connections.Add(new Connection("35", "7"));
            mapTemplate.Connections.Add(new Connection("35", "36"));
            mapTemplate.Connections.Add(new Connection("35", "38"));
            mapTemplate.Connections.Add(new Connection("36", "7"));
            mapTemplate.Connections.Add(new Connection("36", "37"));
            mapTemplate.Connections.Add(new Connection("36", "35"));
            mapTemplate.Connections.Add(new Connection("36", "38"));
            mapTemplate.Connections.Add(new Connection("36", "39"));
            mapTemplate.Connections.Add(new Connection("37", "8"));
            mapTemplate.Connections.Add(new Connection("37", "7"));
            mapTemplate.Connections.Add(new Connection("37", "36"));
            mapTemplate.Connections.Add(new Connection("37", "39"));
            mapTemplate.Connections.Add(new Connection("38", "35"));
            mapTemplate.Connections.Add(new Connection("38", "36"));
            mapTemplate.Connections.Add(new Connection("38", "39"));
            mapTemplate.Connections.Add(new Connection("38", "26"));
            mapTemplate.Connections.Add(new Connection("38", "40"));
            mapTemplate.Connections.Add(new Connection("39", "36"));
            mapTemplate.Connections.Add(new Connection("39", "38"));
            mapTemplate.Connections.Add(new Connection("39", "37"));
            mapTemplate.Connections.Add(new Connection("39", "41"));
            mapTemplate.Connections.Add(new Connection("39", "40"));
            mapTemplate.Connections.Add(new Connection("40", "39"));
            mapTemplate.Connections.Add(new Connection("40", "38"));
            mapTemplate.Connections.Add(new Connection("40", "41"));
            mapTemplate.Connections.Add(new Connection("40", "26"));
            mapTemplate.Connections.Add(new Connection("40", "15"));
            mapTemplate.Connections.Add(new Connection("40", "12"));
            mapTemplate.Connections.Add(new Connection("41", "39"));
            mapTemplate.Connections.Add(new Connection("41", "40"));
            mapTemplate.Connections.Add(new Connection("41", "12"));
            mapTemplate.Connections.Add(new Connection("41", "9"));

            return mapTemplate;
        }
    }
}
