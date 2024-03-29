using ImperaPlus.Domain.Map;

namespace ImperaPlus.DataAccess.ConvertedMaps
{
    public static partial class Maps
    {
        public static MapTemplate Mittelerde()
        {
            var mapTemplate = new MapTemplate("Mittelerde") { Image = "mittelerde.jpg" };
            var country1 = new CountryTemplate("1", "Forlindon") { X = 81, Y = 126 };
            mapTemplate.Countries.Add(country1);
            var country2 = new CountryTemplate("2", "Forochel") { X = 150, Y = 114 };
            mapTemplate.Countries.Add(country2);
            var country3 = new CountryTemplate("3", "Ewedim") { X = 257, Y = 117 };
            mapTemplate.Countries.Add(country3);
            var country4 = new CountryTemplate("4", "Forodwain") { X = 368, Y = 78 };
            mapTemplate.Countries.Add(country4);
            var country5 = new CountryTemplate("5", "Harlindon") { X = 145, Y = 217 };
            mapTemplate.Countries.Add(country5);
            var country6 = new CountryTemplate("6", "Minhiriath") { X = 226, Y = 206 };
            mapTemplate.Countries.Add(country6);
            var country7 = new CountryTemplate("7", "Hudaui") { X = 302, Y = 206 };
            mapTemplate.Countries.Add(country7);
            var country8 = new CountryTemplate("8", "Enedwaith") { X = 246, Y = 313 };
            mapTemplate.Countries.Add(country8);
            var country9 = new CountryTemplate("9", "Fangorn") { X = 336, Y = 308 };
            mapTemplate.Countries.Add(country9);
            var country10 = new CountryTemplate("10", "Nebelgebirge") { X = 356, Y = 219 };
            mapTemplate.Countries.Add(country10);
            var country11 = new CountryTemplate("11", "Duesterwald") { X = 417, Y = 259 };
            mapTemplate.Countries.Add(country11);
            var country12 = new CountryTemplate("12", "Esgaroth") { X = 511, Y = 180 };
            mapTemplate.Countries.Add(country12);
            var country13 = new CountryTemplate("13", "Erebor") { X = 469, Y = 129 };
            mapTemplate.Countries.Add(country13);
            var country14 = new CountryTemplate("14", "Rhanort") { X = 584, Y = 189 };
            mapTemplate.Countries.Add(country14);
            var country15 = new CountryTemplate("15", "Dagorland") { X = 523, Y = 298 };
            mapTemplate.Countries.Add(country15);
            var country16 = new CountryTemplate("16", "Rhun") { X = 636, Y = 277 };
            mapTemplate.Countries.Add(country16);
            var country17 = new CountryTemplate("17", "Barad_Dur") { X = 499, Y = 418 };
            mapTemplate.Countries.Add(country17);
            var country18 = new CountryTemplate("18", "Nurn") { X = 494, Y = 473 };
            mapTemplate.Countries.Add(country18);
            var country19 = new CountryTemplate("19", "Gorgoroth") { X = 543, Y = 437 };
            mapTemplate.Countries.Add(country19);
            var country20 = new CountryTemplate("20", "Nurn_Meer") { X = 580, Y = 476 };
            mapTemplate.Countries.Add(country20);
            var country21 = new CountryTemplate("21", "Khand") { X = 665, Y = 462 };
            mapTemplate.Countries.Add(country21);
            var country22 = new CountryTemplate("22", "Ered") { X = 667, Y = 382 };
            mapTemplate.Countries.Add(country22);
            var country23 = new CountryTemplate("23", "Minas_Tirith") { X = 416, Y = 398 };
            mapTemplate.Countries.Add(country23);
            var country24 = new CountryTemplate("24", "Ithilien") { X = 436, Y = 459 };
            mapTemplate.Countries.Add(country24);
            var country25 = new CountryTemplate("25", "Belfalas") { X = 377, Y = 466 };
            mapTemplate.Countries.Add(country25);
            var country26 = new CountryTemplate("26", "Rohan") { X = 336, Y = 356 };
            mapTemplate.Countries.Add(country26);
            var country27 = new CountryTemplate("27", "Anorien") { X = 334, Y = 396 };
            mapTemplate.Countries.Add(country27);
            var country28 = new CountryTemplate("28", "Anfalas") { X = 295, Y = 446 };
            mapTemplate.Countries.Add(country28);
            var country29 = new CountryTemplate("29", "Lefnui") { X = 217, Y = 402 };
            mapTemplate.Countries.Add(country29);
            var country30 = new CountryTemplate("30", "Udun") { X = 577, Y = 419 };
            mapTemplate.Countries.Add(country30);
            var continent1 = new Continent("1", 2);
            continent1.Countries.Add(country2);
            continent1.Countries.Add(country3);
            continent1.Countries.Add(country4);
            mapTemplate.Continents.Add(continent1);
            var continent2 = new Continent("2", 3);
            continent2.Countries.Add(country13);
            continent2.Countries.Add(country12);
            continent2.Countries.Add(country14);
            continent2.Countries.Add(country16);
            mapTemplate.Continents.Add(continent2);
            var continent3 = new Continent("3", 3);
            continent3.Countries.Add(country10);
            continent3.Countries.Add(country9);
            continent3.Countries.Add(country11);
            continent3.Countries.Add(country15);
            mapTemplate.Continents.Add(continent3);
            var continent4 = new Continent("4", 4);
            continent4.Countries.Add(country1);
            continent4.Countries.Add(country5);
            continent4.Countries.Add(country6);
            continent4.Countries.Add(country8);
            continent4.Countries.Add(country7);
            mapTemplate.Continents.Add(continent4);
            var continent5 = new Continent("5", 6);
            continent5.Countries.Add(country22);
            continent5.Countries.Add(country21);
            continent5.Countries.Add(country20);
            continent5.Countries.Add(country30);
            continent5.Countries.Add(country19);
            continent5.Countries.Add(country17);
            continent5.Countries.Add(country18);
            mapTemplate.Continents.Add(continent5);
            var continent6 = new Continent("6", 6);
            continent6.Countries.Add(country24);
            continent6.Countries.Add(country25);
            continent6.Countries.Add(country23);
            continent6.Countries.Add(country26);
            continent6.Countries.Add(country27);
            continent6.Countries.Add(country28);
            continent6.Countries.Add(country29);
            mapTemplate.Continents.Add(continent6);
            mapTemplate.Connections.Add(new Connection("15", "16"));
            mapTemplate.Connections.Add(new Connection("15", "14"));
            mapTemplate.Connections.Add(new Connection("15", "11"));
            mapTemplate.Connections.Add(new Connection("15", "23"));
            mapTemplate.Connections.Add(new Connection("15", "17"));
            mapTemplate.Connections.Add(new Connection("15", "30"));
            mapTemplate.Connections.Add(new Connection("15", "22"));
            mapTemplate.Connections.Add(new Connection("30", "15"));
            mapTemplate.Connections.Add(new Connection("30", "22"));
            mapTemplate.Connections.Add(new Connection("30", "21"));
            mapTemplate.Connections.Add(new Connection("30", "20"));
            mapTemplate.Connections.Add(new Connection("30", "19"));
            mapTemplate.Connections.Add(new Connection("30", "17"));
            mapTemplate.Connections.Add(new Connection("8", "6"));
            mapTemplate.Connections.Add(new Connection("8", "7"));
            mapTemplate.Connections.Add(new Connection("8", "10"));
            mapTemplate.Connections.Add(new Connection("8", "9"));
            mapTemplate.Connections.Add(new Connection("8", "27"));
            mapTemplate.Connections.Add(new Connection("8", "29"));
            mapTemplate.Connections.Add(new Connection("23", "11"));
            mapTemplate.Connections.Add(new Connection("23", "15"));
            mapTemplate.Connections.Add(new Connection("23", "17"));
            mapTemplate.Connections.Add(new Connection("23", "24"));
            mapTemplate.Connections.Add(new Connection("23", "25"));
            mapTemplate.Connections.Add(new Connection("23", "28"));
            mapTemplate.Connections.Add(new Connection("23", "27"));
            mapTemplate.Connections.Add(new Connection("23", "26"));
            mapTemplate.Connections.Add(new Connection("23", "9"));
            mapTemplate.Connections.Add(new Connection("16", "14"));
            mapTemplate.Connections.Add(new Connection("16", "22"));
            mapTemplate.Connections.Add(new Connection("16", "15"));
            mapTemplate.Connections.Add(new Connection("7", "3"));
            mapTemplate.Connections.Add(new Connection("7", "10"));
            mapTemplate.Connections.Add(new Connection("7", "8"));
            mapTemplate.Connections.Add(new Connection("7", "6"));
            mapTemplate.Connections.Add(new Connection("22", "16"));
            mapTemplate.Connections.Add(new Connection("22", "21"));
            mapTemplate.Connections.Add(new Connection("22", "30"));
            mapTemplate.Connections.Add(new Connection("22", "15"));
            mapTemplate.Connections.Add(new Connection("9", "10"));
            mapTemplate.Connections.Add(new Connection("9", "11"));
            mapTemplate.Connections.Add(new Connection("9", "23"));
            mapTemplate.Connections.Add(new Connection("9", "26"));
            mapTemplate.Connections.Add(new Connection("9", "8"));
            mapTemplate.Connections.Add(new Connection("21", "22"));
            mapTemplate.Connections.Add(new Connection("21", "20"));
            mapTemplate.Connections.Add(new Connection("21", "30"));
            mapTemplate.Connections.Add(new Connection("6", "3"));
            mapTemplate.Connections.Add(new Connection("6", "7"));
            mapTemplate.Connections.Add(new Connection("6", "8"));
            mapTemplate.Connections.Add(new Connection("6", "5"));
            mapTemplate.Connections.Add(new Connection("1", "2"));
            mapTemplate.Connections.Add(new Connection("1", "5"));
            mapTemplate.Connections.Add(new Connection("29", "8"));
            mapTemplate.Connections.Add(new Connection("29", "27"));
            mapTemplate.Connections.Add(new Connection("29", "28"));
            mapTemplate.Connections.Add(new Connection("14", "16"));
            mapTemplate.Connections.Add(new Connection("14", "15"));
            mapTemplate.Connections.Add(new Connection("14", "12"));
            mapTemplate.Connections.Add(new Connection("24", "23"));
            mapTemplate.Connections.Add(new Connection("24", "17"));
            mapTemplate.Connections.Add(new Connection("24", "18"));
            mapTemplate.Connections.Add(new Connection("24", "25"));
            mapTemplate.Connections.Add(new Connection("4", "13"));
            mapTemplate.Connections.Add(new Connection("4", "10"));
            mapTemplate.Connections.Add(new Connection("4", "3"));
            mapTemplate.Connections.Add(new Connection("19", "17"));
            mapTemplate.Connections.Add(new Connection("19", "30"));
            mapTemplate.Connections.Add(new Connection("19", "20"));
            mapTemplate.Connections.Add(new Connection("19", "18"));
            mapTemplate.Connections.Add(new Connection("11", "13"));
            mapTemplate.Connections.Add(new Connection("11", "12"));
            mapTemplate.Connections.Add(new Connection("11", "15"));
            mapTemplate.Connections.Add(new Connection("11", "23"));
            mapTemplate.Connections.Add(new Connection("11", "9"));
            mapTemplate.Connections.Add(new Connection("11", "10"));
            mapTemplate.Connections.Add(new Connection("18", "17"));
            mapTemplate.Connections.Add(new Connection("18", "19"));
            mapTemplate.Connections.Add(new Connection("18", "20"));
            mapTemplate.Connections.Add(new Connection("18", "24"));
            mapTemplate.Connections.Add(new Connection("3", "4"));
            mapTemplate.Connections.Add(new Connection("3", "7"));
            mapTemplate.Connections.Add(new Connection("3", "6"));
            mapTemplate.Connections.Add(new Connection("3", "2"));
            mapTemplate.Connections.Add(new Connection("12", "13"));
            mapTemplate.Connections.Add(new Connection("12", "14"));
            mapTemplate.Connections.Add(new Connection("12", "11"));
            mapTemplate.Connections.Add(new Connection("27", "29"));
            mapTemplate.Connections.Add(new Connection("27", "8"));
            mapTemplate.Connections.Add(new Connection("27", "26"));
            mapTemplate.Connections.Add(new Connection("27", "23"));
            mapTemplate.Connections.Add(new Connection("27", "28"));
            mapTemplate.Connections.Add(new Connection("17", "15"));
            mapTemplate.Connections.Add(new Connection("17", "30"));
            mapTemplate.Connections.Add(new Connection("17", "19"));
            mapTemplate.Connections.Add(new Connection("17", "18"));
            mapTemplate.Connections.Add(new Connection("17", "24"));
            mapTemplate.Connections.Add(new Connection("17", "23"));
            mapTemplate.Connections.Add(new Connection("2", "3"));
            mapTemplate.Connections.Add(new Connection("2", "5"));
            mapTemplate.Connections.Add(new Connection("2", "1"));
            mapTemplate.Connections.Add(new Connection("13", "4"));
            mapTemplate.Connections.Add(new Connection("13", "10"));
            mapTemplate.Connections.Add(new Connection("13", "11"));
            mapTemplate.Connections.Add(new Connection("13", "12"));
            mapTemplate.Connections.Add(new Connection("28", "29"));
            mapTemplate.Connections.Add(new Connection("28", "27"));
            mapTemplate.Connections.Add(new Connection("28", "23"));
            mapTemplate.Connections.Add(new Connection("28", "25"));
            mapTemplate.Connections.Add(new Connection("20", "19"));
            mapTemplate.Connections.Add(new Connection("20", "30"));
            mapTemplate.Connections.Add(new Connection("20", "21"));
            mapTemplate.Connections.Add(new Connection("20", "18"));
            mapTemplate.Connections.Add(new Connection("25", "24"));
            mapTemplate.Connections.Add(new Connection("25", "23"));
            mapTemplate.Connections.Add(new Connection("25", "28"));
            mapTemplate.Connections.Add(new Connection("10", "4"));
            mapTemplate.Connections.Add(new Connection("10", "13"));
            mapTemplate.Connections.Add(new Connection("10", "11"));
            mapTemplate.Connections.Add(new Connection("10", "9"));
            mapTemplate.Connections.Add(new Connection("10", "8"));
            mapTemplate.Connections.Add(new Connection("10", "7"));
            mapTemplate.Connections.Add(new Connection("5", "2"));
            mapTemplate.Connections.Add(new Connection("5", "6"));
            mapTemplate.Connections.Add(new Connection("5", "1"));
            mapTemplate.Connections.Add(new Connection("26", "9"));
            mapTemplate.Connections.Add(new Connection("26", "23"));
            mapTemplate.Connections.Add(new Connection("26", "27"));

            return mapTemplate;
        }
    }
}
