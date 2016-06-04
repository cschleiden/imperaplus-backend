
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
        public static MapTemplate Europa_Neu()
        {

var mapTemplate = new MapTemplate("Europa_Neu") { Image = "europa_neu.gif" };
var country1 = new CountryTemplate("1", "Island") { X = 72, Y = 110 };
mapTemplate.Countries.Add(country1);
var country2 = new CountryTemplate("2", "Norwegen") { X = 469, Y = 186 };
mapTemplate.Countries.Add(country2);
var country3 = new CountryTemplate("3", "Schweden") { X = 548, Y = 242 };
mapTemplate.Countries.Add(country3);
var country4 = new CountryTemplate("4", "Finnland") { X = 680, Y = 163 };
mapTemplate.Countries.Add(country4);
var country5 = new CountryTemplate("5", "Daenemark") { X = 481, Y = 277 };
mapTemplate.Countries.Add(country5);
var country6 = new CountryTemplate("6", "Vereinigtes Koenigreich") { X = 320, Y = 350 };
mapTemplate.Countries.Add(country6);
var country7 = new CountryTemplate("7", "Irland") { X = 211, Y = 333 };
mapTemplate.Countries.Add(country7);
var country8 = new CountryTemplate("8", "Frankreich") { X = 382, Y = 447 };
mapTemplate.Countries.Add(country8);
var country9 = new CountryTemplate("9", "Portugal") { X = 194, Y = 610 };
mapTemplate.Countries.Add(country9);
var country10 = new CountryTemplate("10", "Spanien") { X = 270, Y = 602 };
mapTemplate.Countries.Add(country10);
var country11 = new CountryTemplate("11", "Italien") { X = 548, Y = 545 };
mapTemplate.Countries.Add(country11);
var country12 = new CountryTemplate("12", "Schweiz") { X = 465, Y = 459 };
mapTemplate.Countries.Add(country12);
var country13 = new CountryTemplate("13", "Oesterreich") { X = 570, Y = 442 };
mapTemplate.Countries.Add(country13);
var country14 = new CountryTemplate("14", "Deutschland") { X = 512, Y = 350 };
mapTemplate.Countries.Add(country14);
var country15 = new CountryTemplate("15", "Niederlande") { X = 420, Y = 348 };
mapTemplate.Countries.Add(country15);
var country16 = new CountryTemplate("16", "Belgien") { X = 409, Y = 388 };
mapTemplate.Countries.Add(country16);
var country17 = new CountryTemplate("17", "Luxemburg") { X = 451, Y = 398 };
mapTemplate.Countries.Add(country17);
var country18 = new CountryTemplate("18", "Tschechien") { X = 566, Y = 399 };
mapTemplate.Countries.Add(country18);
var country19 = new CountryTemplate("19", "Slowakei") { X = 636, Y = 425 };
mapTemplate.Countries.Add(country19);
var country20 = new CountryTemplate("20", "Ungarn") { X = 618, Y = 451 };
mapTemplate.Countries.Add(country20);
var country21 = new CountryTemplate("21", "Polen") { X = 624, Y = 347 };
mapTemplate.Countries.Add(country21);
var country22 = new CountryTemplate("22", "Estland") { X = 713, Y = 225 };
mapTemplate.Countries.Add(country22);
var country23 = new CountryTemplate("23", "Lettland") { X = 708, Y = 257 };
mapTemplate.Countries.Add(country23);
var country24 = new CountryTemplate("24", "Litauen") { X = 696, Y = 290 };
mapTemplate.Countries.Add(country24);
var country25 = new CountryTemplate("25", "Belarus") { X = 758, Y = 318 };
mapTemplate.Countries.Add(country25);
var country26 = new CountryTemplate("26", "Ukraine") { X = 832, Y = 426 };
mapTemplate.Countries.Add(country26);
var country27 = new CountryTemplate("27", "Moldavien") { X = 796, Y = 452 };
mapTemplate.Countries.Add(country27);
var country28 = new CountryTemplate("28", "Rumaenien") { X = 742, Y = 481 };
mapTemplate.Countries.Add(country28);
var country29 = new CountryTemplate("29", "Bulgarien") { X = 750, Y = 542 };
mapTemplate.Countries.Add(country29);
var country30 = new CountryTemplate("30", "Tuerkei") { X = 832, Y = 624 };
mapTemplate.Countries.Add(country30);
var country31 = new CountryTemplate("31", "Griechenland") { X = 708, Y = 623 };
mapTemplate.Countries.Add(country31);
var country32 = new CountryTemplate("32", "Albanien") { X = 665, Y = 580 };
mapTemplate.Countries.Add(country32);
var country33 = new CountryTemplate("33", "Mazedonien") { X = 699, Y = 565 };
mapTemplate.Countries.Add(country33);
var country34 = new CountryTemplate("34", "Yugoslawien") { X = 679, Y = 524 };
mapTemplate.Countries.Add(country34);
var country35 = new CountryTemplate("35", "Bosnien_Herzigowina") { X = 630, Y = 515 };
mapTemplate.Countries.Add(country35);
var country36 = new CountryTemplate("36", "Kroatien") { X = 584, Y = 513 };
mapTemplate.Countries.Add(country36);
var country37 = new CountryTemplate("37", "Slowenien") { X = 573, Y = 477 };
mapTemplate.Countries.Add(country37);
var country38 = new CountryTemplate("38", "Andorra") { X = 406, Y = 563 };
mapTemplate.Countries.Add(country38);
var country39 = new CountryTemplate("39", "Monaco") { X = 444, Y = 563 };
mapTemplate.Countries.Add(country39);
var country40 = new CountryTemplate("40", "Russland") { X = 832, Y = 247 };
mapTemplate.Countries.Add(country40);
var country41 = new CountryTemplate("41", "Zypern") { X = 877, Y = 717 };
mapTemplate.Countries.Add(country41);
var country42 = new CountryTemplate("42", "Malta") { X = 600, Y = 695 };
mapTemplate.Countries.Add(country42);
var country43 = new CountryTemplate("43", "Algerien") { X = 382, Y = 709 };
mapTemplate.Countries.Add(country43);
var country44 = new CountryTemplate("44", "Marokko") { X = 254, Y = 717 };
mapTemplate.Countries.Add(country44);
var country45 = new CountryTemplate("45", "Lybien") { X = 494, Y = 703 };
mapTemplate.Countries.Add(country45);
var continent1 = new Continent("1", 3);
continent1.Countries.Add(country1);
continent1.Countries.Add(country2);
continent1.Countries.Add(country3);
continent1.Countries.Add(country4);
continent1.Countries.Add(country5);
mapTemplate.Continents.Add(continent1);
var continent2 = new Continent("2", 4);
continent2.Countries.Add(country9);
continent2.Countries.Add(country10);
continent2.Countries.Add(country38);
continent2.Countries.Add(country39);
continent2.Countries.Add(country8);
continent2.Countries.Add(country6);
continent2.Countries.Add(country7);
mapTemplate.Continents.Add(continent2);
var continent3 = new Continent("3", 4);
continent3.Countries.Add(country15);
continent3.Countries.Add(country16);
continent3.Countries.Add(country17);
continent3.Countries.Add(country14);
continent3.Countries.Add(country12);
continent3.Countries.Add(country13);
mapTemplate.Continents.Add(continent3);
var continent4 = new Continent("4", 6);
continent4.Countries.Add(country11);
continent4.Countries.Add(country37);
continent4.Countries.Add(country36);
continent4.Countries.Add(country35);
continent4.Countries.Add(country34);
continent4.Countries.Add(country32);
continent4.Countries.Add(country33);
continent4.Countries.Add(country31);
continent4.Countries.Add(country42);
mapTemplate.Continents.Add(continent4);
var continent5 = new Continent("5", 1);
continent5.Countries.Add(country44);
continent5.Countries.Add(country43);
continent5.Countries.Add(country45);
mapTemplate.Continents.Add(continent5);
var continent6 = new Continent("6", 2);
continent6.Countries.Add(country21);
continent6.Countries.Add(country18);
continent6.Countries.Add(country19);
continent6.Countries.Add(country20);
mapTemplate.Continents.Add(continent6);
var continent7 = new Continent("7", 3);
continent7.Countries.Add(country27);
continent7.Countries.Add(country28);
continent7.Countries.Add(country29);
continent7.Countries.Add(country30);
continent7.Countries.Add(country41);
mapTemplate.Continents.Add(continent7);
var continent8 = new Continent("8", 3);
continent8.Countries.Add(country40);
continent8.Countries.Add(country22);
continent8.Countries.Add(country23);
continent8.Countries.Add(country24);
continent8.Countries.Add(country25);
continent8.Countries.Add(country26);
mapTemplate.Continents.Add(continent8);
mapTemplate.Connections.Add(new Connection("15", "16"));
mapTemplate.Connections.Add(new Connection("15", "14"));
mapTemplate.Connections.Add(new Connection("30", "29"));
mapTemplate.Connections.Add(new Connection("30", "41"));
mapTemplate.Connections.Add(new Connection("30", "31"));
mapTemplate.Connections.Add(new Connection("8", "6"));
mapTemplate.Connections.Add(new Connection("8", "16"));
mapTemplate.Connections.Add(new Connection("8", "17"));
mapTemplate.Connections.Add(new Connection("8", "14"));
mapTemplate.Connections.Add(new Connection("8", "12"));
mapTemplate.Connections.Add(new Connection("8", "39"));
mapTemplate.Connections.Add(new Connection("8", "38"));
mapTemplate.Connections.Add(new Connection("8", "10"));
mapTemplate.Connections.Add(new Connection("8", "11"));
mapTemplate.Connections.Add(new Connection("23", "22"));
mapTemplate.Connections.Add(new Connection("23", "40"));
mapTemplate.Connections.Add(new Connection("23", "25"));
mapTemplate.Connections.Add(new Connection("23", "24"));
mapTemplate.Connections.Add(new Connection("40", "4"));
mapTemplate.Connections.Add(new Connection("40", "22"));
mapTemplate.Connections.Add(new Connection("40", "23"));
mapTemplate.Connections.Add(new Connection("40", "25"));
mapTemplate.Connections.Add(new Connection("40", "26"));
mapTemplate.Connections.Add(new Connection("40", "2"));
mapTemplate.Connections.Add(new Connection("43", "45"));
mapTemplate.Connections.Add(new Connection("43", "44"));
mapTemplate.Connections.Add(new Connection("16", "15"));
mapTemplate.Connections.Add(new Connection("16", "14"));
mapTemplate.Connections.Add(new Connection("16", "17"));
mapTemplate.Connections.Add(new Connection("16", "8"));
mapTemplate.Connections.Add(new Connection("31", "32"));
mapTemplate.Connections.Add(new Connection("31", "33"));
mapTemplate.Connections.Add(new Connection("31", "29"));
mapTemplate.Connections.Add(new Connection("31", "30"));
mapTemplate.Connections.Add(new Connection("31", "11"));
mapTemplate.Connections.Add(new Connection("35", "36"));
mapTemplate.Connections.Add(new Connection("35", "34"));
mapTemplate.Connections.Add(new Connection("7", "6"));
mapTemplate.Connections.Add(new Connection("22", "4"));
mapTemplate.Connections.Add(new Connection("22", "40"));
mapTemplate.Connections.Add(new Connection("22", "23"));
mapTemplate.Connections.Add(new Connection("32", "34"));
mapTemplate.Connections.Add(new Connection("32", "33"));
mapTemplate.Connections.Add(new Connection("32", "31"));
mapTemplate.Connections.Add(new Connection("34", "35"));
mapTemplate.Connections.Add(new Connection("34", "36"));
mapTemplate.Connections.Add(new Connection("34", "20"));
mapTemplate.Connections.Add(new Connection("34", "28"));
mapTemplate.Connections.Add(new Connection("34", "29"));
mapTemplate.Connections.Add(new Connection("34", "33"));
mapTemplate.Connections.Add(new Connection("34", "32"));
mapTemplate.Connections.Add(new Connection("41", "30"));
mapTemplate.Connections.Add(new Connection("9", "10"));
mapTemplate.Connections.Add(new Connection("21", "24"));
mapTemplate.Connections.Add(new Connection("21", "25"));
mapTemplate.Connections.Add(new Connection("21", "26"));
mapTemplate.Connections.Add(new Connection("21", "19"));
mapTemplate.Connections.Add(new Connection("21", "18"));
mapTemplate.Connections.Add(new Connection("21", "14"));
mapTemplate.Connections.Add(new Connection("33", "34"));
mapTemplate.Connections.Add(new Connection("33", "29"));
mapTemplate.Connections.Add(new Connection("33", "31"));
mapTemplate.Connections.Add(new Connection("33", "32"));
mapTemplate.Connections.Add(new Connection("6", "1"));
mapTemplate.Connections.Add(new Connection("6", "7"));
mapTemplate.Connections.Add(new Connection("6", "8"));
mapTemplate.Connections.Add(new Connection("1", "2"));
mapTemplate.Connections.Add(new Connection("1", "6"));
mapTemplate.Connections.Add(new Connection("29", "34"));
mapTemplate.Connections.Add(new Connection("29", "28"));
mapTemplate.Connections.Add(new Connection("29", "30"));
mapTemplate.Connections.Add(new Connection("29", "31"));
mapTemplate.Connections.Add(new Connection("29", "33"));
mapTemplate.Connections.Add(new Connection("14", "5"));
mapTemplate.Connections.Add(new Connection("14", "21"));
mapTemplate.Connections.Add(new Connection("14", "18"));
mapTemplate.Connections.Add(new Connection("14", "13"));
mapTemplate.Connections.Add(new Connection("14", "12"));
mapTemplate.Connections.Add(new Connection("14", "8"));
mapTemplate.Connections.Add(new Connection("14", "17"));
mapTemplate.Connections.Add(new Connection("14", "16"));
mapTemplate.Connections.Add(new Connection("14", "15"));
mapTemplate.Connections.Add(new Connection("24", "23"));
mapTemplate.Connections.Add(new Connection("24", "25"));
mapTemplate.Connections.Add(new Connection("24", "21"));
mapTemplate.Connections.Add(new Connection("42", "11"));
mapTemplate.Connections.Add(new Connection("42", "45"));
mapTemplate.Connections.Add(new Connection("4", "40"));
mapTemplate.Connections.Add(new Connection("4", "22"));
mapTemplate.Connections.Add(new Connection("4", "3"));
mapTemplate.Connections.Add(new Connection("4", "2"));
mapTemplate.Connections.Add(new Connection("19", "18"));
mapTemplate.Connections.Add(new Connection("19", "21"));
mapTemplate.Connections.Add(new Connection("19", "26"));
mapTemplate.Connections.Add(new Connection("19", "20"));
mapTemplate.Connections.Add(new Connection("19", "13"));
mapTemplate.Connections.Add(new Connection("36", "37"));
mapTemplate.Connections.Add(new Connection("36", "20"));
mapTemplate.Connections.Add(new Connection("36", "34"));
mapTemplate.Connections.Add(new Connection("36", "35"));
mapTemplate.Connections.Add(new Connection("26", "25"));
mapTemplate.Connections.Add(new Connection("26", "40"));
mapTemplate.Connections.Add(new Connection("26", "28"));
mapTemplate.Connections.Add(new Connection("26", "27"));
mapTemplate.Connections.Add(new Connection("26", "20"));
mapTemplate.Connections.Add(new Connection("26", "19"));
mapTemplate.Connections.Add(new Connection("26", "21"));
mapTemplate.Connections.Add(new Connection("11", "12"));
mapTemplate.Connections.Add(new Connection("11", "13"));
mapTemplate.Connections.Add(new Connection("11", "37"));
mapTemplate.Connections.Add(new Connection("11", "31"));
mapTemplate.Connections.Add(new Connection("11", "42"));
mapTemplate.Connections.Add(new Connection("11", "8"));
mapTemplate.Connections.Add(new Connection("18", "14"));
mapTemplate.Connections.Add(new Connection("18", "21"));
mapTemplate.Connections.Add(new Connection("18", "19"));
mapTemplate.Connections.Add(new Connection("18", "13"));
mapTemplate.Connections.Add(new Connection("3", "2"));
mapTemplate.Connections.Add(new Connection("3", "4"));
mapTemplate.Connections.Add(new Connection("3", "5"));
mapTemplate.Connections.Add(new Connection("39", "8"));
mapTemplate.Connections.Add(new Connection("12", "14"));
mapTemplate.Connections.Add(new Connection("12", "13"));
mapTemplate.Connections.Add(new Connection("12", "11"));
mapTemplate.Connections.Add(new Connection("12", "8"));
mapTemplate.Connections.Add(new Connection("44", "43"));
mapTemplate.Connections.Add(new Connection("44", "10"));
mapTemplate.Connections.Add(new Connection("27", "28"));
mapTemplate.Connections.Add(new Connection("27", "26"));
mapTemplate.Connections.Add(new Connection("45", "42"));
mapTemplate.Connections.Add(new Connection("45", "43"));
mapTemplate.Connections.Add(new Connection("17", "16"));
mapTemplate.Connections.Add(new Connection("17", "14"));
mapTemplate.Connections.Add(new Connection("17", "8"));
mapTemplate.Connections.Add(new Connection("2", "3"));
mapTemplate.Connections.Add(new Connection("2", "4"));
mapTemplate.Connections.Add(new Connection("2", "1"));
mapTemplate.Connections.Add(new Connection("2", "40"));
mapTemplate.Connections.Add(new Connection("13", "18"));
mapTemplate.Connections.Add(new Connection("13", "19"));
mapTemplate.Connections.Add(new Connection("13", "20"));
mapTemplate.Connections.Add(new Connection("13", "37"));
mapTemplate.Connections.Add(new Connection("13", "11"));
mapTemplate.Connections.Add(new Connection("13", "12"));
mapTemplate.Connections.Add(new Connection("13", "14"));
mapTemplate.Connections.Add(new Connection("28", "26"));
mapTemplate.Connections.Add(new Connection("28", "27"));
mapTemplate.Connections.Add(new Connection("28", "29"));
mapTemplate.Connections.Add(new Connection("28", "34"));
mapTemplate.Connections.Add(new Connection("28", "20"));
mapTemplate.Connections.Add(new Connection("38", "10"));
mapTemplate.Connections.Add(new Connection("38", "8"));
mapTemplate.Connections.Add(new Connection("20", "19"));
mapTemplate.Connections.Add(new Connection("20", "26"));
mapTemplate.Connections.Add(new Connection("20", "28"));
mapTemplate.Connections.Add(new Connection("20", "34"));
mapTemplate.Connections.Add(new Connection("20", "36"));
mapTemplate.Connections.Add(new Connection("20", "37"));
mapTemplate.Connections.Add(new Connection("20", "13"));
mapTemplate.Connections.Add(new Connection("25", "24"));
mapTemplate.Connections.Add(new Connection("25", "23"));
mapTemplate.Connections.Add(new Connection("25", "40"));
mapTemplate.Connections.Add(new Connection("25", "26"));
mapTemplate.Connections.Add(new Connection("25", "21"));
mapTemplate.Connections.Add(new Connection("37", "13"));
mapTemplate.Connections.Add(new Connection("37", "20"));
mapTemplate.Connections.Add(new Connection("37", "36"));
mapTemplate.Connections.Add(new Connection("37", "11"));
mapTemplate.Connections.Add(new Connection("10", "8"));
mapTemplate.Connections.Add(new Connection("10", "38"));
mapTemplate.Connections.Add(new Connection("10", "44"));
mapTemplate.Connections.Add(new Connection("10", "9"));
mapTemplate.Connections.Add(new Connection("5", "3"));
mapTemplate.Connections.Add(new Connection("5", "14"));

            return mapTemplate;
		}
    }
}

