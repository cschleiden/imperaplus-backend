using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperaPlus.Web.Migrations
{
    public partial class FixChannelAllianceMapping2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Alliances_AllianceId",
                table: "Channels");

            migrationBuilder.DropIndex(
                name: "IX_Channels_AllianceId",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "AllianceId",
                table: "Channels");

            migrationBuilder.CreateIndex(
                name: "IX_Alliances_ChannelId",
                table: "Alliances",
                column: "ChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alliances_Channels_ChannelId",
                table: "Alliances",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alliances_Channels_ChannelId",
                table: "Alliances");

            migrationBuilder.DropIndex(
                name: "IX_Alliances_ChannelId",
                table: "Alliances");

            migrationBuilder.AddColumn<Guid>(
                name: "AllianceId",
                table: "Channels",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channels_AllianceId",
                table: "Channels",
                column: "AllianceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Alliances_AllianceId",
                table: "Channels",
                column: "AllianceId",
                principalTable: "Alliances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
