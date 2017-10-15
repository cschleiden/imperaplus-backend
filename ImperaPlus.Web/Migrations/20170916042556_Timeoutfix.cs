using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperaPlus.Web.Migrations
{
    public partial class Timeoutfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameChatMessage_AspNetUsers_UserId",
                table: "GameChatMessage");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastTurnStartedAt",
                table: "Games",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.Sql("UPDATE Games SET LastTurnStartedAt = LastModifiedAt");

            migrationBuilder.AddForeignKey(
                name: "FK_GameChatMessage_AspNetUsers_UserId",
                table: "GameChatMessage",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameChatMessage_AspNetUsers_UserId",
                table: "GameChatMessage");

            migrationBuilder.DropColumn(
                name: "LastTurnStartedAt",
                table: "Games");

            migrationBuilder.AddForeignKey(
                name: "FK_GameChatMessage_AspNetUsers_UserId",
                table: "GameChatMessage",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
