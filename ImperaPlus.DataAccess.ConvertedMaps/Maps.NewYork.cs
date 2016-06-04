
using ImperaPlus.Domain.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.DataAccess.ConvertedMaps
{
    public static partial class Maps
    {
        public static MapTemplate NewYork()
        {

var mapTemplate = new MapTemplate("NewYork") { Image = "newyork.jpg" };
var country1 = new CountryTemplate("1", "Tottenville") { X = 168, Y = 1016 };
mapTemplate.Countries.Add(country1);
var country2 = new CountryTemplate("2", "Woodrow") { X = 184, Y = 940 };
mapTemplate.Countries.Add(country2);
var country3 = new CountryTemplate("3", "Great Kills") { X = 248, Y = 955 };
mapTemplate.Countries.Add(country3);
var country4 = new CountryTemplate("4", "Fresh Kills") { X = 184, Y = 839 };
mapTemplate.Countries.Add(country4);
var country5 = new CountryTemplate("5", "Newspringville") { X = 236, Y = 873 };
mapTemplate.Countries.Add(country5);
var country6 = new CountryTemplate("6", "Southbeach") { X = 322, Y = 873 };
mapTemplate.Countries.Add(country6);
var country7 = new CountryTemplate("7", "St. George") { X = 309, Y = 752 };
mapTemplate.Countries.Add(country7);
var country8 = new CountryTemplate("8", "Port Richmond") { X = 206, Y = 741 };
mapTemplate.Countries.Add(country8);
var country9 = new CountryTemplate("9", "Coney Island") { X = 514, Y = 852 };
mapTemplate.Countries.Add(country9);
var country10 = new CountryTemplate("10", "Sheepshead Bay") { X = 550, Y = 810 };
mapTemplate.Countries.Add(country10);
var country11 = new CountryTemplate("11", "Bensonhurst") { X = 467, Y = 807 };
mapTemplate.Countries.Add(country11);
var country12 = new CountryTemplate("12", "Bay Ridge") { X = 428, Y = 772 };
mapTemplate.Countries.Add(country12);
var country13 = new CountryTemplate("13", "Borough Park") { X = 503, Y = 730 };
mapTemplate.Countries.Add(country13);
var country14 = new CountryTemplate("14", "Sunset Park") { X = 480, Y = 680 };
mapTemplate.Countries.Add(country14);
var country15 = new CountryTemplate("15", "Park Slope") { X = 465, Y = 632 };
mapTemplate.Countries.Add(country15);
var country16 = new CountryTemplate("16", "Flatbush") { X = 543, Y = 743 };
mapTemplate.Countries.Add(country16);
var country17 = new CountryTemplate("17", "Wingate") { X = 566, Y = 671 };
mapTemplate.Countries.Add(country17);
var country18 = new CountryTemplate("18", "Rugby") { X = 602, Y = 700 };
mapTemplate.Countries.Add(country18);
var country19 = new CountryTemplate("19", "Crown Heights") { X = 584, Y = 650 };
mapTemplate.Countries.Add(country19);
var country20 = new CountryTemplate("20", "Canarsie") { X = 623, Y = 790 };
mapTemplate.Countries.Add(country20);
var country21 = new CountryTemplate("21", "Ocean Hill") { X = 626, Y = 648 };
mapTemplate.Countries.Add(country21);
var country22 = new CountryTemplate("22", "East New York") { X = 680, Y = 683 };
mapTemplate.Countries.Add(country22);
var country23 = new CountryTemplate("23", "Fort Greene") { X = 511, Y = 594 };
mapTemplate.Countries.Add(country23);
var country24 = new CountryTemplate("24", "Bedford") { X = 568, Y = 620 };
mapTemplate.Countries.Add(country24);
var country25 = new CountryTemplate("25", "Bushwick") { X = 601, Y = 590 };
mapTemplate.Countries.Add(country25);
var country26 = new CountryTemplate("26", "Greenpoint") { X = 563, Y = 558 };
mapTemplate.Countries.Add(country26);
var country27 = new CountryTemplate("27", "Seaside") { X = 668, Y = 897 };
mapTemplate.Countries.Add(country27);
var country28 = new CountryTemplate("28", "Rockaways") { X = 896, Y = 815 };
mapTemplate.Countries.Add(country28);
var country29 = new CountryTemplate("29", "John F. Kennedy Airport") { X = 877, Y = 725 };
mapTemplate.Countries.Add(country29);
var country30 = new CountryTemplate("30", "Rosedale") { X = 916, Y = 652 };
mapTemplate.Countries.Add(country30);
var country31 = new CountryTemplate("31", "St. Albans") { X = 844, Y = 591 };
mapTemplate.Countries.Add(country31);
var country32 = new CountryTemplate("32", "Ozone Park") { X = 773, Y = 654 };
mapTemplate.Countries.Add(country32);
var country33 = new CountryTemplate("33", "Woodhaven") { X = 730, Y = 588 };
mapTemplate.Countries.Add(country33);
var country34 = new CountryTemplate("34", "Forest Hills") { X = 722, Y = 527 };
mapTemplate.Countries.Add(country34);
var country35 = new CountryTemplate("35", "Glendale") { X = 663, Y = 564 };
mapTemplate.Countries.Add(country35);
var country36 = new CountryTemplate("36", "Fresh Meadows") { X = 786, Y = 522 };
mapTemplate.Countries.Add(country36);
var country37 = new CountryTemplate("37", "Queens Village") { X = 918, Y = 523 };
mapTemplate.Countries.Add(country37);
var country38 = new CountryTemplate("38", "Bayside") { X = 866, Y = 456 };
mapTemplate.Countries.Add(country38);
var country39 = new CountryTemplate("39", "Flushing") { X = 797, Y = 430 };
mapTemplate.Countries.Add(country39);
var country40 = new CountryTemplate("40", "South Corona") { X = 690, Y = 489 };
mapTemplate.Countries.Add(country40);
var country41 = new CountryTemplate("41", "North Corona") { X = 681, Y = 444 };
mapTemplate.Countries.Add(country41);
var country42 = new CountryTemplate("42", "Sunnyside") { X = 604, Y = 509 };
mapTemplate.Countries.Add(country42);
var country43 = new CountryTemplate("43", "Astoria") { X = 604, Y = 444 };
mapTemplate.Countries.Add(country43);
var country44 = new CountryTemplate("44", "Port Morris") { X = 576, Y = 323 };
mapTemplate.Countries.Add(country44);
var country45 = new CountryTemplate("45", "Hunts Point") { X = 637, Y = 300 };
mapTemplate.Countries.Add(country45);
var country46 = new CountryTemplate("46", "Morrisania") { X = 603, Y = 277 };
mapTemplate.Countries.Add(country46);
var country47 = new CountryTemplate("47", "Highbridge") { X = 537, Y = 298 };
mapTemplate.Countries.Add(country47);
var country48 = new CountryTemplate("48", "Soundview") { X = 693, Y = 258 };
mapTemplate.Countries.Add(country48);
var country49 = new CountryTemplate("49", "Union Port") { X = 727, Y = 255 };
mapTemplate.Countries.Add(country49);
var country50 = new CountryTemplate("50", "Throgs Neck") { X = 794, Y = 241 };
mapTemplate.Countries.Add(country50);
var country51 = new CountryTemplate("51", "University") { X = 547, Y = 242 };
mapTemplate.Countries.Add(country51);
var country52 = new CountryTemplate("52", "Tremont") { X = 587, Y = 238 };
mapTemplate.Countries.Add(country52);
var country53 = new CountryTemplate("53", "East Tremont") { X = 627, Y = 231 };
mapTemplate.Countries.Add(country53);
var country54 = new CountryTemplate("54", "Bedford Park") { X = 637, Y = 152 };
mapTemplate.Countries.Add(country54);
var country55 = new CountryTemplate("55", "MorrisPark") { X = 702, Y = 210 };
mapTemplate.Countries.Add(country55);
var country56 = new CountryTemplate("56", "Westchester") { X = 755, Y = 199 };
mapTemplate.Countries.Add(country56);
var country57 = new CountryTemplate("57", "Country Club") { X = 800, Y = 184 };
mapTemplate.Countries.Add(country57);
var country58 = new CountryTemplate("58", "Riverdale") { X = 549, Y = 126 };
mapTemplate.Countries.Add(country58);
var country59 = new CountryTemplate("59", "Woodlawn") { X = 608, Y = 119 };
mapTemplate.Countries.Add(country59);
var country60 = new CountryTemplate("60", "Kings Bridge") { X = 584, Y = 154 };
mapTemplate.Countries.Add(country60);
var country61 = new CountryTemplate("61", "Wakefield") { X = 734, Y = 101 };
mapTemplate.Countries.Add(country61);
var country62 = new CountryTemplate("62", "Williamsbridge") { X = 671, Y = 125 };
mapTemplate.Countries.Add(country62);
var country63 = new CountryTemplate("63", "Eastchester") { X = 819, Y = 120 };
mapTemplate.Countries.Add(country63);
var country64 = new CountryTemplate("64", "LibertyIsland") { X = 257, Y = 657 };
mapTemplate.Countries.Add(country64);
var country65 = new CountryTemplate("65", "Governors") { X = 379, Y = 646 };
mapTemplate.Countries.Add(country65);
var country66 = new CountryTemplate("66", "Financial District") { X = 329, Y = 618 };
mapTemplate.Countries.Add(country66);
var country67 = new CountryTemplate("67", "Soho") { X = 341, Y = 558 };
mapTemplate.Countries.Add(country67);
var country68 = new CountryTemplate("68", "Chinatown") { X = 403, Y = 568 };
mapTemplate.Countries.Add(country68);
var country69 = new CountryTemplate("69", "Chelsea") { X = 333, Y = 501 };
mapTemplate.Countries.Add(country69);
var country70 = new CountryTemplate("70", "Times Square") { X = 389, Y = 491 };
mapTemplate.Countries.Add(country70);
var country71 = new CountryTemplate("71", "Upper West Side") { X = 391, Y = 408 };
mapTemplate.Countries.Add(country71);
var country72 = new CountryTemplate("72", "Central Park") { X = 441, Y = 404 };
mapTemplate.Countries.Add(country72);
var country73 = new CountryTemplate("73", "Midtown") { X = 420, Y = 525 };
mapTemplate.Countries.Add(country73);
var country74 = new CountryTemplate("74", "Upper East Side") { X = 470, Y = 435 };
mapTemplate.Countries.Add(country74);
var country75 = new CountryTemplate("75", "Roosevelt") { X = 495, Y = 482 };
mapTemplate.Countries.Add(country75);
var country76 = new CountryTemplate("76", "Morningside") { X = 446, Y = 313 };
mapTemplate.Countries.Add(country76);
var country77 = new CountryTemplate("77", "Washington") { X = 510, Y = 209 };
mapTemplate.Countries.Add(country77);
var country78 = new CountryTemplate("78", "Harlem") { X = 479, Y = 339 };
mapTemplate.Countries.Add(country78);
var country79 = new CountryTemplate("79", "East Harlem") { X = 509, Y = 368 };
mapTemplate.Countries.Add(country79);
var country80 = new CountryTemplate("80", "Island Park") { X = 560, Y = 389 };
mapTemplate.Countries.Add(country80);
var continent1 = new Continent("1", 1);
continent1.Countries.Add(country1);
continent1.Countries.Add(country2);
continent1.Countries.Add(country3);
mapTemplate.Continents.Add(continent1);
var continent2 = new Continent("2", 3);
continent2.Countries.Add(country4);
continent2.Countries.Add(country5);
continent2.Countries.Add(country6);
continent2.Countries.Add(country7);
continent2.Countries.Add(country8);
mapTemplate.Continents.Add(continent2);
var continent3 = new Continent("3", 3);
continent3.Countries.Add(country9);
continent3.Countries.Add(country10);
continent3.Countries.Add(country11);
continent3.Countries.Add(country12);
mapTemplate.Continents.Add(continent3);
var continent4 = new Continent("4", 2);
continent4.Countries.Add(country13);
continent4.Countries.Add(country14);
continent4.Countries.Add(country15);
mapTemplate.Continents.Add(continent4);
var continent5 = new Continent("5", 2);
continent5.Countries.Add(country20);
continent5.Countries.Add(country21);
continent5.Countries.Add(country22);
mapTemplate.Continents.Add(continent5);
var continent6 = new Continent("6", 4);
continent6.Countries.Add(country16);
continent6.Countries.Add(country17);
continent6.Countries.Add(country18);
continent6.Countries.Add(country19);
mapTemplate.Continents.Add(continent6);
var continent7 = new Continent("7", 3);
continent7.Countries.Add(country23);
continent7.Countries.Add(country24);
continent7.Countries.Add(country25);
continent7.Countries.Add(country26);
mapTemplate.Continents.Add(continent7);
var continent8 = new Continent("8", 1);
continent8.Countries.Add(country27);
continent8.Countries.Add(country28);
mapTemplate.Continents.Add(continent8);
var continent9 = new Continent("9", 3);
continent9.Countries.Add(country29);
continent9.Countries.Add(country30);
continent9.Countries.Add(country31);
continent9.Countries.Add(country32);
mapTemplate.Continents.Add(continent9);
var continent10 = new Continent("10", 2);
continent10.Countries.Add(country33);
continent10.Countries.Add(country34);
continent10.Countries.Add(country35);
mapTemplate.Continents.Add(continent10);
var continent11 = new Continent("11", 4);
continent11.Countries.Add(country36);
continent11.Countries.Add(country37);
continent11.Countries.Add(country38);
continent11.Countries.Add(country39);
mapTemplate.Continents.Add(continent11);
var continent12 = new Continent("12", 3);
continent12.Countries.Add(country40);
continent12.Countries.Add(country41);
continent12.Countries.Add(country42);
continent12.Countries.Add(country43);
mapTemplate.Continents.Add(continent12);
var continent13 = new Continent("13", 2);
continent13.Countries.Add(country44);
continent13.Countries.Add(country45);
continent13.Countries.Add(country46);
continent13.Countries.Add(country47);
mapTemplate.Continents.Add(continent13);
var continent14 = new Continent("14", 2);
continent14.Countries.Add(country48);
continent14.Countries.Add(country49);
continent14.Countries.Add(country50);
mapTemplate.Continents.Add(continent14);
var continent15 = new Continent("15", 4);
continent15.Countries.Add(country51);
continent15.Countries.Add(country52);
continent15.Countries.Add(country53);
continent15.Countries.Add(country54);
mapTemplate.Continents.Add(continent15);
var continent16 = new Continent("16", 2);
continent16.Countries.Add(country55);
continent16.Countries.Add(country56);
continent16.Countries.Add(country57);
mapTemplate.Continents.Add(continent16);
var continent17 = new Continent("17", 2);
continent17.Countries.Add(country58);
continent17.Countries.Add(country59);
continent17.Countries.Add(country60);
mapTemplate.Continents.Add(continent17);
var continent18 = new Continent("18", 2);
continent18.Countries.Add(country61);
continent18.Countries.Add(country62);
continent18.Countries.Add(country63);
mapTemplate.Continents.Add(continent18);
var continent19 = new Continent("19", 3);
continent19.Countries.Add(country64);
continent19.Countries.Add(country65);
continent19.Countries.Add(country66);
continent19.Countries.Add(country67);
continent19.Countries.Add(country68);
mapTemplate.Continents.Add(continent19);
var continent20 = new Continent("20", 4);
continent20.Countries.Add(country69);
continent20.Countries.Add(country70);
continent20.Countries.Add(country71);
continent20.Countries.Add(country72);
mapTemplate.Continents.Add(continent20);
var continent21 = new Continent("21", 2);
continent21.Countries.Add(country73);
continent21.Countries.Add(country74);
continent21.Countries.Add(country75);
mapTemplate.Continents.Add(continent21);
var continent22 = new Continent("22", 2);
continent22.Countries.Add(country76);
continent22.Countries.Add(country77);
continent22.Countries.Add(country78);
mapTemplate.Continents.Add(continent22);
var continent23 = new Continent("23", 1);
continent23.Countries.Add(country79);
continent23.Countries.Add(country80);
mapTemplate.Continents.Add(continent23);
mapTemplate.Connections.Add(new Connection("1", "2"));
mapTemplate.Connections.Add(new Connection("1", "3"));
mapTemplate.Connections.Add(new Connection("2", "1"));
mapTemplate.Connections.Add(new Connection("2", "3"));
mapTemplate.Connections.Add(new Connection("2", "4"));
mapTemplate.Connections.Add(new Connection("2", "5"));
mapTemplate.Connections.Add(new Connection("3", "1"));
mapTemplate.Connections.Add(new Connection("3", "2"));
mapTemplate.Connections.Add(new Connection("3", "5"));
mapTemplate.Connections.Add(new Connection("3", "6"));
mapTemplate.Connections.Add(new Connection("3", "9"));
mapTemplate.Connections.Add(new Connection("3", "27"));
mapTemplate.Connections.Add(new Connection("4", "2"));
mapTemplate.Connections.Add(new Connection("4", "5"));
mapTemplate.Connections.Add(new Connection("4", "8"));
mapTemplate.Connections.Add(new Connection("5", "2"));
mapTemplate.Connections.Add(new Connection("5", "3"));
mapTemplate.Connections.Add(new Connection("5", "4"));
mapTemplate.Connections.Add(new Connection("5", "6"));
mapTemplate.Connections.Add(new Connection("5", "7"));
mapTemplate.Connections.Add(new Connection("5", "8"));
mapTemplate.Connections.Add(new Connection("5", "18"));
mapTemplate.Connections.Add(new Connection("5", "70"));
mapTemplate.Connections.Add(new Connection("6", "3"));
mapTemplate.Connections.Add(new Connection("6", "5"));
mapTemplate.Connections.Add(new Connection("6", "7"));
mapTemplate.Connections.Add(new Connection("6", "9"));
mapTemplate.Connections.Add(new Connection("7", "5"));
mapTemplate.Connections.Add(new Connection("7", "6"));
mapTemplate.Connections.Add(new Connection("7", "8"));
mapTemplate.Connections.Add(new Connection("7", "12"));
mapTemplate.Connections.Add(new Connection("8", "4"));
mapTemplate.Connections.Add(new Connection("8", "5"));
mapTemplate.Connections.Add(new Connection("8", "7"));
mapTemplate.Connections.Add(new Connection("8", "29"));
mapTemplate.Connections.Add(new Connection("8", "41"));
mapTemplate.Connections.Add(new Connection("8", "64"));
mapTemplate.Connections.Add(new Connection("9", "3"));
mapTemplate.Connections.Add(new Connection("9", "6"));
mapTemplate.Connections.Add(new Connection("9", "10"));
mapTemplate.Connections.Add(new Connection("9", "11"));
mapTemplate.Connections.Add(new Connection("9", "27"));
mapTemplate.Connections.Add(new Connection("9", "65"));
mapTemplate.Connections.Add(new Connection("10", "9"));
mapTemplate.Connections.Add(new Connection("10", "11"));
mapTemplate.Connections.Add(new Connection("10", "13"));
mapTemplate.Connections.Add(new Connection("10", "16"));
mapTemplate.Connections.Add(new Connection("10", "20"));
mapTemplate.Connections.Add(new Connection("10", "27"));
mapTemplate.Connections.Add(new Connection("11", "9"));
mapTemplate.Connections.Add(new Connection("11", "10"));
mapTemplate.Connections.Add(new Connection("11", "12"));
mapTemplate.Connections.Add(new Connection("11", "13"));
mapTemplate.Connections.Add(new Connection("12", "7"));
mapTemplate.Connections.Add(new Connection("12", "11"));
mapTemplate.Connections.Add(new Connection("12", "13"));
mapTemplate.Connections.Add(new Connection("12", "14"));
mapTemplate.Connections.Add(new Connection("13", "10"));
mapTemplate.Connections.Add(new Connection("13", "11"));
mapTemplate.Connections.Add(new Connection("13", "12"));
mapTemplate.Connections.Add(new Connection("13", "14"));
mapTemplate.Connections.Add(new Connection("13", "16"));
mapTemplate.Connections.Add(new Connection("14", "12"));
mapTemplate.Connections.Add(new Connection("14", "13"));
mapTemplate.Connections.Add(new Connection("14", "15"));
mapTemplate.Connections.Add(new Connection("14", "16"));
mapTemplate.Connections.Add(new Connection("14", "65"));
mapTemplate.Connections.Add(new Connection("15", "14"));
mapTemplate.Connections.Add(new Connection("15", "16"));
mapTemplate.Connections.Add(new Connection("15", "23"));
mapTemplate.Connections.Add(new Connection("15", "68"));
mapTemplate.Connections.Add(new Connection("15", "19"));
mapTemplate.Connections.Add(new Connection("16", "10"));
mapTemplate.Connections.Add(new Connection("16", "13"));
mapTemplate.Connections.Add(new Connection("16", "14"));
mapTemplate.Connections.Add(new Connection("16", "15"));
mapTemplate.Connections.Add(new Connection("16", "17"));
mapTemplate.Connections.Add(new Connection("16", "18"));
mapTemplate.Connections.Add(new Connection("16", "19"));
mapTemplate.Connections.Add(new Connection("16", "20"));
mapTemplate.Connections.Add(new Connection("17", "16"));
mapTemplate.Connections.Add(new Connection("17", "19"));
mapTemplate.Connections.Add(new Connection("17", "18"));
mapTemplate.Connections.Add(new Connection("18", "5"));
mapTemplate.Connections.Add(new Connection("18", "16"));
mapTemplate.Connections.Add(new Connection("18", "17"));
mapTemplate.Connections.Add(new Connection("18", "19"));
mapTemplate.Connections.Add(new Connection("18", "20"));
mapTemplate.Connections.Add(new Connection("18", "21"));
mapTemplate.Connections.Add(new Connection("18", "27"));
mapTemplate.Connections.Add(new Connection("18", "36"));
mapTemplate.Connections.Add(new Connection("18", "70"));
mapTemplate.Connections.Add(new Connection("18", "27"));
mapTemplate.Connections.Add(new Connection("19", "15"));
mapTemplate.Connections.Add(new Connection("19", "16"));
mapTemplate.Connections.Add(new Connection("19", "17"));
mapTemplate.Connections.Add(new Connection("19", "21"));
mapTemplate.Connections.Add(new Connection("19", "23"));
mapTemplate.Connections.Add(new Connection("19", "24"));
mapTemplate.Connections.Add(new Connection("19", "18"));
mapTemplate.Connections.Add(new Connection("20", "10"));
mapTemplate.Connections.Add(new Connection("20", "16"));
mapTemplate.Connections.Add(new Connection("20", "18"));
mapTemplate.Connections.Add(new Connection("20", "21"));
mapTemplate.Connections.Add(new Connection("20", "22"));
mapTemplate.Connections.Add(new Connection("20", "27"));
mapTemplate.Connections.Add(new Connection("21", "18"));
mapTemplate.Connections.Add(new Connection("21", "19"));
mapTemplate.Connections.Add(new Connection("21", "20"));
mapTemplate.Connections.Add(new Connection("21", "22"));
mapTemplate.Connections.Add(new Connection("21", "24"));
mapTemplate.Connections.Add(new Connection("21", "25"));
mapTemplate.Connections.Add(new Connection("22", "20"));
mapTemplate.Connections.Add(new Connection("22", "21"));
mapTemplate.Connections.Add(new Connection("22", "25"));
mapTemplate.Connections.Add(new Connection("22", "32"));
mapTemplate.Connections.Add(new Connection("22", "33"));
mapTemplate.Connections.Add(new Connection("22", "35"));
mapTemplate.Connections.Add(new Connection("23", "15"));
mapTemplate.Connections.Add(new Connection("23", "19"));
mapTemplate.Connections.Add(new Connection("23", "24"));
mapTemplate.Connections.Add(new Connection("23", "26"));
mapTemplate.Connections.Add(new Connection("24", "19"));
mapTemplate.Connections.Add(new Connection("24", "21"));
mapTemplate.Connections.Add(new Connection("24", "23"));
mapTemplate.Connections.Add(new Connection("24", "25"));
mapTemplate.Connections.Add(new Connection("24", "26"));
mapTemplate.Connections.Add(new Connection("25", "21"));
mapTemplate.Connections.Add(new Connection("25", "22"));
mapTemplate.Connections.Add(new Connection("25", "24"));
mapTemplate.Connections.Add(new Connection("25", "26"));
mapTemplate.Connections.Add(new Connection("25", "35"));
mapTemplate.Connections.Add(new Connection("26", "23"));
mapTemplate.Connections.Add(new Connection("26", "24"));
mapTemplate.Connections.Add(new Connection("26", "25"));
mapTemplate.Connections.Add(new Connection("26", "35"));
mapTemplate.Connections.Add(new Connection("26", "42"));
mapTemplate.Connections.Add(new Connection("26", "75"));
mapTemplate.Connections.Add(new Connection("27", "9"));
mapTemplate.Connections.Add(new Connection("27", "10"));
mapTemplate.Connections.Add(new Connection("27", "20"));
mapTemplate.Connections.Add(new Connection("27", "28"));
mapTemplate.Connections.Add(new Connection("27", "20"));
mapTemplate.Connections.Add(new Connection("27", "18"));
mapTemplate.Connections.Add(new Connection("27", "3"));
mapTemplate.Connections.Add(new Connection("27", "29"));
mapTemplate.Connections.Add(new Connection("28", "27"));
mapTemplate.Connections.Add(new Connection("28", "29"));
mapTemplate.Connections.Add(new Connection("29", "8"));
mapTemplate.Connections.Add(new Connection("29", "27"));
mapTemplate.Connections.Add(new Connection("29", "28"));
mapTemplate.Connections.Add(new Connection("29", "30"));
mapTemplate.Connections.Add(new Connection("29", "31"));
mapTemplate.Connections.Add(new Connection("29", "32"));
mapTemplate.Connections.Add(new Connection("29", "41"));
mapTemplate.Connections.Add(new Connection("29", "36"));
mapTemplate.Connections.Add(new Connection("30", "29"));
mapTemplate.Connections.Add(new Connection("30", "31"));
mapTemplate.Connections.Add(new Connection("30", "37"));
mapTemplate.Connections.Add(new Connection("31", "29"));
mapTemplate.Connections.Add(new Connection("31", "30"));
mapTemplate.Connections.Add(new Connection("31", "32"));
mapTemplate.Connections.Add(new Connection("31", "33"));
mapTemplate.Connections.Add(new Connection("31", "36"));
mapTemplate.Connections.Add(new Connection("31", "37"));
mapTemplate.Connections.Add(new Connection("32", "22"));
mapTemplate.Connections.Add(new Connection("32", "29"));
mapTemplate.Connections.Add(new Connection("32", "31"));
mapTemplate.Connections.Add(new Connection("32", "33"));
mapTemplate.Connections.Add(new Connection("33", "22"));
mapTemplate.Connections.Add(new Connection("33", "31"));
mapTemplate.Connections.Add(new Connection("33", "32"));
mapTemplate.Connections.Add(new Connection("33", "34"));
mapTemplate.Connections.Add(new Connection("33", "35"));
mapTemplate.Connections.Add(new Connection("33", "36"));
mapTemplate.Connections.Add(new Connection("34", "33"));
mapTemplate.Connections.Add(new Connection("34", "35"));
mapTemplate.Connections.Add(new Connection("34", "36"));
mapTemplate.Connections.Add(new Connection("34", "40"));
mapTemplate.Connections.Add(new Connection("35", "22"));
mapTemplate.Connections.Add(new Connection("35", "25"));
mapTemplate.Connections.Add(new Connection("35", "26"));
mapTemplate.Connections.Add(new Connection("35", "33"));
mapTemplate.Connections.Add(new Connection("35", "34"));
mapTemplate.Connections.Add(new Connection("35", "42"));
mapTemplate.Connections.Add(new Connection("35", "40"));
mapTemplate.Connections.Add(new Connection("36", "18"));
mapTemplate.Connections.Add(new Connection("36", "29"));
mapTemplate.Connections.Add(new Connection("36", "31"));
mapTemplate.Connections.Add(new Connection("36", "33"));
mapTemplate.Connections.Add(new Connection("36", "34"));
mapTemplate.Connections.Add(new Connection("36", "37"));
mapTemplate.Connections.Add(new Connection("36", "38"));
mapTemplate.Connections.Add(new Connection("36", "39"));
mapTemplate.Connections.Add(new Connection("36", "40"));
mapTemplate.Connections.Add(new Connection("36", "41"));
mapTemplate.Connections.Add(new Connection("36", "53"));
mapTemplate.Connections.Add(new Connection("36", "29"));
mapTemplate.Connections.Add(new Connection("37", "30"));
mapTemplate.Connections.Add(new Connection("37", "31"));
mapTemplate.Connections.Add(new Connection("37", "36"));
mapTemplate.Connections.Add(new Connection("37", "38"));
mapTemplate.Connections.Add(new Connection("38", "36"));
mapTemplate.Connections.Add(new Connection("38", "37"));
mapTemplate.Connections.Add(new Connection("38", "39"));
mapTemplate.Connections.Add(new Connection("38", "50"));
mapTemplate.Connections.Add(new Connection("38", "75"));
mapTemplate.Connections.Add(new Connection("39", "36"));
mapTemplate.Connections.Add(new Connection("39", "38"));
mapTemplate.Connections.Add(new Connection("39", "49"));
mapTemplate.Connections.Add(new Connection("40", "34"));
mapTemplate.Connections.Add(new Connection("40", "35"));
mapTemplate.Connections.Add(new Connection("40", "36"));
mapTemplate.Connections.Add(new Connection("40", "41"));
mapTemplate.Connections.Add(new Connection("40", "42"));
mapTemplate.Connections.Add(new Connection("41", "8"));
mapTemplate.Connections.Add(new Connection("41", "29"));
mapTemplate.Connections.Add(new Connection("41", "36"));
mapTemplate.Connections.Add(new Connection("41", "40"));
mapTemplate.Connections.Add(new Connection("41", "42"));
mapTemplate.Connections.Add(new Connection("41", "43"));
mapTemplate.Connections.Add(new Connection("42", "26"));
mapTemplate.Connections.Add(new Connection("42", "35"));
mapTemplate.Connections.Add(new Connection("42", "40"));
mapTemplate.Connections.Add(new Connection("42", "41"));
mapTemplate.Connections.Add(new Connection("42", "43"));
mapTemplate.Connections.Add(new Connection("43", "41"));
mapTemplate.Connections.Add(new Connection("43", "42"));
mapTemplate.Connections.Add(new Connection("43", "44"));
mapTemplate.Connections.Add(new Connection("43", "80"));
mapTemplate.Connections.Add(new Connection("44", "43"));
mapTemplate.Connections.Add(new Connection("44", "45"));
mapTemplate.Connections.Add(new Connection("44", "46"));
mapTemplate.Connections.Add(new Connection("44", "47"));
mapTemplate.Connections.Add(new Connection("44", "79"));
mapTemplate.Connections.Add(new Connection("45", "44"));
mapTemplate.Connections.Add(new Connection("45", "46"));
mapTemplate.Connections.Add(new Connection("45", "48"));
mapTemplate.Connections.Add(new Connection("46", "44"));
mapTemplate.Connections.Add(new Connection("46", "45"));
mapTemplate.Connections.Add(new Connection("46", "47"));
mapTemplate.Connections.Add(new Connection("46", "48"));
mapTemplate.Connections.Add(new Connection("46", "52"));
mapTemplate.Connections.Add(new Connection("46", "53"));
mapTemplate.Connections.Add(new Connection("47", "44"));
mapTemplate.Connections.Add(new Connection("47", "46"));
mapTemplate.Connections.Add(new Connection("47", "51"));
mapTemplate.Connections.Add(new Connection("47", "52"));
mapTemplate.Connections.Add(new Connection("47", "78"));
mapTemplate.Connections.Add(new Connection("48", "45"));
mapTemplate.Connections.Add(new Connection("48", "46"));
mapTemplate.Connections.Add(new Connection("48", "49"));
mapTemplate.Connections.Add(new Connection("48", "53"));
mapTemplate.Connections.Add(new Connection("48", "55"));
mapTemplate.Connections.Add(new Connection("49", "39"));
mapTemplate.Connections.Add(new Connection("49", "48"));
mapTemplate.Connections.Add(new Connection("49", "50"));
mapTemplate.Connections.Add(new Connection("49", "55"));
mapTemplate.Connections.Add(new Connection("50", "38"));
mapTemplate.Connections.Add(new Connection("50", "49"));
mapTemplate.Connections.Add(new Connection("50", "55"));
mapTemplate.Connections.Add(new Connection("50", "56"));
mapTemplate.Connections.Add(new Connection("50", "57"));
mapTemplate.Connections.Add(new Connection("50", "75"));
mapTemplate.Connections.Add(new Connection("51", "47"));
mapTemplate.Connections.Add(new Connection("51", "52"));
mapTemplate.Connections.Add(new Connection("51", "60"));
mapTemplate.Connections.Add(new Connection("51", "77"));
mapTemplate.Connections.Add(new Connection("52", "46"));
mapTemplate.Connections.Add(new Connection("52", "47"));
mapTemplate.Connections.Add(new Connection("52", "51"));
mapTemplate.Connections.Add(new Connection("52", "53"));
mapTemplate.Connections.Add(new Connection("52", "54"));
mapTemplate.Connections.Add(new Connection("52", "60"));
mapTemplate.Connections.Add(new Connection("53", "36"));
mapTemplate.Connections.Add(new Connection("53", "46"));
mapTemplate.Connections.Add(new Connection("53", "48"));
mapTemplate.Connections.Add(new Connection("53", "52"));
mapTemplate.Connections.Add(new Connection("53", "54"));
mapTemplate.Connections.Add(new Connection("53", "55"));
mapTemplate.Connections.Add(new Connection("53", "62"));
mapTemplate.Connections.Add(new Connection("53", "63"));
mapTemplate.Connections.Add(new Connection("53", "77"));
mapTemplate.Connections.Add(new Connection("54", "52"));
mapTemplate.Connections.Add(new Connection("54", "53"));
mapTemplate.Connections.Add(new Connection("54", "59"));
mapTemplate.Connections.Add(new Connection("54", "60"));
mapTemplate.Connections.Add(new Connection("54", "62"));
mapTemplate.Connections.Add(new Connection("55", "48"));
mapTemplate.Connections.Add(new Connection("55", "49"));
mapTemplate.Connections.Add(new Connection("55", "50"));
mapTemplate.Connections.Add(new Connection("55", "53"));
mapTemplate.Connections.Add(new Connection("55", "56"));
mapTemplate.Connections.Add(new Connection("55", "62"));
mapTemplate.Connections.Add(new Connection("56", "50"));
mapTemplate.Connections.Add(new Connection("56", "55"));
mapTemplate.Connections.Add(new Connection("56", "57"));
mapTemplate.Connections.Add(new Connection("56", "62"));
mapTemplate.Connections.Add(new Connection("56", "63"));
mapTemplate.Connections.Add(new Connection("57", "50"));
mapTemplate.Connections.Add(new Connection("57", "56"));
mapTemplate.Connections.Add(new Connection("57", "63"));
mapTemplate.Connections.Add(new Connection("58", "59"));
mapTemplate.Connections.Add(new Connection("58", "60"));
mapTemplate.Connections.Add(new Connection("58", "77"));
mapTemplate.Connections.Add(new Connection("59", "54"));
mapTemplate.Connections.Add(new Connection("59", "58"));
mapTemplate.Connections.Add(new Connection("59", "60"));
mapTemplate.Connections.Add(new Connection("59", "61"));
mapTemplate.Connections.Add(new Connection("59", "62"));
mapTemplate.Connections.Add(new Connection("60", "51"));
mapTemplate.Connections.Add(new Connection("60", "52"));
mapTemplate.Connections.Add(new Connection("60", "54"));
mapTemplate.Connections.Add(new Connection("60", "58"));
mapTemplate.Connections.Add(new Connection("60", "59"));
mapTemplate.Connections.Add(new Connection("61", "59"));
mapTemplate.Connections.Add(new Connection("61", "62"));
mapTemplate.Connections.Add(new Connection("61", "63"));
mapTemplate.Connections.Add(new Connection("62", "53"));
mapTemplate.Connections.Add(new Connection("62", "54"));
mapTemplate.Connections.Add(new Connection("62", "55"));
mapTemplate.Connections.Add(new Connection("62", "56"));
mapTemplate.Connections.Add(new Connection("62", "59"));
mapTemplate.Connections.Add(new Connection("62", "61"));
mapTemplate.Connections.Add(new Connection("62", "63"));
mapTemplate.Connections.Add(new Connection("63", "53"));
mapTemplate.Connections.Add(new Connection("63", "56"));
mapTemplate.Connections.Add(new Connection("63", "57"));
mapTemplate.Connections.Add(new Connection("63", "61"));
mapTemplate.Connections.Add(new Connection("63", "62"));
mapTemplate.Connections.Add(new Connection("64", "8"));
mapTemplate.Connections.Add(new Connection("64", "66"));
mapTemplate.Connections.Add(new Connection("65", "9"));
mapTemplate.Connections.Add(new Connection("65", "14"));
mapTemplate.Connections.Add(new Connection("65", "66"));
mapTemplate.Connections.Add(new Connection("65", "75"));
mapTemplate.Connections.Add(new Connection("66", "64"));
mapTemplate.Connections.Add(new Connection("66", "65"));
mapTemplate.Connections.Add(new Connection("66", "67"));
mapTemplate.Connections.Add(new Connection("66", "68"));
mapTemplate.Connections.Add(new Connection("67", "66"));
mapTemplate.Connections.Add(new Connection("67", "68"));
mapTemplate.Connections.Add(new Connection("67", "69"));
mapTemplate.Connections.Add(new Connection("67", "70"));
mapTemplate.Connections.Add(new Connection("68", "15"));
mapTemplate.Connections.Add(new Connection("68", "66"));
mapTemplate.Connections.Add(new Connection("68", "67"));
mapTemplate.Connections.Add(new Connection("68", "70"));
mapTemplate.Connections.Add(new Connection("68", "73"));
mapTemplate.Connections.Add(new Connection("69", "67"));
mapTemplate.Connections.Add(new Connection("69", "70"));
mapTemplate.Connections.Add(new Connection("69", "71"));
mapTemplate.Connections.Add(new Connection("70", "5"));
mapTemplate.Connections.Add(new Connection("70", "18"));
mapTemplate.Connections.Add(new Connection("70", "67"));
mapTemplate.Connections.Add(new Connection("70", "68"));
mapTemplate.Connections.Add(new Connection("70", "69"));
mapTemplate.Connections.Add(new Connection("70", "71"));
mapTemplate.Connections.Add(new Connection("70", "72"));
mapTemplate.Connections.Add(new Connection("70", "73"));
mapTemplate.Connections.Add(new Connection("70", "74"));
mapTemplate.Connections.Add(new Connection("70", "77"));
mapTemplate.Connections.Add(new Connection("71", "69"));
mapTemplate.Connections.Add(new Connection("71", "70"));
mapTemplate.Connections.Add(new Connection("71", "72"));
mapTemplate.Connections.Add(new Connection("71", "76"));
mapTemplate.Connections.Add(new Connection("72", "70"));
mapTemplate.Connections.Add(new Connection("72", "71"));
mapTemplate.Connections.Add(new Connection("72", "74"));
mapTemplate.Connections.Add(new Connection("72", "76"));
mapTemplate.Connections.Add(new Connection("72", "78"));
mapTemplate.Connections.Add(new Connection("72", "79"));
mapTemplate.Connections.Add(new Connection("73", "68"));
mapTemplate.Connections.Add(new Connection("73", "70"));
mapTemplate.Connections.Add(new Connection("73", "74"));
mapTemplate.Connections.Add(new Connection("73", "75"));
mapTemplate.Connections.Add(new Connection("74", "70"));
mapTemplate.Connections.Add(new Connection("74", "72"));
mapTemplate.Connections.Add(new Connection("74", "73"));
mapTemplate.Connections.Add(new Connection("74", "79"));
mapTemplate.Connections.Add(new Connection("75", "26"));
mapTemplate.Connections.Add(new Connection("75", "38"));
mapTemplate.Connections.Add(new Connection("75", "50"));
mapTemplate.Connections.Add(new Connection("75", "65"));
mapTemplate.Connections.Add(new Connection("75", "73"));
mapTemplate.Connections.Add(new Connection("76", "71"));
mapTemplate.Connections.Add(new Connection("76", "72"));
mapTemplate.Connections.Add(new Connection("76", "77"));
mapTemplate.Connections.Add(new Connection("76", "78"));
mapTemplate.Connections.Add(new Connection("77", "51"));
mapTemplate.Connections.Add(new Connection("77", "53"));
mapTemplate.Connections.Add(new Connection("77", "58"));
mapTemplate.Connections.Add(new Connection("77", "70"));
mapTemplate.Connections.Add(new Connection("77", "76"));
mapTemplate.Connections.Add(new Connection("77", "78"));
mapTemplate.Connections.Add(new Connection("78", "47"));
mapTemplate.Connections.Add(new Connection("78", "72"));
mapTemplate.Connections.Add(new Connection("78", "76"));
mapTemplate.Connections.Add(new Connection("78", "77"));
mapTemplate.Connections.Add(new Connection("78", "79"));
mapTemplate.Connections.Add(new Connection("79", "44"));
mapTemplate.Connections.Add(new Connection("79", "72"));
mapTemplate.Connections.Add(new Connection("79", "74"));
mapTemplate.Connections.Add(new Connection("79", "78"));
mapTemplate.Connections.Add(new Connection("79", "80"));
mapTemplate.Connections.Add(new Connection("80", "43"));
mapTemplate.Connections.Add(new Connection("80", "79"));

            return mapTemplate;
		}
    }
}

