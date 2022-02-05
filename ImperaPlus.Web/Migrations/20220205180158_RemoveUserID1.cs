using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImperaPlus.Web.Migrations
{
    public partial class RemoveUserID1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LadderStanding_AspNetUsers_UserId1",
                table: "LadderStanding");

            migrationBuilder.DropIndex(
                name: "IX_LadderStanding_UserId1",
                table: "LadderStanding");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "LadderStanding");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "LadderStanding",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LadderStanding_UserId1",
                table: "LadderStanding",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_LadderStanding_AspNetUsers_UserId1",
                table: "LadderStanding",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
