
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
        public static MapTemplate bmt()
        {

var mapTemplate = new MapTemplate("bmt") { Image = "bmt.jpg" };
var country1 = new CountryTemplate("1", "Graues Fort") { X = 863, Y = 724 };
mapTemplate.Countries.Add(country1);
var country2 = new CountryTemplate("2", "Calia") { X = 868, Y = 853 };
mapTemplate.Countries.Add(country2);
var country3 = new CountryTemplate("3", "Mandryn") { X = 922, Y = 812 };
mapTemplate.Countries.Add(country3);
var country4 = new CountryTemplate("4", "Fort Corres") { X = 1008, Y = 826 };
mapTemplate.Countries.Add(country4);
var country5 = new CountryTemplate("5", "Kaltbr�cken") { X = 965, Y = 893 };
mapTemplate.Countries.Add(country5);
var country6 = new CountryTemplate("6", "Fennin") { X = 1111, Y = 894 };
mapTemplate.Countries.Add(country6);
var country7 = new CountryTemplate("7", "Meerblick") { X = 810, Y = 916 };
mapTemplate.Countries.Add(country7);
var country8 = new CountryTemplate("8", "Sheel") { X = 925, Y = 959 };
mapTemplate.Countries.Add(country8);
var country9 = new CountryTemplate("9", "Agen") { X = 1003, Y = 983 };
mapTemplate.Countries.Add(country9);
var country10 = new CountryTemplate("10", "Imardin") { X = 859, Y = 934 };
mapTemplate.Countries.Add(country10);
var country11 = new CountryTemplate("11", "Faynara") { X = 1004, Y = 475 };
mapTemplate.Countries.Add(country11);
var country12 = new CountryTemplate("12", "Jayan") { X = 1167, Y = 500 };
mapTemplate.Countries.Add(country12);
var country13 = new CountryTemplate("13", "Westwall") { X = 970, Y = 586 };
mapTemplate.Countries.Add(country13);
var country14 = new CountryTemplate("14", "Westw�ste") { X = 1052, Y = 563 };
mapTemplate.Countries.Add(country14);
var country15 = new CountryTemplate("15", "Ostw�ste") { X = 1117, Y = 664 };
mapTemplate.Countries.Add(country15);
var country16 = new CountryTemplate("16", "Arvice") { X = 1156, Y = 701 };
mapTemplate.Countries.Add(country16);
var country17 = new CountryTemplate("17", "Nobis") { X = 1143, Y = 784 };
mapTemplate.Countries.Add(country17);
var country18 = new CountryTemplate("18", "Akkarin") { X = 1013, Y = 731 };
mapTemplate.Countries.Add(country18);
var country19 = new CountryTemplate("19", "Duna") { X = 1068, Y = 102 };
mapTemplate.Countries.Add(country19);
var country20 = new CountryTemplate("20", "Rachiro") { X = 964, Y = 334 };
mapTemplate.Countries.Add(country20);
var country21 = new CountryTemplate("21", "Aschew�ste") { X = 1155, Y = 183 };
mapTemplate.Countries.Add(country21);
var country22 = new CountryTemplate("22", "Aduna") { X = 1109, Y = 384 };
mapTemplate.Countries.Add(country22);
var country23 = new CountryTemplate("23", "Kapvia") { X = 19, Y = 229 };
mapTemplate.Countries.Add(country23);
var country24 = new CountryTemplate("24", "Dusterwald") { X = 106, Y = 180 };
mapTemplate.Countries.Add(country24);
var country25 = new CountryTemplate("25", "Nodra") { X = 166, Y = 44 };
mapTemplate.Countries.Add(country25);
var country26 = new CountryTemplate("26", "Hadria") { X = 282, Y = 39 };
mapTemplate.Countries.Add(country26);
var country27 = new CountryTemplate("27", "Barden") { X = 589, Y = 15 };
mapTemplate.Countries.Add(country27);
var country28 = new CountryTemplate("28", "Duevarden") { X = 590, Y = 159 };
mapTemplate.Countries.Add(country28);
var country29 = new CountryTemplate("29", "Kamora") { X = 343, Y = 261 };
mapTemplate.Countries.Add(country29);
var country30 = new CountryTemplate("30", "Tempel") { X = 122, Y = 98 };
mapTemplate.Countries.Add(country30);
var country31 = new CountryTemplate("31", "Virydia") { X = 688, Y = 215 };
mapTemplate.Countries.Add(country31);
var country32 = new CountryTemplate("32", "Sturmrast") { X = 585, Y = 226 };
mapTemplate.Countries.Add(country32);
var country33 = new CountryTemplate("33", "Rastf�hre") { X = 480, Y = 383 };
mapTemplate.Countries.Add(country33);
var country34 = new CountryTemplate("34", "Mitwalden") { X = 653, Y = 345 };
mapTemplate.Countries.Add(country34);
var country35 = new CountryTemplate("35", "Fin") { X = 820, Y = 265 };
mapTemplate.Countries.Add(country35);
var country36 = new CountryTemplate("36", "Lindblum") { X = 793, Y = 383 };
mapTemplate.Countries.Add(country36);
var country37 = new CountryTemplate("37", "Regin") { X = 680, Y = 464 };
mapTemplate.Countries.Add(country37);
var country38 = new CountryTemplate("38", "Spira") { X = 751, Y = 567 };
mapTemplate.Countries.Add(country38);
var country39 = new CountryTemplate("39", "Meson") { X = 668, Y = 601 };
mapTemplate.Countries.Add(country39);
var country40 = new CountryTemplate("40", "Dreiecken") { X = 768, Y = 693 };
mapTemplate.Countries.Add(country40);
var country41 = new CountryTemplate("41", "Kendil") { X = 865, Y = 603 };
mapTemplate.Countries.Add(country41);
var country42 = new CountryTemplate("42", "Alm") { X = 856, Y = 477 };
mapTemplate.Countries.Add(country42);
var country43 = new CountryTemplate("43", "Capia") { X = 558, Y = 526 };
mapTemplate.Countries.Add(country43);
var country44 = new CountryTemplate("44", "Espen") { X = 220, Y = 650 };
mapTemplate.Countries.Add(country44);
var country45 = new CountryTemplate("45", "Cleyra") { X = 275, Y = 796 };
mapTemplate.Countries.Add(country45);
var country46 = new CountryTemplate("46", "Dali") { X = 191, Y = 788 };
mapTemplate.Countries.Add(country46);
var country47 = new CountryTemplate("47", "Trabia") { X = 207, Y = 910 };
mapTemplate.Countries.Add(country47);
var country48 = new CountryTemplate("48", "Lifa") { X = 240, Y = 1040 };
mapTemplate.Countries.Add(country48);
var country49 = new CountryTemplate("49", "Son") { X = 164, Y = 1129 };
mapTemplate.Countries.Add(country49);
var country50 = new CountryTemplate("50", "Madain") { X = 252, Y = 1235 };
mapTemplate.Countries.Add(country50);
var country51 = new CountryTemplate("51", "Sari") { X = 149, Y = 1261 };
mapTemplate.Countries.Add(country51);
var country52 = new CountryTemplate("52", "Al Leda") { X = 353, Y = 1272 };
mapTemplate.Countries.Add(country52);
var country53 = new CountryTemplate("53", "Ese Nawoer") { X = 343, Y = 1364 };
mapTemplate.Countries.Add(country53);
var country54 = new CountryTemplate("54", "Ori Vert") { X = 170, Y = 1352 };
mapTemplate.Countries.Add(country54);
var continent1 = new Continent("1", 3);
continent1.Countries.Add(country23);
continent1.Countries.Add(country24);
continent1.Countries.Add(country25);
continent1.Countries.Add(country26);
continent1.Countries.Add(country27);
continent1.Countries.Add(country28);
continent1.Countries.Add(country29);
continent1.Countries.Add(country30);
mapTemplate.Continents.Add(continent1);
var continent2 = new Continent("2", 9);
continent2.Countries.Add(country31);
continent2.Countries.Add(country32);
continent2.Countries.Add(country33);
continent2.Countries.Add(country34);
continent2.Countries.Add(country35);
continent2.Countries.Add(country36);
continent2.Countries.Add(country37);
continent2.Countries.Add(country38);
continent2.Countries.Add(country39);
continent2.Countries.Add(country40);
continent2.Countries.Add(country41);
continent2.Countries.Add(country42);
continent2.Countries.Add(country43);
mapTemplate.Continents.Add(continent2);
var continent3 = new Continent("3", 2);
continent3.Countries.Add(country19);
continent3.Countries.Add(country20);
continent3.Countries.Add(country21);
continent3.Countries.Add(country22);
mapTemplate.Continents.Add(continent3);
var continent4 = new Continent("4", 5);
continent4.Countries.Add(country11);
continent4.Countries.Add(country12);
continent4.Countries.Add(country13);
continent4.Countries.Add(country14);
continent4.Countries.Add(country15);
continent4.Countries.Add(country16);
continent4.Countries.Add(country17);
continent4.Countries.Add(country18);
mapTemplate.Continents.Add(continent4);
var continent5 = new Continent("5", 5);
continent5.Countries.Add(country1);
continent5.Countries.Add(country2);
continent5.Countries.Add(country3);
continent5.Countries.Add(country4);
continent5.Countries.Add(country5);
continent5.Countries.Add(country6);
continent5.Countries.Add(country7);
continent5.Countries.Add(country8);
continent5.Countries.Add(country9);
continent5.Countries.Add(country10);
mapTemplate.Continents.Add(continent5);
var continent6 = new Continent("6", 4);
continent6.Countries.Add(country44);
continent6.Countries.Add(country45);
continent6.Countries.Add(country46);
continent6.Countries.Add(country47);
continent6.Countries.Add(country48);
continent6.Countries.Add(country49);
continent6.Countries.Add(country50);
continent6.Countries.Add(country51);
mapTemplate.Continents.Add(continent6);
var continent7 = new Continent("7", 2);
continent7.Countries.Add(country52);
continent7.Countries.Add(country53);
continent7.Countries.Add(country54);
mapTemplate.Continents.Add(continent7);
var continent8 = new Continent("8", 1);
continent8.Countries.Add(country30);
mapTemplate.Continents.Add(continent8);
var continent9 = new Continent("9", 1);
continent9.Countries.Add(country43);
mapTemplate.Continents.Add(continent9);
var continent10 = new Continent("10", 1);
continent10.Countries.Add(country10);
mapTemplate.Continents.Add(continent10);
mapTemplate.Connections.Add(new Connection("1", "40"));
mapTemplate.Connections.Add(new Connection("1", "18"));
mapTemplate.Connections.Add(new Connection("1", "3"));
mapTemplate.Connections.Add(new Connection("1", "2"));
mapTemplate.Connections.Add(new Connection("2", "1"));
mapTemplate.Connections.Add(new Connection("2", "3"));
mapTemplate.Connections.Add(new Connection("2", "5"));
mapTemplate.Connections.Add(new Connection("2", "7"));
mapTemplate.Connections.Add(new Connection("3", "1"));
mapTemplate.Connections.Add(new Connection("3", "18"));
mapTemplate.Connections.Add(new Connection("3", "2"));
mapTemplate.Connections.Add(new Connection("3", "4"));
mapTemplate.Connections.Add(new Connection("4", "3"));
mapTemplate.Connections.Add(new Connection("4", "17"));
mapTemplate.Connections.Add(new Connection("4", "6"));
mapTemplate.Connections.Add(new Connection("4", "5"));
mapTemplate.Connections.Add(new Connection("5", "8"));
mapTemplate.Connections.Add(new Connection("5", "4"));
mapTemplate.Connections.Add(new Connection("5", "2"));
mapTemplate.Connections.Add(new Connection("6", "4"));
mapTemplate.Connections.Add(new Connection("6", "9"));
mapTemplate.Connections.Add(new Connection("7", "2"));
mapTemplate.Connections.Add(new Connection("7", "50"));
mapTemplate.Connections.Add(new Connection("7", "10"));
mapTemplate.Connections.Add(new Connection("8", "9"));
mapTemplate.Connections.Add(new Connection("8", "5"));
mapTemplate.Connections.Add(new Connection("8", "10"));
mapTemplate.Connections.Add(new Connection("9", "6"));
mapTemplate.Connections.Add(new Connection("9", "8"));
mapTemplate.Connections.Add(new Connection("10", "52"));
mapTemplate.Connections.Add(new Connection("10", "7"));
mapTemplate.Connections.Add(new Connection("10", "8"));
mapTemplate.Connections.Add(new Connection("11", "13"));
mapTemplate.Connections.Add(new Connection("11", "42"));
mapTemplate.Connections.Add(new Connection("11", "36"));
mapTemplate.Connections.Add(new Connection("11", "12"));
mapTemplate.Connections.Add(new Connection("11", "14"));
mapTemplate.Connections.Add(new Connection("11", "20"));
mapTemplate.Connections.Add(new Connection("11", "22"));
mapTemplate.Connections.Add(new Connection("12", "14"));
mapTemplate.Connections.Add(new Connection("12", "15"));
mapTemplate.Connections.Add(new Connection("12", "16"));
mapTemplate.Connections.Add(new Connection("12", "11"));
mapTemplate.Connections.Add(new Connection("12", "22"));
mapTemplate.Connections.Add(new Connection("13", "11"));
mapTemplate.Connections.Add(new Connection("13", "41"));
mapTemplate.Connections.Add(new Connection("13", "18"));
mapTemplate.Connections.Add(new Connection("13", "14"));
mapTemplate.Connections.Add(new Connection("14", "13"));
mapTemplate.Connections.Add(new Connection("14", "15"));
mapTemplate.Connections.Add(new Connection("14", "11"));
mapTemplate.Connections.Add(new Connection("14", "12"));
mapTemplate.Connections.Add(new Connection("15", "18"));
mapTemplate.Connections.Add(new Connection("15", "14"));
mapTemplate.Connections.Add(new Connection("15", "16"));
mapTemplate.Connections.Add(new Connection("15", "12"));
mapTemplate.Connections.Add(new Connection("16", "15"));
mapTemplate.Connections.Add(new Connection("16", "12"));
mapTemplate.Connections.Add(new Connection("17", "4"));
mapTemplate.Connections.Add(new Connection("17", "18"));
mapTemplate.Connections.Add(new Connection("18", "1"));
mapTemplate.Connections.Add(new Connection("18", "40"));
mapTemplate.Connections.Add(new Connection("18", "3"));
mapTemplate.Connections.Add(new Connection("18", "17"));
mapTemplate.Connections.Add(new Connection("18", "15"));
mapTemplate.Connections.Add(new Connection("18", "13"));
mapTemplate.Connections.Add(new Connection("19", "21"));
mapTemplate.Connections.Add(new Connection("19", "20"));
mapTemplate.Connections.Add(new Connection("20", "35"));
mapTemplate.Connections.Add(new Connection("20", "11"));
mapTemplate.Connections.Add(new Connection("20", "22"));
mapTemplate.Connections.Add(new Connection("20", "21"));
mapTemplate.Connections.Add(new Connection("20", "19"));
mapTemplate.Connections.Add(new Connection("21", "20"));
mapTemplate.Connections.Add(new Connection("21", "22"));
mapTemplate.Connections.Add(new Connection("21", "19"));
mapTemplate.Connections.Add(new Connection("22", "11"));
mapTemplate.Connections.Add(new Connection("22", "12"));
mapTemplate.Connections.Add(new Connection("22", "20"));
mapTemplate.Connections.Add(new Connection("22", "21"));
mapTemplate.Connections.Add(new Connection("23", "44"));
mapTemplate.Connections.Add(new Connection("23", "33"));
mapTemplate.Connections.Add(new Connection("23", "24"));
mapTemplate.Connections.Add(new Connection("24", "23"));
mapTemplate.Connections.Add(new Connection("24", "25"));
mapTemplate.Connections.Add(new Connection("24", "30"));
mapTemplate.Connections.Add(new Connection("25", "24"));
mapTemplate.Connections.Add(new Connection("25", "30"));
mapTemplate.Connections.Add(new Connection("25", "26"));
mapTemplate.Connections.Add(new Connection("26", "25"));
mapTemplate.Connections.Add(new Connection("26", "27"));
mapTemplate.Connections.Add(new Connection("27", "26"));
mapTemplate.Connections.Add(new Connection("27", "28"));
mapTemplate.Connections.Add(new Connection("28", "27"));
mapTemplate.Connections.Add(new Connection("28", "29"));
mapTemplate.Connections.Add(new Connection("28", "31"));
mapTemplate.Connections.Add(new Connection("28", "32"));
mapTemplate.Connections.Add(new Connection("29", "28"));
mapTemplate.Connections.Add(new Connection("29", "33"));
mapTemplate.Connections.Add(new Connection("29", "32"));
mapTemplate.Connections.Add(new Connection("30", "24"));
mapTemplate.Connections.Add(new Connection("30", "25"));
mapTemplate.Connections.Add(new Connection("31", "36"));
mapTemplate.Connections.Add(new Connection("31", "28"));
mapTemplate.Connections.Add(new Connection("31", "32"));
mapTemplate.Connections.Add(new Connection("31", "35"));
mapTemplate.Connections.Add(new Connection("31", "34"));
mapTemplate.Connections.Add(new Connection("32", "28"));
mapTemplate.Connections.Add(new Connection("32", "29"));
mapTemplate.Connections.Add(new Connection("32", "33"));
mapTemplate.Connections.Add(new Connection("32", "31"));
mapTemplate.Connections.Add(new Connection("32", "34"));
mapTemplate.Connections.Add(new Connection("33", "23"));
mapTemplate.Connections.Add(new Connection("33", "29"));
mapTemplate.Connections.Add(new Connection("33", "32"));
mapTemplate.Connections.Add(new Connection("33", "34"));
mapTemplate.Connections.Add(new Connection("33", "37"));
mapTemplate.Connections.Add(new Connection("34", "31"));
mapTemplate.Connections.Add(new Connection("34", "32"));
mapTemplate.Connections.Add(new Connection("34", "33"));
mapTemplate.Connections.Add(new Connection("34", "37"));
mapTemplate.Connections.Add(new Connection("34", "36"));
mapTemplate.Connections.Add(new Connection("35", "31"));
mapTemplate.Connections.Add(new Connection("35", "36"));
mapTemplate.Connections.Add(new Connection("35", "20"));
mapTemplate.Connections.Add(new Connection("36", "31"));
mapTemplate.Connections.Add(new Connection("36", "34"));
mapTemplate.Connections.Add(new Connection("36", "35"));
mapTemplate.Connections.Add(new Connection("36", "38"));
mapTemplate.Connections.Add(new Connection("36", "11"));
mapTemplate.Connections.Add(new Connection("37", "34"));
mapTemplate.Connections.Add(new Connection("37", "33"));
mapTemplate.Connections.Add(new Connection("37", "43"));
mapTemplate.Connections.Add(new Connection("37", "39"));
mapTemplate.Connections.Add(new Connection("38", "39"));
mapTemplate.Connections.Add(new Connection("38", "40"));
mapTemplate.Connections.Add(new Connection("38", "36"));
mapTemplate.Connections.Add(new Connection("38", "41"));
mapTemplate.Connections.Add(new Connection("39", "43"));
mapTemplate.Connections.Add(new Connection("39", "37"));
mapTemplate.Connections.Add(new Connection("39", "38"));
mapTemplate.Connections.Add(new Connection("39", "40"));
mapTemplate.Connections.Add(new Connection("40", "38"));
mapTemplate.Connections.Add(new Connection("40", "39"));
mapTemplate.Connections.Add(new Connection("40", "41"));
mapTemplate.Connections.Add(new Connection("40", "18"));
mapTemplate.Connections.Add(new Connection("40", "1"));
mapTemplate.Connections.Add(new Connection("41", "38"));
mapTemplate.Connections.Add(new Connection("41", "42"));
mapTemplate.Connections.Add(new Connection("41", "13"));
mapTemplate.Connections.Add(new Connection("41", "40"));
mapTemplate.Connections.Add(new Connection("42", "41"));
mapTemplate.Connections.Add(new Connection("42", "11"));
mapTemplate.Connections.Add(new Connection("43", "37"));
mapTemplate.Connections.Add(new Connection("43", "39"));
mapTemplate.Connections.Add(new Connection("43", "45"));
mapTemplate.Connections.Add(new Connection("44", "46"));
mapTemplate.Connections.Add(new Connection("44", "45"));
mapTemplate.Connections.Add(new Connection("44", "23"));
mapTemplate.Connections.Add(new Connection("45", "48"));
mapTemplate.Connections.Add(new Connection("45", "47"));
mapTemplate.Connections.Add(new Connection("45", "46"));
mapTemplate.Connections.Add(new Connection("45", "44"));
mapTemplate.Connections.Add(new Connection("45", "43"));
mapTemplate.Connections.Add(new Connection("46", "47"));
mapTemplate.Connections.Add(new Connection("46", "45"));
mapTemplate.Connections.Add(new Connection("46", "44"));
mapTemplate.Connections.Add(new Connection("47", "49"));
mapTemplate.Connections.Add(new Connection("47", "48"));
mapTemplate.Connections.Add(new Connection("47", "45"));
mapTemplate.Connections.Add(new Connection("47", "46"));
mapTemplate.Connections.Add(new Connection("48", "50"));
mapTemplate.Connections.Add(new Connection("48", "49"));
mapTemplate.Connections.Add(new Connection("48", "47"));
mapTemplate.Connections.Add(new Connection("48", "45"));
mapTemplate.Connections.Add(new Connection("49", "50"));
mapTemplate.Connections.Add(new Connection("49", "48"));
mapTemplate.Connections.Add(new Connection("49", "47"));
mapTemplate.Connections.Add(new Connection("50", "51"));
mapTemplate.Connections.Add(new Connection("50", "52"));
mapTemplate.Connections.Add(new Connection("50", "49"));
mapTemplate.Connections.Add(new Connection("50", "48"));
mapTemplate.Connections.Add(new Connection("50", "7"));
mapTemplate.Connections.Add(new Connection("51", "54"));
mapTemplate.Connections.Add(new Connection("51", "50"));
mapTemplate.Connections.Add(new Connection("51", "52"));
mapTemplate.Connections.Add(new Connection("52", "54"));
mapTemplate.Connections.Add(new Connection("52", "53"));
mapTemplate.Connections.Add(new Connection("52", "10"));
mapTemplate.Connections.Add(new Connection("52", "50"));
mapTemplate.Connections.Add(new Connection("52", "51"));
mapTemplate.Connections.Add(new Connection("53", "54"));
mapTemplate.Connections.Add(new Connection("53", "52"));
mapTemplate.Connections.Add(new Connection("54", "53"));
mapTemplate.Connections.Add(new Connection("54", "52"));
mapTemplate.Connections.Add(new Connection("54", "51"));

            return mapTemplate;
		}
    }
}

