using ImperaPlus.Domain.Map;

namespace ImperaPlus.DataAccess.ConvertedMaps
{
    public static partial class Maps
    {
        public static MapTemplate Waterworld()
        {
            var mapTemplate = new MapTemplate("Waterworld") { Image = "waterworld.jpg" };
            var country1 = new CountryTemplate("1", "BeaufortSee") { X = 210, Y = 16 };
            mapTemplate.Countries.Add(country1);
            var country2 = new CountryTemplate("2", "HudsonBay") { X = 318, Y = 96 };
            mapTemplate.Countries.Add(country2);
            var country3 = new CountryTemplate("3", "Baffinmeer") { X = 418, Y = 44 };
            mapTemplate.Countries.Add(country3);
            var country4 = new CountryTemplate("4", "EuropNordmeer") { X = 611, Y = 50 };
            mapTemplate.Countries.Add(country4);
            var country5 = new CountryTemplate("5", "Nordsee") { X = 606, Y = 93 };
            mapTemplate.Countries.Add(country5);
            var country6 = new CountryTemplate("6", "Ostsee") { X = 669, Y = 98 };
            mapTemplate.Countries.Add(country6);
            var country7 = new CountryTemplate("7", "Barentsee") { X = 1067, Y = 17 };
            mapTemplate.Countries.Add(country7);
            var country8 = new CountryTemplate("8", "GolfvonAlaska") { X = 72, Y = 158 };
            mapTemplate.Countries.Add(country8);
            var country9 = new CountryTemplate("9", "GolfvonKalifornien") { X = 56, Y = 260 };
            mapTemplate.Countries.Add(country9);
            var country10 = new CountryTemplate("10", "GolfvonPanama") { X = 162, Y = 355 };
            mapTemplate.Countries.Add(country10);
            var country11 = new CountryTemplate("11", "Ostpazifik") { X = 61, Y = 511 };
            mapTemplate.Countries.Add(country11);
            var country12 = new CountryTemplate("12", "ChilenischeSee") { X = 236, Y = 524 };
            mapTemplate.Countries.Add(country12);
            var country13 = new CountryTemplate("13", "Beringsee") { X = 1219, Y = 138 };
            mapTemplate.Countries.Add(country13);
            var country14 = new CountryTemplate("14", "Westpazifik") { X = 1171, Y = 272 };
            mapTemplate.Countries.Add(country14);
            var country15 = new CountryTemplate("15", "Mittelpazifik") { X = 1271, Y = 251 };
            mapTemplate.Countries.Add(country15);
            var country16 = new CountryTemplate("16", "Bismarcksee") { X = 1219, Y = 360 };
            mapTemplate.Countries.Add(country16);
            var country17 = new CountryTemplate("17", "KeltischeSee") { X = 511, Y = 132 };
            mapTemplate.Countries.Add(country17);
            var country18 = new CountryTemplate("18", "Nordatlantik") { X = 465, Y = 215 };
            mapTemplate.Countries.Add(country18);
            var country19 = new CountryTemplate("19", "Westatlantik") { X = 348, Y = 241 };
            mapTemplate.Countries.Add(country19);
            var country20 = new CountryTemplate("20", "GolfvonMexiko") { X = 226, Y = 252 };
            mapTemplate.Countries.Add(country20);
            var country21 = new CountryTemplate("21", "KaribischesMeer") { X = 285, Y = 301 };
            mapTemplate.Countries.Add(country21);
            var country22 = new CountryTemplate("22", "BrasilianischesMeer") { X = 441, Y = 348 };
            mapTemplate.Countries.Add(country22);
            var country23 = new CountryTemplate("23", "GolfvonGuinea") { X = 577, Y = 415 };
            mapTemplate.Countries.Add(country23);
            var country24 = new CountryTemplate("24", "RiodelaPlata") { X = 417, Y = 544 };
            mapTemplate.Countries.Add(country24);
            var country25 = new CountryTemplate("25", "Südatlantik") { X = 565, Y = 525 };
            mapTemplate.Countries.Add(country25);
            var country26 = new CountryTemplate("26", "westlMittelmeer") { X = 628, Y = 175 };
            mapTemplate.Countries.Add(country26);
            var country27 = new CountryTemplate("27", "oestlMittelmeer") { X = 677, Y = 199 };
            mapTemplate.Countries.Add(country27);
            var country28 = new CountryTemplate("28", "SchwarzesMeer") { X = 725, Y = 157 };
            mapTemplate.Countries.Add(country28);
            var country29 = new CountryTemplate("29", "RotesMeer") { X = 750, Y = 268 };
            mapTemplate.Countries.Add(country29);
            var country30 = new CountryTemplate("30", "PersischerGolf") { X = 803, Y = 233 };
            mapTemplate.Countries.Add(country30);
            var country31 = new CountryTemplate("31", "GolfvonAden") { X = 812, Y = 301 };
            mapTemplate.Countries.Add(country31);
            var country32 = new CountryTemplate("32", "ArabischesMeer") { X = 860, Y = 315 };
            mapTemplate.Countries.Add(country32);
            var country33 = new CountryTemplate("33", "Westindik") { X = 839, Y = 399 };
            mapTemplate.Countries.Add(country33);
            var country34 = new CountryTemplate("34", "KanalvonMosambik") { X = 753, Y = 449 };
            mapTemplate.Countries.Add(country34);
            var country35 = new CountryTemplate("35", "Kapsee") { X = 759, Y = 559 };
            mapTemplate.Countries.Add(country35);
            var country36 = new CountryTemplate("36", "Suedindik") { X = 874, Y = 535 };
            mapTemplate.Countries.Add(country36);
            var country37 = new CountryTemplate("37", "Ostindik") { X = 979, Y = 432 };
            mapTemplate.Countries.Add(country37);
            var country38 = new CountryTemplate("38", "GolfvonBengalen") { X = 961, Y = 318 };
            mapTemplate.Countries.Add(country38);
            var country39 = new CountryTemplate("39", "OchotskischesMeer") { X = 1122, Y = 109 };
            mapTemplate.Countries.Add(country39);
            var country40 = new CountryTemplate("40", "JapanischesMeer") { X = 1115, Y = 177 };
            mapTemplate.Countries.Add(country40);
            var country41 = new CountryTemplate("41", "OstchinesischesMeer") { X = 1093, Y = 222 };
            mapTemplate.Countries.Add(country41);
            var country42 = new CountryTemplate("42", "SuedchinesischesMeer") { X = 1064, Y = 278 };
            mapTemplate.Countries.Add(country42);
            var country43 = new CountryTemplate("43", "GolfvonThailand") { X = 1045, Y = 345 };
            mapTemplate.Countries.Add(country43);
            var country44 = new CountryTemplate("44", "Celebes-See") { X = 1101, Y = 323 };
            mapTemplate.Countries.Add(country44);
            var country45 = new CountryTemplate("45", "Javasee") { X = 1079, Y = 392 };
            mapTemplate.Countries.Add(country45);
            var country46 = new CountryTemplate("46", "Bandasee") { X = 1139, Y = 399 };
            mapTemplate.Countries.Add(country46);
            var country47 = new CountryTemplate("47", "Korallenmeer") { X = 1200, Y = 418 };
            mapTemplate.Countries.Add(country47);
            var country48 = new CountryTemplate("48", "Salomonsee") { X = 1248, Y = 447 };
            mapTemplate.Countries.Add(country48);
            var country49 = new CountryTemplate("49", "Tasmansee") { X = 1214, Y = 547 };
            mapTemplate.Countries.Add(country49);
            var country50 = new CountryTemplate("50", "SuedlichesEismeer") { X = 1188, Y = 580 };
            mapTemplate.Countries.Add(country50);
            var country51 = new CountryTemplate("51", "GrAustralischeBucht") { X = 1085, Y = 575 };
            mapTemplate.Countries.Add(country51);
            var continent1 = new Continent("1", 4);
            continent1.Countries.Add(country1);
            continent1.Countries.Add(country2);
            continent1.Countries.Add(country3);
            continent1.Countries.Add(country4);
            continent1.Countries.Add(country5);
            continent1.Countries.Add(country6);
            continent1.Countries.Add(country7);
            mapTemplate.Continents.Add(continent1);
            var continent2 = new Continent("2", 5);
            continent2.Countries.Add(country8);
            continent2.Countries.Add(country9);
            continent2.Countries.Add(country10);
            continent2.Countries.Add(country11);
            continent2.Countries.Add(country12);
            continent2.Countries.Add(country13);
            continent2.Countries.Add(country14);
            continent2.Countries.Add(country15);
            continent2.Countries.Add(country16);
            mapTemplate.Continents.Add(continent2);
            var continent3 = new Continent("3", 4);
            continent3.Countries.Add(country45);
            continent3.Countries.Add(country46);
            continent3.Countries.Add(country47);
            continent3.Countries.Add(country48);
            continent3.Countries.Add(country49);
            continent3.Countries.Add(country50);
            continent3.Countries.Add(country51);
            mapTemplate.Continents.Add(continent3);
            var continent4 = new Continent("4", 3);
            continent4.Countries.Add(country26);
            continent4.Countries.Add(country27);
            continent4.Countries.Add(country28);
            continent4.Countries.Add(country29);
            continent4.Countries.Add(country30);
            continent4.Countries.Add(country31);
            continent4.Countries.Add(country32);
            mapTemplate.Continents.Add(continent4);
            var continent5 = new Continent("5", 5);
            continent5.Countries.Add(country17);
            continent5.Countries.Add(country18);
            continent5.Countries.Add(country19);
            continent5.Countries.Add(country20);
            continent5.Countries.Add(country21);
            continent5.Countries.Add(country22);
            continent5.Countries.Add(country23);
            continent5.Countries.Add(country24);
            continent5.Countries.Add(country25);
            mapTemplate.Continents.Add(continent5);
            var continent6 = new Continent("6", 3);
            continent6.Countries.Add(country33);
            continent6.Countries.Add(country34);
            continent6.Countries.Add(country35);
            continent6.Countries.Add(country36);
            continent6.Countries.Add(country37);
            continent6.Countries.Add(country38);
            mapTemplate.Continents.Add(continent6);
            var continent7 = new Continent("7", 3);
            continent7.Countries.Add(country39);
            continent7.Countries.Add(country40);
            continent7.Countries.Add(country41);
            continent7.Countries.Add(country42);
            continent7.Countries.Add(country43);
            continent7.Countries.Add(country44);
            mapTemplate.Continents.Add(continent7);
            mapTemplate.Connections.Add(new Connection("1", "4"));
            mapTemplate.Connections.Add(new Connection("1", "3"));
            mapTemplate.Connections.Add(new Connection("1", "2"));
            mapTemplate.Connections.Add(new Connection("1", "7"));
            mapTemplate.Connections.Add(new Connection("1", "8"));
            mapTemplate.Connections.Add(new Connection("1", "13"));
            mapTemplate.Connections.Add(new Connection("2", "1"));
            mapTemplate.Connections.Add(new Connection("2", "3"));
            mapTemplate.Connections.Add(new Connection("3", "1"));
            mapTemplate.Connections.Add(new Connection("3", "2"));
            mapTemplate.Connections.Add(new Connection("3", "17"));
            mapTemplate.Connections.Add(new Connection("3", "18"));
            mapTemplate.Connections.Add(new Connection("4", "5"));
            mapTemplate.Connections.Add(new Connection("4", "17"));
            mapTemplate.Connections.Add(new Connection("4", "1"));
            mapTemplate.Connections.Add(new Connection("4", "7"));
            mapTemplate.Connections.Add(new Connection("5", "4"));
            mapTemplate.Connections.Add(new Connection("5", "6"));
            mapTemplate.Connections.Add(new Connection("5", "17"));
            mapTemplate.Connections.Add(new Connection("6", "5"));
            mapTemplate.Connections.Add(new Connection("7", "4"));
            mapTemplate.Connections.Add(new Connection("7", "1"));
            mapTemplate.Connections.Add(new Connection("7", "13"));
            mapTemplate.Connections.Add(new Connection("8", "9"));
            mapTemplate.Connections.Add(new Connection("8", "1"));
            mapTemplate.Connections.Add(new Connection("8", "13"));
            mapTemplate.Connections.Add(new Connection("9", "10"));
            mapTemplate.Connections.Add(new Connection("9", "8"));
            mapTemplate.Connections.Add(new Connection("9", "11"));
            mapTemplate.Connections.Add(new Connection("9", "15"));
            mapTemplate.Connections.Add(new Connection("10", "21"));
            mapTemplate.Connections.Add(new Connection("10", "12"));
            mapTemplate.Connections.Add(new Connection("10", "9"));
            mapTemplate.Connections.Add(new Connection("10", "11"));
            mapTemplate.Connections.Add(new Connection("11", "9"));
            mapTemplate.Connections.Add(new Connection("11", "10"));
            mapTemplate.Connections.Add(new Connection("11", "12"));
            mapTemplate.Connections.Add(new Connection("11", "50"));
            mapTemplate.Connections.Add(new Connection("11", "48"));
            mapTemplate.Connections.Add(new Connection("11", "16"));
            mapTemplate.Connections.Add(new Connection("12", "24"));
            mapTemplate.Connections.Add(new Connection("12", "10"));
            mapTemplate.Connections.Add(new Connection("12", "11"));
            mapTemplate.Connections.Add(new Connection("13", "40"));
            mapTemplate.Connections.Add(new Connection("13", "39"));
            mapTemplate.Connections.Add(new Connection("13", "7"));
            mapTemplate.Connections.Add(new Connection("13", "14"));
            mapTemplate.Connections.Add(new Connection("13", "15"));
            mapTemplate.Connections.Add(new Connection("13", "8"));
            mapTemplate.Connections.Add(new Connection("13", "1"));
            mapTemplate.Connections.Add(new Connection("14", "40"));
            mapTemplate.Connections.Add(new Connection("14", "41"));
            mapTemplate.Connections.Add(new Connection("14", "13"));
            mapTemplate.Connections.Add(new Connection("14", "15"));
            mapTemplate.Connections.Add(new Connection("14", "16"));
            mapTemplate.Connections.Add(new Connection("14", "44"));
            mapTemplate.Connections.Add(new Connection("15", "14"));
            mapTemplate.Connections.Add(new Connection("15", "16"));
            mapTemplate.Connections.Add(new Connection("15", "13"));
            mapTemplate.Connections.Add(new Connection("15", "9"));
            mapTemplate.Connections.Add(new Connection("16", "14"));
            mapTemplate.Connections.Add(new Connection("16", "46"));
            mapTemplate.Connections.Add(new Connection("16", "48"));
            mapTemplate.Connections.Add(new Connection("16", "15"));
            mapTemplate.Connections.Add(new Connection("16", "11"));
            mapTemplate.Connections.Add(new Connection("16", "44"));
            mapTemplate.Connections.Add(new Connection("17", "5"));
            mapTemplate.Connections.Add(new Connection("17", "4"));
            mapTemplate.Connections.Add(new Connection("17", "3"));
            mapTemplate.Connections.Add(new Connection("17", "18"));
            mapTemplate.Connections.Add(new Connection("18", "3"));
            mapTemplate.Connections.Add(new Connection("18", "17"));
            mapTemplate.Connections.Add(new Connection("18", "26"));
            mapTemplate.Connections.Add(new Connection("18", "19"));
            mapTemplate.Connections.Add(new Connection("18", "22"));
            mapTemplate.Connections.Add(new Connection("19", "18"));
            mapTemplate.Connections.Add(new Connection("19", "22"));
            mapTemplate.Connections.Add(new Connection("19", "20"));
            mapTemplate.Connections.Add(new Connection("19", "21"));
            mapTemplate.Connections.Add(new Connection("20", "19"));
            mapTemplate.Connections.Add(new Connection("20", "21"));
            mapTemplate.Connections.Add(new Connection("21", "20"));
            mapTemplate.Connections.Add(new Connection("21", "19"));
            mapTemplate.Connections.Add(new Connection("21", "22"));
            mapTemplate.Connections.Add(new Connection("21", "10"));
            mapTemplate.Connections.Add(new Connection("22", "18"));
            mapTemplate.Connections.Add(new Connection("22", "19"));
            mapTemplate.Connections.Add(new Connection("22", "21"));
            mapTemplate.Connections.Add(new Connection("22", "24"));
            mapTemplate.Connections.Add(new Connection("22", "23"));
            mapTemplate.Connections.Add(new Connection("23", "22"));
            mapTemplate.Connections.Add(new Connection("23", "24"));
            mapTemplate.Connections.Add(new Connection("23", "25"));
            mapTemplate.Connections.Add(new Connection("24", "22"));
            mapTemplate.Connections.Add(new Connection("24", "23"));
            mapTemplate.Connections.Add(new Connection("24", "25"));
            mapTemplate.Connections.Add(new Connection("24", "12"));
            mapTemplate.Connections.Add(new Connection("25", "23"));
            mapTemplate.Connections.Add(new Connection("25", "24"));
            mapTemplate.Connections.Add(new Connection("25", "35"));
            mapTemplate.Connections.Add(new Connection("26", "18"));
            mapTemplate.Connections.Add(new Connection("26", "27"));
            mapTemplate.Connections.Add(new Connection("27", "26"));
            mapTemplate.Connections.Add(new Connection("27", "28"));
            mapTemplate.Connections.Add(new Connection("27", "29"));
            mapTemplate.Connections.Add(new Connection("28", "27"));
            mapTemplate.Connections.Add(new Connection("29", "27"));
            mapTemplate.Connections.Add(new Connection("29", "31"));
            mapTemplate.Connections.Add(new Connection("30", "32"));
            mapTemplate.Connections.Add(new Connection("31", "29"));
            mapTemplate.Connections.Add(new Connection("31", "32"));
            mapTemplate.Connections.Add(new Connection("31", "33"));
            mapTemplate.Connections.Add(new Connection("32", "31"));
            mapTemplate.Connections.Add(new Connection("32", "30"));
            mapTemplate.Connections.Add(new Connection("32", "33"));
            mapTemplate.Connections.Add(new Connection("33", "31"));
            mapTemplate.Connections.Add(new Connection("33", "32"));
            mapTemplate.Connections.Add(new Connection("33", "34"));
            mapTemplate.Connections.Add(new Connection("33", "36"));
            mapTemplate.Connections.Add(new Connection("33", "37"));
            mapTemplate.Connections.Add(new Connection("33", "38"));
            mapTemplate.Connections.Add(new Connection("34", "35"));
            mapTemplate.Connections.Add(new Connection("34", "33"));
            mapTemplate.Connections.Add(new Connection("35", "25"));
            mapTemplate.Connections.Add(new Connection("35", "34"));
            mapTemplate.Connections.Add(new Connection("35", "36"));
            mapTemplate.Connections.Add(new Connection("36", "35"));
            mapTemplate.Connections.Add(new Connection("36", "33"));
            mapTemplate.Connections.Add(new Connection("36", "37"));
            mapTemplate.Connections.Add(new Connection("36", "51"));
            mapTemplate.Connections.Add(new Connection("37", "36"));
            mapTemplate.Connections.Add(new Connection("37", "33"));
            mapTemplate.Connections.Add(new Connection("37", "38"));
            mapTemplate.Connections.Add(new Connection("37", "51"));
            mapTemplate.Connections.Add(new Connection("37", "46"));
            mapTemplate.Connections.Add(new Connection("37", "45"));
            mapTemplate.Connections.Add(new Connection("38", "37"));
            mapTemplate.Connections.Add(new Connection("38", "33"));
            mapTemplate.Connections.Add(new Connection("38", "43"));
            mapTemplate.Connections.Add(new Connection("39", "40"));
            mapTemplate.Connections.Add(new Connection("39", "13"));
            mapTemplate.Connections.Add(new Connection("40", "41"));
            mapTemplate.Connections.Add(new Connection("40", "39"));
            mapTemplate.Connections.Add(new Connection("40", "14"));
            mapTemplate.Connections.Add(new Connection("40", "13"));
            mapTemplate.Connections.Add(new Connection("41", "42"));
            mapTemplate.Connections.Add(new Connection("41", "40"));
            mapTemplate.Connections.Add(new Connection("41", "14"));
            mapTemplate.Connections.Add(new Connection("42", "43"));
            mapTemplate.Connections.Add(new Connection("42", "44"));
            mapTemplate.Connections.Add(new Connection("42", "41"));
            mapTemplate.Connections.Add(new Connection("43", "38"));
            mapTemplate.Connections.Add(new Connection("43", "45"));
            mapTemplate.Connections.Add(new Connection("43", "44"));
            mapTemplate.Connections.Add(new Connection("43", "42"));
            mapTemplate.Connections.Add(new Connection("44", "43"));
            mapTemplate.Connections.Add(new Connection("44", "42"));
            mapTemplate.Connections.Add(new Connection("44", "14"));
            mapTemplate.Connections.Add(new Connection("44", "46"));
            mapTemplate.Connections.Add(new Connection("44", "16"));
            mapTemplate.Connections.Add(new Connection("45", "37"));
            mapTemplate.Connections.Add(new Connection("45", "43"));
            mapTemplate.Connections.Add(new Connection("45", "46"));
            mapTemplate.Connections.Add(new Connection("46", "37"));
            mapTemplate.Connections.Add(new Connection("46", "16"));
            mapTemplate.Connections.Add(new Connection("46", "45"));
            mapTemplate.Connections.Add(new Connection("46", "44"));
            mapTemplate.Connections.Add(new Connection("46", "47"));
            mapTemplate.Connections.Add(new Connection("47", "46"));
            mapTemplate.Connections.Add(new Connection("47", "48"));
            mapTemplate.Connections.Add(new Connection("48", "47"));
            mapTemplate.Connections.Add(new Connection("48", "16"));
            mapTemplate.Connections.Add(new Connection("48", "49"));
            mapTemplate.Connections.Add(new Connection("48", "50"));
            mapTemplate.Connections.Add(new Connection("48", "11"));
            mapTemplate.Connections.Add(new Connection("49", "48"));
            mapTemplate.Connections.Add(new Connection("49", "50"));
            mapTemplate.Connections.Add(new Connection("49", "51"));
            mapTemplate.Connections.Add(new Connection("50", "48"));
            mapTemplate.Connections.Add(new Connection("50", "49"));
            mapTemplate.Connections.Add(new Connection("50", "51"));
            mapTemplate.Connections.Add(new Connection("50", "11"));
            mapTemplate.Connections.Add(new Connection("51", "36"));
            mapTemplate.Connections.Add(new Connection("51", "37"));
            mapTemplate.Connections.Add(new Connection("51", "50"));
            mapTemplate.Connections.Add(new Connection("51", "49"));

            return mapTemplate;
        }
    }
}
