using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperaPlus.Web.Migrations
{
    public partial class GameIdForPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GameId",
                table: "Player",
                nullable: false,
                defaultValue: 0L);

            // Back-fill GameId column
            migrationBuilder.Sql(@"update p set p.GameId = t.GameId from Player p inner join Team t on t.Id = p.TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_GameId_UserId",
                table: "Player",
                columns: new[] { "GameId", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Games_GameId",
                table: "Player",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Games_GameId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_GameId_UserId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Player");
        }
    }
}
