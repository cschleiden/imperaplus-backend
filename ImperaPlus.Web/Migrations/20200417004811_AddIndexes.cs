using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperaPlus.Web.Migrations
{
    public partial class AddIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Player_UserId",
                table: "Player");

            migrationBuilder.CreateIndex(
                name: "IX_Player_UserId_IsHidden",
                table: "Player",
                columns: new[] { "UserId", "IsHidden" });

            migrationBuilder.CreateIndex(
                name: "IX_Games_State_CurrentPlayerId",
                table: "Games",
                columns: new[] { "State", "CurrentPlayerId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Player_UserId_IsHidden",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Games_State_CurrentPlayerId",
                table: "Games");

            migrationBuilder.CreateIndex(
                name: "IX_Player_UserId",
                table: "Player",
                column: "UserId");
        }
    }
}
