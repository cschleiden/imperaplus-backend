using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperaPlus.Web.Migrations
{
    public partial class UpdateTournamentTeamFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeam_AspNetUsers_CreatedById",
                table: "TournamentTeam");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentTeam_AspNetUsers_CreatedById",
                table: "TournamentTeam",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeam_AspNetUsers_CreatedById",
                table: "TournamentTeam");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentTeam_AspNetUsers_CreatedById",
                table: "TournamentTeam",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
