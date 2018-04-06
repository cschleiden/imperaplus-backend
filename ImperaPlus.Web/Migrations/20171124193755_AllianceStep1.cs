using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperaPlus.Web.Migrations
{
    public partial class AllianceStep1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Alliance_AllianceId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Alliance");

            migrationBuilder.CreateTable(
                name: "Alliances",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChannelId = table.Column<Guid>(nullable: false),
                    ChannelId1 = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alliances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alliances_Channels_ChannelId1",
                        column: x => x.ChannelId1,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AllianceJoinRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AllianceId = table.Column<Guid>(nullable: false),
                    ApprovedByUserId = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    DeniedByUserId = table.Column<string>(nullable: true),
                    LastModifiedAt = table.Column<DateTime>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    RequestedByUserId = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllianceJoinRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllianceJoinRequest_Alliances_AllianceId",
                        column: x => x.AllianceId,
                        principalTable: "Alliances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllianceJoinRequest_AspNetUsers_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AllianceJoinRequest_AspNetUsers_DeniedByUserId",
                        column: x => x.DeniedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AllianceJoinRequest_AspNetUsers_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alliances_ChannelId1",
                table: "Alliances",
                column: "ChannelId1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AllianceJoinRequest_AllianceId",
                table: "AllianceJoinRequest",
                column: "AllianceId");

            migrationBuilder.CreateIndex(
                name: "IX_AllianceJoinRequest_ApprovedByUserId",
                table: "AllianceJoinRequest",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AllianceJoinRequest_DeniedByUserId",
                table: "AllianceJoinRequest",
                column: "DeniedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AllianceJoinRequest_RequestedByUserId",
                table: "AllianceJoinRequest",
                column: "RequestedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Alliances_AllianceId",
                table: "AspNetUsers",
                column: "AllianceId",
                principalTable: "Alliances",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Alliances_AllianceId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AllianceJoinRequest");

            migrationBuilder.DropTable(
                name: "Alliances");

            migrationBuilder.CreateTable(
                name: "Alliance",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChannelId = table.Column<Guid>(nullable: false),
                    ChannelId1 = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alliance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alliance_Channels_ChannelId1",
                        column: x => x.ChannelId1,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alliance_ChannelId1",
                table: "Alliance",
                column: "ChannelId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Alliance_AllianceId",
                table: "AspNetUsers",
                column: "AllianceId",
                principalTable: "Alliance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
