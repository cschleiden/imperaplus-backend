using ImperaPlus.Domain.Map;

namespace ImperaPlus.DataAccess.ConvertedMaps
{
    public static partial class Maps
    {
        public static MapTemplate Oesterreich()
        {
            var mapTemplate = new MapTemplate("Oesterreich") { Image = "oe.gif" };
            var country1 = new CountryTemplate("1", "Bregenz") { X = 60, Y = 310 };
            mapTemplate.Countries.Add(country1);
            var country2 = new CountryTemplate("2", "Bludenz") { X = 65, Y = 360 };
            mapTemplate.Countries.Add(country2);
            var country3 = new CountryTemplate("3", "Reutte") { X = 165, Y = 300 };
            mapTemplate.Countries.Add(country3);
            var country4 = new CountryTemplate("4", "Landeck") { X = 145, Y = 375 };
            mapTemplate.Countries.Add(country4);
            var country5 = new CountryTemplate("5", "Innsbruck") { X = 225, Y = 350 };
            mapTemplate.Countries.Add(country5);
            var country6 = new CountryTemplate("6", "Schwaz") { X = 320, Y = 305 };
            mapTemplate.Countries.Add(country6);
            var country7 = new CountryTemplate("7", "Osttirol") { X = 400, Y = 400 };
            mapTemplate.Countries.Add(country7);
            var country8 = new CountryTemplate("8", "Salzburg") { X = 500, Y = 260 };
            mapTemplate.Countries.Add(country8);
            var country9 = new CountryTemplate("9", "ZellamSee") { X = 460, Y = 335 };
            mapTemplate.Countries.Add(country9);
            var country10 = new CountryTemplate("10", "Tamsweg") { X = 560, Y = 365 };
            mapTemplate.Countries.Add(country10);
            var country11 = new CountryTemplate("11", "Gmuend") { X = 505, Y = 415 };
            mapTemplate.Countries.Add(country11);
            var country12 = new CountryTemplate("12", "StVeit") { X = 620, Y = 420 };
            mapTemplate.Countries.Add(country12);
            var country13 = new CountryTemplate("13", "Klagenfurt") { X = 640, Y = 470 };
            mapTemplate.Countries.Add(country13);
            var country14 = new CountryTemplate("14", "Wolfsberg") { X = 705, Y = 410 };
            mapTemplate.Countries.Add(country14);
            var country15 = new CountryTemplate("15", "Ried") { X = 545, Y = 145 };
            mapTemplate.Countries.Add(country15);
            var country16 = new CountryTemplate("16", "Gmunden") { X = 550, Y = 210 };
            mapTemplate.Countries.Add(country16);
            var country17 = new CountryTemplate("17", "Rohrbach") { X = 620, Y = 110 };
            mapTemplate.Countries.Add(country17);
            var country18 = new CountryTemplate("18", "Linz") { X = 615, Y = 165 };
            mapTemplate.Countries.Add(country18);
            var country19 = new CountryTemplate("19", "Steyr") { X = 635, Y = 220 };
            mapTemplate.Countries.Add(country19);
            var country20 = new CountryTemplate("20", "Freistadt") { X = 685, Y = 120 };
            mapTemplate.Countries.Add(country20);
            var country21 = new CountryTemplate("21", "Schladming") { X = 595, Y = 305 };
            mapTemplate.Countries.Add(country21);
            var country22 = new CountryTemplate("22", "Zeltweg") { X = 660, Y = 355 };
            mapTemplate.Countries.Add(country22);
            var country23 = new CountryTemplate("23", "Graz") { X = 710, Y = 300 };
            mapTemplate.Countries.Add(country23);
            var country24 = new CountryTemplate("24", "Kapfenberg") { X = 765, Y = 270 };
            mapTemplate.Countries.Add(country24);
            var country25 = new CountryTemplate("25", "Leibnitz") { X = 765, Y = 435 };
            mapTemplate.Countries.Add(country25);
            var country26 = new CountryTemplate("26", "Hartberg") { X = 830, Y = 330 };
            mapTemplate.Countries.Add(country26);
            var country27 = new CountryTemplate("27", "Fuerstenfeld") { X = 865, Y = 370 };
            mapTemplate.Countries.Add(country27);
            var country28 = new CountryTemplate("28", "Scheibbs") { X = 710, Y = 200 };
            mapTemplate.Countries.Add(country28);
            var country29 = new CountryTemplate("29", "Waidhofen") { X = 740, Y = 85 };
            mapTemplate.Countries.Add(country29);
            var country30 = new CountryTemplate("30", "Melz") { X = 760, Y = 155 };
            mapTemplate.Countries.Add(country30);
            var country31 = new CountryTemplate("31", "Horn") { X = 815, Y = 60 };
            mapTemplate.Countries.Add(country31);
            var country32 = new CountryTemplate("32", "Mistelbach") { X = 930, Y = 90 };
            mapTemplate.Countries.Add(country32);
            var country33 = new CountryTemplate("33", "Klosterneuburg") { X = 850, Y = 130 };
            mapTemplate.Countries.Add(country33);
            var country34 = new CountryTemplate("34", "StPoelten") { X = 810, Y = 165 };
            mapTemplate.Countries.Add(country34);
            var country35 = new CountryTemplate("35", "WrNeustadt") { X = 810, Y = 210 };
            mapTemplate.Countries.Add(country35);
            var country36 = new CountryTemplate("36", "Neunkirchen") { X = 860, Y = 265 };
            mapTemplate.Countries.Add(country36);
            var country37 = new CountryTemplate("37", "Wien") { X = 890, Y = 170 };
            mapTemplate.Countries.Add(country37);
            var country38 = new CountryTemplate("38", "Eisenstadt") { X = 965, Y = 220 };
            mapTemplate.Countries.Add(country38);
            var country39 = new CountryTemplate("39", "Mattersburg") { X = 910, Y = 245 };
            mapTemplate.Countries.Add(country39);
            var country40 = new CountryTemplate("40", "Oberwart") { X = 890, Y = 340 };
            mapTemplate.Countries.Add(country40);
            var continent1 = new Continent("1", 1);
            continent1.Countries.Add(country1);
            continent1.Countries.Add(country2);
            mapTemplate.Continents.Add(continent1);
            var continent2 = new Continent("2", 3);
            continent2.Countries.Add(country3);
            continent2.Countries.Add(country4);
            continent2.Countries.Add(country5);
            continent2.Countries.Add(country6);
            continent2.Countries.Add(country7);
            mapTemplate.Continents.Add(continent2);
            var continent3 = new Continent("3", 2);
            continent3.Countries.Add(country8);
            continent3.Countries.Add(country9);
            continent3.Countries.Add(country10);
            mapTemplate.Continents.Add(continent3);
            var continent4 = new Continent("4", 3);
            continent4.Countries.Add(country11);
            continent4.Countries.Add(country12);
            continent4.Countries.Add(country13);
            continent4.Countries.Add(country14);
            mapTemplate.Continents.Add(continent4);
            var continent5 = new Continent("5", 5);
            continent5.Countries.Add(country15);
            continent5.Countries.Add(country16);
            continent5.Countries.Add(country17);
            continent5.Countries.Add(country18);
            continent5.Countries.Add(country19);
            continent5.Countries.Add(country20);
            mapTemplate.Continents.Add(continent5);
            var continent6 = new Continent("6", 6);
            continent6.Countries.Add(country21);
            continent6.Countries.Add(country22);
            continent6.Countries.Add(country23);
            continent6.Countries.Add(country24);
            continent6.Countries.Add(country25);
            continent6.Countries.Add(country26);
            continent6.Countries.Add(country27);
            mapTemplate.Continents.Add(continent6);
            var continent7 = new Continent("7", 7);
            continent7.Countries.Add(country28);
            continent7.Countries.Add(country29);
            continent7.Countries.Add(country30);
            continent7.Countries.Add(country31);
            continent7.Countries.Add(country32);
            continent7.Countries.Add(country33);
            continent7.Countries.Add(country34);
            continent7.Countries.Add(country35);
            continent7.Countries.Add(country36);
            mapTemplate.Continents.Add(continent7);
            var continent8 = new Continent("8", 1);
            continent8.Countries.Add(country37);
            mapTemplate.Continents.Add(continent8);
            var continent9 = new Continent("9", 2);
            continent9.Countries.Add(country38);
            continent9.Countries.Add(country39);
            continent9.Countries.Add(country40);
            mapTemplate.Continents.Add(continent9);
            mapTemplate.Connections.Add(new Connection("1", "2"));
            mapTemplate.Connections.Add(new Connection("1", "3"));
            mapTemplate.Connections.Add(new Connection("2", "1"));
            mapTemplate.Connections.Add(new Connection("2", "3"));
            mapTemplate.Connections.Add(new Connection("2", "4"));
            mapTemplate.Connections.Add(new Connection("3", "1"));
            mapTemplate.Connections.Add(new Connection("3", "2"));
            mapTemplate.Connections.Add(new Connection("3", "4"));
            mapTemplate.Connections.Add(new Connection("3", "5"));
            mapTemplate.Connections.Add(new Connection("4", "2"));
            mapTemplate.Connections.Add(new Connection("4", "3"));
            mapTemplate.Connections.Add(new Connection("4", "5"));
            mapTemplate.Connections.Add(new Connection("5", "3"));
            mapTemplate.Connections.Add(new Connection("5", "4"));
            mapTemplate.Connections.Add(new Connection("5", "6"));
            mapTemplate.Connections.Add(new Connection("6", "5"));
            mapTemplate.Connections.Add(new Connection("6", "9"));
            mapTemplate.Connections.Add(new Connection("7", "9"));
            mapTemplate.Connections.Add(new Connection("7", "11"));
            mapTemplate.Connections.Add(new Connection("8", "9"));
            mapTemplate.Connections.Add(new Connection("8", "15"));
            mapTemplate.Connections.Add(new Connection("8", "16"));
            mapTemplate.Connections.Add(new Connection("9", "6"));
            mapTemplate.Connections.Add(new Connection("9", "7"));
            mapTemplate.Connections.Add(new Connection("9", "8"));
            mapTemplate.Connections.Add(new Connection("9", "10"));
            mapTemplate.Connections.Add(new Connection("9", "11"));
            mapTemplate.Connections.Add(new Connection("9", "16"));
            mapTemplate.Connections.Add(new Connection("9", "21"));
            mapTemplate.Connections.Add(new Connection("10", "9"));
            mapTemplate.Connections.Add(new Connection("10", "11"));
            mapTemplate.Connections.Add(new Connection("10", "21"));
            mapTemplate.Connections.Add(new Connection("10", "22"));
            mapTemplate.Connections.Add(new Connection("11", "7"));
            mapTemplate.Connections.Add(new Connection("11", "9"));
            mapTemplate.Connections.Add(new Connection("11", "10"));
            mapTemplate.Connections.Add(new Connection("11", "12"));
            mapTemplate.Connections.Add(new Connection("11", "13"));
            mapTemplate.Connections.Add(new Connection("11", "22"));
            mapTemplate.Connections.Add(new Connection("12", "11"));
            mapTemplate.Connections.Add(new Connection("12", "13"));
            mapTemplate.Connections.Add(new Connection("12", "14"));
            mapTemplate.Connections.Add(new Connection("12", "22"));
            mapTemplate.Connections.Add(new Connection("13", "11"));
            mapTemplate.Connections.Add(new Connection("13", "12"));
            mapTemplate.Connections.Add(new Connection("13", "14"));
            mapTemplate.Connections.Add(new Connection("14", "12"));
            mapTemplate.Connections.Add(new Connection("14", "13"));
            mapTemplate.Connections.Add(new Connection("14", "22"));
            mapTemplate.Connections.Add(new Connection("14", "25"));
            mapTemplate.Connections.Add(new Connection("15", "8"));
            mapTemplate.Connections.Add(new Connection("15", "16"));
            mapTemplate.Connections.Add(new Connection("15", "17"));
            mapTemplate.Connections.Add(new Connection("15", "18"));
            mapTemplate.Connections.Add(new Connection("16", "8"));
            mapTemplate.Connections.Add(new Connection("16", "9"));
            mapTemplate.Connections.Add(new Connection("16", "15"));
            mapTemplate.Connections.Add(new Connection("16", "18"));
            mapTemplate.Connections.Add(new Connection("16", "19"));
            mapTemplate.Connections.Add(new Connection("16", "21"));
            mapTemplate.Connections.Add(new Connection("17", "15"));
            mapTemplate.Connections.Add(new Connection("17", "18"));
            mapTemplate.Connections.Add(new Connection("17", "20"));
            mapTemplate.Connections.Add(new Connection("18", "15"));
            mapTemplate.Connections.Add(new Connection("18", "16"));
            mapTemplate.Connections.Add(new Connection("18", "17"));
            mapTemplate.Connections.Add(new Connection("18", "19"));
            mapTemplate.Connections.Add(new Connection("18", "20"));
            mapTemplate.Connections.Add(new Connection("18", "28"));
            mapTemplate.Connections.Add(new Connection("19", "16"));
            mapTemplate.Connections.Add(new Connection("19", "18"));
            mapTemplate.Connections.Add(new Connection("19", "21"));
            mapTemplate.Connections.Add(new Connection("19", "28"));
            mapTemplate.Connections.Add(new Connection("20", "17"));
            mapTemplate.Connections.Add(new Connection("20", "18"));
            mapTemplate.Connections.Add(new Connection("20", "28"));
            mapTemplate.Connections.Add(new Connection("20", "29"));
            mapTemplate.Connections.Add(new Connection("20", "30"));
            mapTemplate.Connections.Add(new Connection("21", "9"));
            mapTemplate.Connections.Add(new Connection("21", "10"));
            mapTemplate.Connections.Add(new Connection("21", "16"));
            mapTemplate.Connections.Add(new Connection("21", "19"));
            mapTemplate.Connections.Add(new Connection("21", "22"));
            mapTemplate.Connections.Add(new Connection("21", "23"));
            mapTemplate.Connections.Add(new Connection("21", "24"));
            mapTemplate.Connections.Add(new Connection("21", "28"));
            mapTemplate.Connections.Add(new Connection("22", "10"));
            mapTemplate.Connections.Add(new Connection("22", "11"));
            mapTemplate.Connections.Add(new Connection("22", "12"));
            mapTemplate.Connections.Add(new Connection("22", "14"));
            mapTemplate.Connections.Add(new Connection("22", "21"));
            mapTemplate.Connections.Add(new Connection("22", "23"));
            mapTemplate.Connections.Add(new Connection("22", "25"));
            mapTemplate.Connections.Add(new Connection("23", "21"));
            mapTemplate.Connections.Add(new Connection("23", "22"));
            mapTemplate.Connections.Add(new Connection("23", "24"));
            mapTemplate.Connections.Add(new Connection("23", "25"));
            mapTemplate.Connections.Add(new Connection("23", "26"));
            mapTemplate.Connections.Add(new Connection("24", "21"));
            mapTemplate.Connections.Add(new Connection("24", "23"));
            mapTemplate.Connections.Add(new Connection("24", "26"));
            mapTemplate.Connections.Add(new Connection("24", "28"));
            mapTemplate.Connections.Add(new Connection("24", "35"));
            mapTemplate.Connections.Add(new Connection("24", "36"));
            mapTemplate.Connections.Add(new Connection("25", "14"));
            mapTemplate.Connections.Add(new Connection("25", "22"));
            mapTemplate.Connections.Add(new Connection("25", "23"));
            mapTemplate.Connections.Add(new Connection("25", "26"));
            mapTemplate.Connections.Add(new Connection("25", "27"));
            mapTemplate.Connections.Add(new Connection("25", "40"));
            mapTemplate.Connections.Add(new Connection("26", "23"));
            mapTemplate.Connections.Add(new Connection("26", "25"));
            mapTemplate.Connections.Add(new Connection("26", "27"));
            mapTemplate.Connections.Add(new Connection("26", "36"));
            mapTemplate.Connections.Add(new Connection("26", "40"));
            mapTemplate.Connections.Add(new Connection("26", "24"));
            mapTemplate.Connections.Add(new Connection("27", "25"));
            mapTemplate.Connections.Add(new Connection("27", "26"));
            mapTemplate.Connections.Add(new Connection("27", "40"));
            mapTemplate.Connections.Add(new Connection("28", "18"));
            mapTemplate.Connections.Add(new Connection("28", "19"));
            mapTemplate.Connections.Add(new Connection("28", "20"));
            mapTemplate.Connections.Add(new Connection("28", "21"));
            mapTemplate.Connections.Add(new Connection("28", "24"));
            mapTemplate.Connections.Add(new Connection("28", "30"));
            mapTemplate.Connections.Add(new Connection("28", "34"));
            mapTemplate.Connections.Add(new Connection("28", "35"));
            mapTemplate.Connections.Add(new Connection("29", "20"));
            mapTemplate.Connections.Add(new Connection("29", "30"));
            mapTemplate.Connections.Add(new Connection("29", "31"));
            mapTemplate.Connections.Add(new Connection("30", "20"));
            mapTemplate.Connections.Add(new Connection("30", "28"));
            mapTemplate.Connections.Add(new Connection("30", "29"));
            mapTemplate.Connections.Add(new Connection("30", "31"));
            mapTemplate.Connections.Add(new Connection("30", "33"));
            mapTemplate.Connections.Add(new Connection("30", "34"));
            mapTemplate.Connections.Add(new Connection("31", "29"));
            mapTemplate.Connections.Add(new Connection("31", "30"));
            mapTemplate.Connections.Add(new Connection("31", "32"));
            mapTemplate.Connections.Add(new Connection("31", "33"));
            mapTemplate.Connections.Add(new Connection("32", "31"));
            mapTemplate.Connections.Add(new Connection("32", "33"));
            mapTemplate.Connections.Add(new Connection("32", "34"));
            mapTemplate.Connections.Add(new Connection("32", "37"));
            mapTemplate.Connections.Add(new Connection("32", "38"));
            mapTemplate.Connections.Add(new Connection("33", "30"));
            mapTemplate.Connections.Add(new Connection("33", "31"));
            mapTemplate.Connections.Add(new Connection("33", "32"));
            mapTemplate.Connections.Add(new Connection("33", "34"));
            mapTemplate.Connections.Add(new Connection("33", "37"));
            mapTemplate.Connections.Add(new Connection("34", "28"));
            mapTemplate.Connections.Add(new Connection("34", "30"));
            mapTemplate.Connections.Add(new Connection("34", "32"));
            mapTemplate.Connections.Add(new Connection("34", "33"));
            mapTemplate.Connections.Add(new Connection("34", "35"));
            mapTemplate.Connections.Add(new Connection("34", "37"));
            mapTemplate.Connections.Add(new Connection("34", "38"));
            mapTemplate.Connections.Add(new Connection("35", "24"));
            mapTemplate.Connections.Add(new Connection("35", "28"));
            mapTemplate.Connections.Add(new Connection("35", "34"));
            mapTemplate.Connections.Add(new Connection("35", "36"));
            mapTemplate.Connections.Add(new Connection("35", "38"));
            mapTemplate.Connections.Add(new Connection("35", "39"));
            mapTemplate.Connections.Add(new Connection("36", "24"));
            mapTemplate.Connections.Add(new Connection("36", "26"));
            mapTemplate.Connections.Add(new Connection("36", "35"));
            mapTemplate.Connections.Add(new Connection("36", "39"));
            mapTemplate.Connections.Add(new Connection("36", "40"));
            mapTemplate.Connections.Add(new Connection("37", "32"));
            mapTemplate.Connections.Add(new Connection("37", "33"));
            mapTemplate.Connections.Add(new Connection("37", "34"));
            mapTemplate.Connections.Add(new Connection("38", "32"));
            mapTemplate.Connections.Add(new Connection("38", "34"));
            mapTemplate.Connections.Add(new Connection("38", "35"));
            mapTemplate.Connections.Add(new Connection("38", "39"));
            mapTemplate.Connections.Add(new Connection("39", "35"));
            mapTemplate.Connections.Add(new Connection("39", "36"));
            mapTemplate.Connections.Add(new Connection("39", "38"));
            mapTemplate.Connections.Add(new Connection("39", "40"));
            mapTemplate.Connections.Add(new Connection("40", "25"));
            mapTemplate.Connections.Add(new Connection("40", "26"));
            mapTemplate.Connections.Add(new Connection("40", "27"));
            mapTemplate.Connections.Add(new Connection("40", "36"));
            mapTemplate.Connections.Add(new Connection("40", "39"));

            return mapTemplate;
        }
    }
}
