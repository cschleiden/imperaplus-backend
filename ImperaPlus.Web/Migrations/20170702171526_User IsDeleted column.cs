using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperaPlus.Web.Migrations
{
    public partial class UserIsDeletedcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeam_TournamentGroup_TournamentGroupId",
                table: "TournamentTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeam_Tournament_TournamentId1",
                table: "TournamentTeam");

            migrationBuilder.DropIndex(
                name: "IX_TournamentTeam_TournamentGroupId",
                table: "TournamentTeam");

            migrationBuilder.DropIndex(
                name: "IX_TournamentTeam_TournamentId1",
                table: "TournamentTeam");

            migrationBuilder.DropColumn(
                name: "TournamentGroupId",
                table: "TournamentTeam");

            migrationBuilder.DropColumn(
                name: "TournamentId1",
                table: "TournamentTeam");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "TournamentGroupId",
                table: "TournamentTeam",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TournamentId1",
                table: "TournamentTeam",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTeam_TournamentGroupId",
                table: "TournamentTeam",
                column: "TournamentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTeam_TournamentId1",
                table: "TournamentTeam",
                column: "TournamentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentTeam_TournamentGroup_TournamentGroupId",
                table: "TournamentTeam",
                column: "TournamentGroupId",
                principalTable: "TournamentGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentTeam_Tournament_TournamentId1",
                table: "TournamentTeam",
                column: "TournamentId1",
                principalTable: "Tournament",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
