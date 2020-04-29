using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperaPlus.Web.Migrations
{
    public partial class UpdateNewsFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsEntries_AspNetUsers_CreatedById",
                table: "NewsEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsEntries_AspNetUsers_CreatedById",
                table: "NewsEntries",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsEntries_AspNetUsers_CreatedById",
                table: "NewsEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsEntries_AspNetUsers_CreatedById",
                table: "NewsEntries",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
