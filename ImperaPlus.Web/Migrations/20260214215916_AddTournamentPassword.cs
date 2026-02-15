using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImperaPlus.Web.Migrations
{
    public partial class AddTournamentPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Tournament",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Tournament");
        }
    }
}
