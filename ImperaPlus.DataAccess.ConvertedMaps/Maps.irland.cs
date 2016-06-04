
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
        public static MapTemplate irland()
        {

var mapTemplate = new MapTemplate("irland") { Image = "irland.png" };
var country1 = new CountryTemplate("1", "Londonderry") { X = 559, Y = 158 };
mapTemplate.Countries.Add(country1);
var country2 = new CountryTemplate("2", "Antrim") { X = 621, Y = 122 };
mapTemplate.Countries.Add(country2);
var country3 = new CountryTemplate("3", "Down") { X = 683, Y = 256 };
mapTemplate.Countries.Add(country3);
var country4 = new CountryTemplate("4", "Tyrone") { X = 504, Y = 227 };
mapTemplate.Countries.Add(country4);
var country5 = new CountryTemplate("5", "Armagh") { X = 600, Y = 321 };
mapTemplate.Countries.Add(country5);
var country6 = new CountryTemplate("6", "Fermanagh") { X = 453, Y = 299 };
mapTemplate.Countries.Add(country6);
var country7 = new CountryTemplate("7", "Donegal") { X = 386, Y = 172 };
mapTemplate.Countries.Add(country7);
var country8 = new CountryTemplate("8", "Cavan") { X = 508, Y = 385 };
mapTemplate.Countries.Add(country8);
var country9 = new CountryTemplate("9", "Monaghan") { X = 531, Y = 301 };
mapTemplate.Countries.Add(country9);
var country10 = new CountryTemplate("10", "Sligo") { X = 281, Y = 309 };
mapTemplate.Countries.Add(country10);
var country11 = new CountryTemplate("11", "Mayo") { X = 191, Y = 345 };
mapTemplate.Countries.Add(country11);
var country12 = new CountryTemplate("12", "Leitrim") { X = 402, Y = 342 };
mapTemplate.Countries.Add(country12);
var country13 = new CountryTemplate("13", "Roscommon") { X = 364, Y = 434 };
mapTemplate.Countries.Add(country13);
var country14 = new CountryTemplate("14", "Gaillimh") { X = 307, Y = 533 };
mapTemplate.Countries.Add(country14);
var country15 = new CountryTemplate("15", "Clare") { X = 226, Y = 638 };
mapTemplate.Countries.Add(country15);
var country16 = new CountryTemplate("16", "Limerick") { X = 324, Y = 687 };
mapTemplate.Countries.Add(country16);
var country17 = new CountryTemplate("17", "Kerry") { X = 179, Y = 745 };
mapTemplate.Countries.Add(country17);
var country18 = new CountryTemplate("18", "Cork") { X = 291, Y = 792 };
mapTemplate.Countries.Add(country18);
var country19 = new CountryTemplate("19", "Tipperary") { X = 402, Y = 709 };
mapTemplate.Countries.Add(country19);
var country20 = new CountryTemplate("20", "Waterford") { X = 461, Y = 762 };
mapTemplate.Countries.Add(country20);
var country21 = new CountryTemplate("21", "Louth") { X = 608, Y = 372 };
mapTemplate.Countries.Add(country21);
var country22 = new CountryTemplate("22", "Longford") { X = 444, Y = 405 };
mapTemplate.Countries.Add(country22);
var country23 = new CountryTemplate("23", "Westmeath") { X = 476, Y = 455 };
mapTemplate.Countries.Add(country23);
var country24 = new CountryTemplate("24", "Meath") { X = 571, Y = 465 };
mapTemplate.Countries.Add(country24);
var country25 = new CountryTemplate("25", "Offaly") { X = 424, Y = 549 };
mapTemplate.Countries.Add(country25);
var country26 = new CountryTemplate("26", "Kildare") { X = 558, Y = 515 };
mapTemplate.Countries.Add(country26);
var country27 = new CountryTemplate("27", "Wicklow") { X = 635, Y = 612 };
mapTemplate.Countries.Add(country27);
var country28 = new CountryTemplate("28", "Carlow") { X = 561, Y = 634 };
mapTemplate.Countries.Add(country28);
var country29 = new CountryTemplate("29", "Wexford") { X = 613, Y = 683 };
mapTemplate.Countries.Add(country29);
var country30 = new CountryTemplate("30", "Kilkenny") { X = 496, Y = 658 };
mapTemplate.Countries.Add(country30);
var country31 = new CountryTemplate("31", "Laois") { X = 494, Y = 578 };
mapTemplate.Countries.Add(country31);
var country32 = new CountryTemplate("32", "Baile Atha Cliath") { X = 630, Y = 502 };
mapTemplate.Countries.Add(country32);
var continent1 = new Continent("1", 3);
continent1.Countries.Add(country7);
continent1.Countries.Add(country8);
continent1.Countries.Add(country9);
mapTemplate.Continents.Add(continent1);
var continent2 = new Continent("2", 6);
continent2.Countries.Add(country21);
continent2.Countries.Add(country22);
continent2.Countries.Add(country23);
continent2.Countries.Add(country24);
continent2.Countries.Add(country25);
continent2.Countries.Add(country26);
continent2.Countries.Add(country27);
continent2.Countries.Add(country28);
continent2.Countries.Add(country29);
continent2.Countries.Add(country30);
continent2.Countries.Add(country31);
continent2.Countries.Add(country32);
mapTemplate.Continents.Add(continent2);
var continent3 = new Continent("3", 3);
continent3.Countries.Add(country15);
continent3.Countries.Add(country16);
continent3.Countries.Add(country17);
continent3.Countries.Add(country18);
continent3.Countries.Add(country19);
continent3.Countries.Add(country20);
mapTemplate.Continents.Add(continent3);
var continent4 = new Continent("4", 3);
continent4.Countries.Add(country10);
continent4.Countries.Add(country11);
continent4.Countries.Add(country12);
continent4.Countries.Add(country13);
continent4.Countries.Add(country14);
mapTemplate.Continents.Add(continent4);
var continent5 = new Continent("5", 3);
continent5.Countries.Add(country1);
continent5.Countries.Add(country2);
continent5.Countries.Add(country3);
continent5.Countries.Add(country4);
continent5.Countries.Add(country5);
continent5.Countries.Add(country6);
mapTemplate.Continents.Add(continent5);
mapTemplate.Connections.Add(new Connection("1", "2"));
mapTemplate.Connections.Add(new Connection("1", "4"));
mapTemplate.Connections.Add(new Connection("1", "7"));
mapTemplate.Connections.Add(new Connection("2", "5"));
mapTemplate.Connections.Add(new Connection("2", "3"));
mapTemplate.Connections.Add(new Connection("2", "1"));
mapTemplate.Connections.Add(new Connection("3", "5"));
mapTemplate.Connections.Add(new Connection("3", "2"));
mapTemplate.Connections.Add(new Connection("4", "5"));
mapTemplate.Connections.Add(new Connection("4", "1"));
mapTemplate.Connections.Add(new Connection("4", "7"));
mapTemplate.Connections.Add(new Connection("4", "6"));
mapTemplate.Connections.Add(new Connection("4", "9"));
mapTemplate.Connections.Add(new Connection("5", "2"));
mapTemplate.Connections.Add(new Connection("5", "3"));
mapTemplate.Connections.Add(new Connection("5", "4"));
mapTemplate.Connections.Add(new Connection("5", "9"));
mapTemplate.Connections.Add(new Connection("5", "21"));
mapTemplate.Connections.Add(new Connection("6", "4"));
mapTemplate.Connections.Add(new Connection("6", "7"));
mapTemplate.Connections.Add(new Connection("6", "9"));
mapTemplate.Connections.Add(new Connection("6", "8"));
mapTemplate.Connections.Add(new Connection("6", "12"));
mapTemplate.Connections.Add(new Connection("7", "1"));
mapTemplate.Connections.Add(new Connection("7", "4"));
mapTemplate.Connections.Add(new Connection("7", "6"));
mapTemplate.Connections.Add(new Connection("7", "12"));
mapTemplate.Connections.Add(new Connection("8", "9"));
mapTemplate.Connections.Add(new Connection("8", "6"));
mapTemplate.Connections.Add(new Connection("8", "24"));
mapTemplate.Connections.Add(new Connection("8", "12"));
mapTemplate.Connections.Add(new Connection("8", "22"));
mapTemplate.Connections.Add(new Connection("8", "23"));
mapTemplate.Connections.Add(new Connection("9", "6"));
mapTemplate.Connections.Add(new Connection("9", "5"));
mapTemplate.Connections.Add(new Connection("9", "4"));
mapTemplate.Connections.Add(new Connection("9", "8"));
mapTemplate.Connections.Add(new Connection("9", "21"));
mapTemplate.Connections.Add(new Connection("10", "13"));
mapTemplate.Connections.Add(new Connection("10", "12"));
mapTemplate.Connections.Add(new Connection("10", "11"));
mapTemplate.Connections.Add(new Connection("11", "10"));
mapTemplate.Connections.Add(new Connection("11", "14"));
mapTemplate.Connections.Add(new Connection("11", "13"));
mapTemplate.Connections.Add(new Connection("12", "7"));
mapTemplate.Connections.Add(new Connection("12", "6"));
mapTemplate.Connections.Add(new Connection("12", "10"));
mapTemplate.Connections.Add(new Connection("12", "13"));
mapTemplate.Connections.Add(new Connection("12", "8"));
mapTemplate.Connections.Add(new Connection("12", "22"));
mapTemplate.Connections.Add(new Connection("13", "14"));
mapTemplate.Connections.Add(new Connection("13", "12"));
mapTemplate.Connections.Add(new Connection("13", "11"));
mapTemplate.Connections.Add(new Connection("13", "10"));
mapTemplate.Connections.Add(new Connection("13", "22"));
mapTemplate.Connections.Add(new Connection("13", "23"));
mapTemplate.Connections.Add(new Connection("13", "25"));
mapTemplate.Connections.Add(new Connection("14", "11"));
mapTemplate.Connections.Add(new Connection("14", "13"));
mapTemplate.Connections.Add(new Connection("14", "15"));
mapTemplate.Connections.Add(new Connection("14", "19"));
mapTemplate.Connections.Add(new Connection("14", "25"));
mapTemplate.Connections.Add(new Connection("15", "14"));
mapTemplate.Connections.Add(new Connection("15", "16"));
mapTemplate.Connections.Add(new Connection("15", "19"));
mapTemplate.Connections.Add(new Connection("16", "15"));
mapTemplate.Connections.Add(new Connection("16", "17"));
mapTemplate.Connections.Add(new Connection("16", "18"));
mapTemplate.Connections.Add(new Connection("16", "19"));
mapTemplate.Connections.Add(new Connection("17", "16"));
mapTemplate.Connections.Add(new Connection("17", "18"));
mapTemplate.Connections.Add(new Connection("18", "17"));
mapTemplate.Connections.Add(new Connection("18", "20"));
mapTemplate.Connections.Add(new Connection("18", "16"));
mapTemplate.Connections.Add(new Connection("18", "19"));
mapTemplate.Connections.Add(new Connection("19", "20"));
mapTemplate.Connections.Add(new Connection("19", "16"));
mapTemplate.Connections.Add(new Connection("19", "15"));
mapTemplate.Connections.Add(new Connection("19", "14"));
mapTemplate.Connections.Add(new Connection("19", "18"));
mapTemplate.Connections.Add(new Connection("19", "30"));
mapTemplate.Connections.Add(new Connection("19", "25"));
mapTemplate.Connections.Add(new Connection("19", "31"));
mapTemplate.Connections.Add(new Connection("20", "18"));
mapTemplate.Connections.Add(new Connection("20", "19"));
mapTemplate.Connections.Add(new Connection("20", "30"));
mapTemplate.Connections.Add(new Connection("21", "5"));
mapTemplate.Connections.Add(new Connection("21", "24"));
mapTemplate.Connections.Add(new Connection("21", "9"));
mapTemplate.Connections.Add(new Connection("22", "13"));
mapTemplate.Connections.Add(new Connection("22", "8"));
mapTemplate.Connections.Add(new Connection("22", "12"));
mapTemplate.Connections.Add(new Connection("22", "23"));
mapTemplate.Connections.Add(new Connection("23", "22"));
mapTemplate.Connections.Add(new Connection("23", "8"));
mapTemplate.Connections.Add(new Connection("23", "13"));
mapTemplate.Connections.Add(new Connection("23", "24"));
mapTemplate.Connections.Add(new Connection("23", "25"));
mapTemplate.Connections.Add(new Connection("24", "21"));
mapTemplate.Connections.Add(new Connection("24", "8"));
mapTemplate.Connections.Add(new Connection("24", "23"));
mapTemplate.Connections.Add(new Connection("24", "26"));
mapTemplate.Connections.Add(new Connection("24", "25"));
mapTemplate.Connections.Add(new Connection("24", "32"));
mapTemplate.Connections.Add(new Connection("25", "23"));
mapTemplate.Connections.Add(new Connection("25", "14"));
mapTemplate.Connections.Add(new Connection("25", "13"));
mapTemplate.Connections.Add(new Connection("25", "31"));
mapTemplate.Connections.Add(new Connection("25", "19"));
mapTemplate.Connections.Add(new Connection("25", "24"));
mapTemplate.Connections.Add(new Connection("25", "26"));
mapTemplate.Connections.Add(new Connection("26", "27"));
mapTemplate.Connections.Add(new Connection("26", "31"));
mapTemplate.Connections.Add(new Connection("26", "28"));
mapTemplate.Connections.Add(new Connection("26", "25"));
mapTemplate.Connections.Add(new Connection("26", "24"));
mapTemplate.Connections.Add(new Connection("26", "32"));
mapTemplate.Connections.Add(new Connection("27", "29"));
mapTemplate.Connections.Add(new Connection("27", "28"));
mapTemplate.Connections.Add(new Connection("27", "26"));
mapTemplate.Connections.Add(new Connection("27", "32"));
mapTemplate.Connections.Add(new Connection("28", "29"));
mapTemplate.Connections.Add(new Connection("28", "30"));
mapTemplate.Connections.Add(new Connection("28", "31"));
mapTemplate.Connections.Add(new Connection("28", "27"));
mapTemplate.Connections.Add(new Connection("28", "26"));
mapTemplate.Connections.Add(new Connection("29", "28"));
mapTemplate.Connections.Add(new Connection("29", "30"));
mapTemplate.Connections.Add(new Connection("29", "27"));
mapTemplate.Connections.Add(new Connection("30", "28"));
mapTemplate.Connections.Add(new Connection("30", "19"));
mapTemplate.Connections.Add(new Connection("30", "20"));
mapTemplate.Connections.Add(new Connection("30", "29"));
mapTemplate.Connections.Add(new Connection("30", "31"));
mapTemplate.Connections.Add(new Connection("31", "25"));
mapTemplate.Connections.Add(new Connection("31", "19"));
mapTemplate.Connections.Add(new Connection("31", "30"));
mapTemplate.Connections.Add(new Connection("31", "28"));
mapTemplate.Connections.Add(new Connection("31", "26"));
mapTemplate.Connections.Add(new Connection("32", "27"));
mapTemplate.Connections.Add(new Connection("32", "26"));
mapTemplate.Connections.Add(new Connection("32", "24"));

            return mapTemplate;
		}
    }
}

