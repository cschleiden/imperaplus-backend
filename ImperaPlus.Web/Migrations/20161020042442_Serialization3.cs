using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperaPlus.Web.Migrations
{
    public partial class Serialization3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountriesSerialized",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "SerializedMapTemplates",
                table: "Tournament",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerializedMapTemplates",
                table: "Tournament");

            migrationBuilder.AddColumn<string>(
                name: "CountriesSerialized",
                table: "Games",
                nullable: true);
        }
    }
}
