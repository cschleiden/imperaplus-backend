using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperaPlus.Web.Migrations
{
    public partial class ChannelForAlliances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alliances_Channels_ChannelId1",
                table: "Alliances");

            migrationBuilder.DropForeignKey(
                name: "FK_Channels_AspNetUsers_CreatedById",
                table: "Channels");

            migrationBuilder.DropIndex(
                name: "IX_Channels_CreatedById",
                table: "Channels");

            migrationBuilder.DropIndex(
                name: "IX_Alliances_ChannelId1",
                table: "Alliances");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "ChannelId1",
                table: "Alliances");

            migrationBuilder.DropColumn(
                name: "AllianceId",
                table: "Channels");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Alliances_AllianceId",
                table: "Channels");

            migrationBuilder.DropIndex(
                name: "IX_Channels_AllianceId",
                table: "Channels");

            //migrationBuilder.AlterColumn<long>(
            //    name: "AllianceId",
            //    table: "Channels",
            //    nullable: true,
            //    oldClrType: typeof(Guid),
            //    oldNullable: true);

            migrationBuilder.DropColumn(
                name: "AllianceId",
                table: "Channels");

            migrationBuilder.AddColumn<long>(
                name: "AllianceId",
                table: "Channels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Channels",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ChannelId1",
                table: "Alliances",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channels_CreatedById",
                table: "Channels",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Alliances_ChannelId1",
                table: "Alliances",
                column: "ChannelId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Alliances_Channels_ChannelId1",
                table: "Alliances",
                column: "ChannelId1",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_AspNetUsers_CreatedById",
                table: "Channels",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
