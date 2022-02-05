using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImperaPlus.Web.Migrations
{
    public partial class RemoveTournamentId1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentGroup_Tournament_TournamentId1",
                table: "TournamentGroup");

            migrationBuilder.DropIndex(
                name: "IX_TournamentGroup_TournamentId1",
                table: "TournamentGroup");

            migrationBuilder.DropColumn(
                name: "TournamentId1",
                table: "TournamentGroup");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TournamentId1",
                table: "TournamentGroup",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TournamentGroup_TournamentId1",
                table: "TournamentGroup",
                column: "TournamentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentGroup_Tournament_TournamentId1",
                table: "TournamentGroup",
                column: "TournamentId1",
                principalTable: "Tournament",
                principalColumn: "Id");
        }
    }
}
