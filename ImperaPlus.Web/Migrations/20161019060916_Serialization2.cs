using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperaPlus.Web.Migrations
{
    public partial class Serialization2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerializedMapTemplates",
                table: "Ladders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerializedVictoryConditions",
                table: "GameOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerializedVisibilityModifier",
                table: "GameOptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerializedCountries",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerializedMapTemplates",
                table: "Ladders");

            migrationBuilder.DropColumn(
                name: "SerializedVictoryConditions",
                table: "GameOptions");

            migrationBuilder.DropColumn(
                name: "SerializedVisibilityModifier",
                table: "GameOptions");

            migrationBuilder.DropColumn(
                name: "SerializedCountries",
                table: "Games");
        }
    }
}
