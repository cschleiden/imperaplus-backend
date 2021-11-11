using ImperaPlus.Domain.Map;

namespace ImperaPlus.DataAccess.ConvertedMaps
{
    public static partial class Maps
    {
        public static MapTemplate Randland()
        {
            var mapTemplate = new MapTemplate("Randland") { Image = "randland.jpg" };
            var country1 = new CountryTemplate("1", "Amadicia") { X = 414, Y = 580 };
            mapTemplate.Countries.Add(country1);
            var country2 = new CountryTemplate("2", "Ghealdan") { X = 420, Y = 496 };
            mapTemplate.Countries.Add(country2);
            var country3 = new CountryTemplate("3", "West-Andor") { X = 585, Y = 451 };
            mapTemplate.Countries.Add(country3);
            var country4 = new CountryTemplate("4", "Caemlyn") { X = 712, Y = 429 };
            mapTemplate.Countries.Add(country4);
            var country5 = new CountryTemplate("5", "Murandy") { X = 614, Y = 516 };
            mapTemplate.Countries.Add(country5);
            var country6 = new CountryTemplate("6", "Illian") { X = 637, Y = 650 };
            mapTemplate.Countries.Add(country6);
            var country7 = new CountryTemplate("7", "Altara") { X = 502, Y = 611 };
            mapTemplate.Countries.Add(country7);
            var country8 = new CountryTemplate("8", "Schattenkueste") { X = 312, Y = 632 };
            mapTemplate.Countries.Add(country8);
            var country9 = new CountryTemplate("9", "Tarabon") { X = 266, Y = 499 };
            mapTemplate.Countries.Add(country9);
            var country10 = new CountryTemplate("10", "EbenevonAlmoth") { X = 306, Y = 400 };
            mapTemplate.Countries.Add(country10);
            var country11 = new CountryTemplate("11", "AradDoman") { X = 291, Y = 279 };
            mapTemplate.Countries.Add(country11);
            var country12 = new CountryTemplate("12", "Saldaea") { X = 511, Y = 155 };
            mapTemplate.Countries.Add(country12);
            var country13 = new CountryTemplate("13", "DieschwarzenHuegel") { X = 655, Y = 240 };
            mapTemplate.Countries.Add(country13);
            var country14 = new CountryTemplate("14", "TarValon") { X = 776, Y = 246 };
            mapTemplate.Countries.Add(country14);
            var country15 = new CountryTemplate("15", "Kandor") { X = 680, Y = 139 };
            mapTemplate.Countries.Add(country15);
            var country16 = new CountryTemplate("16", "Arafel") { X = 761, Y = 130 };
            mapTemplate.Countries.Add(country16);
            var country17 = new CountryTemplate("17", "Shienar") { X = 920, Y = 132 };
            mapTemplate.Countries.Add(country17);
            var country18 = new CountryTemplate("18", "BrudermoerdersDolch") { X = 964, Y = 225 };
            mapTemplate.Countries.Add(country18);
            var country19 = new CountryTemplate("19", "Cairhien") { X = 844, Y = 349 };
            mapTemplate.Countries.Add(country19);
            var country20 = new CountryTemplate("20", "Tear") { X = 804, Y = 640 };
            mapTemplate.Countries.Add(country20);
            var country21 = new CountryTemplate("21", "SteddingShengtai") { X = 958, Y = 596 };
            mapTemplate.Countries.Add(country21);
            var country22 = new CountryTemplate("22", "RueckgratderWelt") { X = 984, Y = 408 };
            mapTemplate.Countries.Add(country22);
            var country23 = new CountryTemplate("23", "Mayene") { X = 972, Y = 684 };
            mapTemplate.Countries.Add(country23);
            var country24 = new CountryTemplate("24", "Malkier") { X = 925, Y = 78 };
            mapTemplate.Countries.Add(country24);
            var country25 = new CountryTemplate("25", "ShayolGhul") { X = 816, Y = 78 };
            mapTemplate.Countries.Add(country25);
            var country26 = new CountryTemplate("26", "VerdorbeneLande") { X = 898, Y = 28 };
            mapTemplate.Countries.Add(country26);
            var country27 = new CountryTemplate("27", "DieGrosseFaeule") { X = 678, Y = 71 };
            mapTemplate.Countries.Add(country27);
            var country28 = new CountryTemplate("28", "WeltsEnd") { X = 401, Y = 104 };
            mapTemplate.Countries.Add(country28);
            var country29 = new CountryTemplate("29", "AileDascher") { X = 96, Y = 165 };
            mapTemplate.Countries.Add(country29);
            var country30 = new CountryTemplate("30", "AileSomera") { X = 16, Y = 388 };
            mapTemplate.Countries.Add(country30);
            var country31 = new CountryTemplate("31", "AileJafar") { X = 123, Y = 522 };
            mapTemplate.Countries.Add(country31);
            var country32 = new CountryTemplate("32", "Tremalking") { X = 35, Y = 702 };
            mapTemplate.Countries.Add(country32);
            var country33 = new CountryTemplate("33", "Qaim") { X = 342, Y = 724 };
            mapTemplate.Countries.Add(country33);
            var country34 = new CountryTemplate("34", "CaralainSteppe") { X = 641, Y = 362 };
            mapTemplate.Countries.Add(country34);
            var country35 = new CountryTemplate("35", "Aridhol") { X = 502, Y = 285 };
            mapTemplate.Countries.Add(country35);
            var country36 = new CountryTemplate("36", "HaddonSuempfe") { X = 852, Y = 570 };
            mapTemplate.Countries.Add(country36);
            var country37 = new CountryTemplate("37", "Aringill") { X = 812, Y = 485 };
            mapTemplate.Countries.Add(country37);
            var country38 = new CountryTemplate("38", "TomanHalbinsel") { X = 193, Y = 383 };
            mapTemplate.Countries.Add(country38);
            var country39 = new CountryTemplate("39", "Cindalking") { X = 922, Y = 734 };
            mapTemplate.Countries.Add(country39);
            var country40 = new CountryTemplate("40", "EbenevonMaredo") { X = 712, Y = 639 };
            mapTemplate.Countries.Add(country40);
            var country41 = new CountryTemplate("41", "FarMadding") { X = 735, Y = 527 };
            mapTemplate.Countries.Add(country41);
            var country42 = new CountryTemplate("42", "WestlicheFaeule") { X = 494, Y = 47 };
            mapTemplate.Countries.Add(country42);
            var country43 = new CountryTemplate("43", "Manetheren") { X = 455, Y = 437 };
            mapTemplate.Countries.Add(country43);
            var continent1 = new Continent("1", 3);
            continent1.Countries.Add(country29);
            continent1.Countries.Add(country30);
            continent1.Countries.Add(country31);
            continent1.Countries.Add(country32);
            continent1.Countries.Add(country33);
            continent1.Countries.Add(country39);
            mapTemplate.Continents.Add(continent1);
            var continent2 = new Continent("2", 3);
            continent2.Countries.Add(country8);
            continent2.Countries.Add(country9);
            continent2.Countries.Add(country10);
            continent2.Countries.Add(country11);
            continent2.Countries.Add(country38);
            mapTemplate.Continents.Add(continent2);
            var continent3 = new Continent("3", 3);
            continent3.Countries.Add(country24);
            continent3.Countries.Add(country25);
            continent3.Countries.Add(country26);
            continent3.Countries.Add(country27);
            continent3.Countries.Add(country28);
            continent3.Countries.Add(country42);
            mapTemplate.Continents.Add(continent3);
            var continent4 = new Continent("4", 5);
            continent4.Countries.Add(country18);
            continent4.Countries.Add(country19);
            continent4.Countries.Add(country20);
            continent4.Countries.Add(country21);
            continent4.Countries.Add(country22);
            continent4.Countries.Add(country23);
            continent4.Countries.Add(country36);
            continent4.Countries.Add(country37);
            mapTemplate.Continents.Add(continent4);
            var continent5 = new Continent("5", 5);
            continent5.Countries.Add(country12);
            continent5.Countries.Add(country13);
            continent5.Countries.Add(country14);
            continent5.Countries.Add(country15);
            continent5.Countries.Add(country16);
            continent5.Countries.Add(country17);
            continent5.Countries.Add(country34);
            continent5.Countries.Add(country35);
            mapTemplate.Continents.Add(continent5);
            var continent6 = new Continent("6", 6);
            continent6.Countries.Add(country1);
            continent6.Countries.Add(country2);
            continent6.Countries.Add(country3);
            continent6.Countries.Add(country4);
            continent6.Countries.Add(country5);
            continent6.Countries.Add(country6);
            continent6.Countries.Add(country7);
            continent6.Countries.Add(country40);
            continent6.Countries.Add(country41);
            continent6.Countries.Add(country43);
            mapTemplate.Continents.Add(continent6);
            mapTemplate.Connections.Add(new Connection("1", "9"));
            mapTemplate.Connections.Add(new Connection("1", "8"));
            mapTemplate.Connections.Add(new Connection("1", "7"));
            mapTemplate.Connections.Add(new Connection("1", "2"));
            mapTemplate.Connections.Add(new Connection("2", "1"));
            mapTemplate.Connections.Add(new Connection("2", "7"));
            mapTemplate.Connections.Add(new Connection("2", "43"));
            mapTemplate.Connections.Add(new Connection("3", "5"));
            mapTemplate.Connections.Add(new Connection("3", "4"));
            mapTemplate.Connections.Add(new Connection("3", "34"));
            mapTemplate.Connections.Add(new Connection("3", "43"));
            mapTemplate.Connections.Add(new Connection("4", "5"));
            mapTemplate.Connections.Add(new Connection("4", "19"));
            mapTemplate.Connections.Add(new Connection("4", "3"));
            mapTemplate.Connections.Add(new Connection("4", "41"));
            mapTemplate.Connections.Add(new Connection("4", "37"));
            mapTemplate.Connections.Add(new Connection("4", "34"));
            mapTemplate.Connections.Add(new Connection("5", "7"));
            mapTemplate.Connections.Add(new Connection("5", "6"));
            mapTemplate.Connections.Add(new Connection("5", "3"));
            mapTemplate.Connections.Add(new Connection("5", "4"));
            mapTemplate.Connections.Add(new Connection("5", "41"));
            mapTemplate.Connections.Add(new Connection("5", "43"));
            mapTemplate.Connections.Add(new Connection("6", "7"));
            mapTemplate.Connections.Add(new Connection("6", "5"));
            mapTemplate.Connections.Add(new Connection("6", "40"));
            mapTemplate.Connections.Add(new Connection("6", "41"));
            mapTemplate.Connections.Add(new Connection("7", "8"));
            mapTemplate.Connections.Add(new Connection("7", "1"));
            mapTemplate.Connections.Add(new Connection("7", "2"));
            mapTemplate.Connections.Add(new Connection("7", "5"));
            mapTemplate.Connections.Add(new Connection("7", "6"));
            mapTemplate.Connections.Add(new Connection("7", "33"));
            mapTemplate.Connections.Add(new Connection("7", "43"));
            mapTemplate.Connections.Add(new Connection("8", "9"));
            mapTemplate.Connections.Add(new Connection("8", "1"));
            mapTemplate.Connections.Add(new Connection("8", "7"));
            mapTemplate.Connections.Add(new Connection("9", "10"));
            mapTemplate.Connections.Add(new Connection("9", "8"));
            mapTemplate.Connections.Add(new Connection("9", "1"));
            mapTemplate.Connections.Add(new Connection("9", "31"));
            mapTemplate.Connections.Add(new Connection("10", "11"));
            mapTemplate.Connections.Add(new Connection("10", "9"));
            mapTemplate.Connections.Add(new Connection("10", "38"));
            mapTemplate.Connections.Add(new Connection("11", "10"));
            mapTemplate.Connections.Add(new Connection("11", "12"));
            mapTemplate.Connections.Add(new Connection("12", "11"));
            mapTemplate.Connections.Add(new Connection("12", "28"));
            mapTemplate.Connections.Add(new Connection("12", "27"));
            mapTemplate.Connections.Add(new Connection("12", "13"));
            mapTemplate.Connections.Add(new Connection("12", "15"));
            mapTemplate.Connections.Add(new Connection("12", "35"));
            mapTemplate.Connections.Add(new Connection("13", "12"));
            mapTemplate.Connections.Add(new Connection("13", "15"));
            mapTemplate.Connections.Add(new Connection("13", "14"));
            mapTemplate.Connections.Add(new Connection("13", "34"));
            mapTemplate.Connections.Add(new Connection("13", "35"));
            mapTemplate.Connections.Add(new Connection("14", "13"));
            mapTemplate.Connections.Add(new Connection("14", "15"));
            mapTemplate.Connections.Add(new Connection("14", "16"));
            mapTemplate.Connections.Add(new Connection("14", "17"));
            mapTemplate.Connections.Add(new Connection("14", "18"));
            mapTemplate.Connections.Add(new Connection("14", "19"));
            mapTemplate.Connections.Add(new Connection("14", "34"));
            mapTemplate.Connections.Add(new Connection("15", "27"));
            mapTemplate.Connections.Add(new Connection("15", "13"));
            mapTemplate.Connections.Add(new Connection("15", "14"));
            mapTemplate.Connections.Add(new Connection("15", "12"));
            mapTemplate.Connections.Add(new Connection("15", "16"));
            mapTemplate.Connections.Add(new Connection("16", "14"));
            mapTemplate.Connections.Add(new Connection("16", "25"));
            mapTemplate.Connections.Add(new Connection("16", "15"));
            mapTemplate.Connections.Add(new Connection("16", "17"));
            mapTemplate.Connections.Add(new Connection("16", "24"));
            mapTemplate.Connections.Add(new Connection("16", "27"));
            mapTemplate.Connections.Add(new Connection("17", "14"));
            mapTemplate.Connections.Add(new Connection("17", "18"));
            mapTemplate.Connections.Add(new Connection("17", "24"));
            mapTemplate.Connections.Add(new Connection("17", "16"));
            mapTemplate.Connections.Add(new Connection("18", "14"));
            mapTemplate.Connections.Add(new Connection("18", "17"));
            mapTemplate.Connections.Add(new Connection("18", "22"));
            mapTemplate.Connections.Add(new Connection("18", "19"));
            mapTemplate.Connections.Add(new Connection("19", "14"));
            mapTemplate.Connections.Add(new Connection("19", "4"));
            mapTemplate.Connections.Add(new Connection("19", "22"));
            mapTemplate.Connections.Add(new Connection("19", "37"));
            mapTemplate.Connections.Add(new Connection("19", "34"));
            mapTemplate.Connections.Add(new Connection("19", "18"));
            mapTemplate.Connections.Add(new Connection("20", "23"));
            mapTemplate.Connections.Add(new Connection("20", "21"));
            mapTemplate.Connections.Add(new Connection("20", "40"));
            mapTemplate.Connections.Add(new Connection("20", "36"));
            mapTemplate.Connections.Add(new Connection("21", "23"));
            mapTemplate.Connections.Add(new Connection("21", "20"));
            mapTemplate.Connections.Add(new Connection("21", "22"));
            mapTemplate.Connections.Add(new Connection("21", "36"));
            mapTemplate.Connections.Add(new Connection("22", "21"));
            mapTemplate.Connections.Add(new Connection("22", "18"));
            mapTemplate.Connections.Add(new Connection("22", "19"));
            mapTemplate.Connections.Add(new Connection("22", "36"));
            mapTemplate.Connections.Add(new Connection("22", "37"));
            mapTemplate.Connections.Add(new Connection("23", "20"));
            mapTemplate.Connections.Add(new Connection("23", "21"));
            mapTemplate.Connections.Add(new Connection("23", "39"));
            mapTemplate.Connections.Add(new Connection("24", "17"));
            mapTemplate.Connections.Add(new Connection("24", "26"));
            mapTemplate.Connections.Add(new Connection("24", "25"));
            mapTemplate.Connections.Add(new Connection("24", "16"));
            mapTemplate.Connections.Add(new Connection("25", "27"));
            mapTemplate.Connections.Add(new Connection("25", "26"));
            mapTemplate.Connections.Add(new Connection("25", "24"));
            mapTemplate.Connections.Add(new Connection("25", "16"));
            mapTemplate.Connections.Add(new Connection("26", "24"));
            mapTemplate.Connections.Add(new Connection("26", "27"));
            mapTemplate.Connections.Add(new Connection("26", "25"));
            mapTemplate.Connections.Add(new Connection("27", "28"));
            mapTemplate.Connections.Add(new Connection("27", "15"));
            mapTemplate.Connections.Add(new Connection("27", "12"));
            mapTemplate.Connections.Add(new Connection("27", "26"));
            mapTemplate.Connections.Add(new Connection("27", "25"));
            mapTemplate.Connections.Add(new Connection("27", "16"));
            mapTemplate.Connections.Add(new Connection("27", "42"));
            mapTemplate.Connections.Add(new Connection("28", "12"));
            mapTemplate.Connections.Add(new Connection("28", "27"));
            mapTemplate.Connections.Add(new Connection("28", "29"));
            mapTemplate.Connections.Add(new Connection("28", "42"));
            mapTemplate.Connections.Add(new Connection("29", "28"));
            mapTemplate.Connections.Add(new Connection("29", "30"));
            mapTemplate.Connections.Add(new Connection("30", "31"));
            mapTemplate.Connections.Add(new Connection("30", "29"));
            mapTemplate.Connections.Add(new Connection("31", "32"));
            mapTemplate.Connections.Add(new Connection("31", "9"));
            mapTemplate.Connections.Add(new Connection("31", "30"));
            mapTemplate.Connections.Add(new Connection("32", "33"));
            mapTemplate.Connections.Add(new Connection("32", "31"));
            mapTemplate.Connections.Add(new Connection("33", "32"));
            mapTemplate.Connections.Add(new Connection("33", "7"));
            mapTemplate.Connections.Add(new Connection("33", "39"));
            mapTemplate.Connections.Add(new Connection("34", "3"));
            mapTemplate.Connections.Add(new Connection("34", "4"));
            mapTemplate.Connections.Add(new Connection("34", "19"));
            mapTemplate.Connections.Add(new Connection("34", "14"));
            mapTemplate.Connections.Add(new Connection("34", "13"));
            mapTemplate.Connections.Add(new Connection("34", "35"));
            mapTemplate.Connections.Add(new Connection("34", "43"));
            mapTemplate.Connections.Add(new Connection("35", "34"));
            mapTemplate.Connections.Add(new Connection("35", "13"));
            mapTemplate.Connections.Add(new Connection("35", "12"));
            mapTemplate.Connections.Add(new Connection("35", "43"));
            mapTemplate.Connections.Add(new Connection("36", "40"));
            mapTemplate.Connections.Add(new Connection("36", "41"));
            mapTemplate.Connections.Add(new Connection("36", "37"));
            mapTemplate.Connections.Add(new Connection("36", "22"));
            mapTemplate.Connections.Add(new Connection("36", "21"));
            mapTemplate.Connections.Add(new Connection("36", "20"));
            mapTemplate.Connections.Add(new Connection("37", "41"));
            mapTemplate.Connections.Add(new Connection("37", "36"));
            mapTemplate.Connections.Add(new Connection("37", "19"));
            mapTemplate.Connections.Add(new Connection("37", "4"));
            mapTemplate.Connections.Add(new Connection("37", "22"));
            mapTemplate.Connections.Add(new Connection("38", "10"));
            mapTemplate.Connections.Add(new Connection("39", "23"));
            mapTemplate.Connections.Add(new Connection("39", "33"));
            mapTemplate.Connections.Add(new Connection("40", "6"));
            mapTemplate.Connections.Add(new Connection("40", "20"));
            mapTemplate.Connections.Add(new Connection("40", "36"));
            mapTemplate.Connections.Add(new Connection("40", "41"));
            mapTemplate.Connections.Add(new Connection("41", "40"));
            mapTemplate.Connections.Add(new Connection("41", "5"));
            mapTemplate.Connections.Add(new Connection("41", "4"));
            mapTemplate.Connections.Add(new Connection("41", "37"));
            mapTemplate.Connections.Add(new Connection("41", "6"));
            mapTemplate.Connections.Add(new Connection("41", "36"));
            mapTemplate.Connections.Add(new Connection("42", "27"));
            mapTemplate.Connections.Add(new Connection("42", "28"));
            mapTemplate.Connections.Add(new Connection("43", "2"));
            mapTemplate.Connections.Add(new Connection("43", "7"));
            mapTemplate.Connections.Add(new Connection("43", "5"));
            mapTemplate.Connections.Add(new Connection("43", "3"));
            mapTemplate.Connections.Add(new Connection("43", "35"));
            mapTemplate.Connections.Add(new Connection("43", "34"));

            return mapTemplate;
        }
    }
}
