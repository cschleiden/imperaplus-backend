using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperaPlus.Web.Migrations
{
    public partial class MoveCountries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Map",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerializedCountries = table.Column<string>(nullable: true),
                    GameId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Map_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Map_GameId",
                table: "Map",
                column: "GameId",
                unique: true);

            migrationBuilder.Sql(
                "INSERT INTO Map (GameId, SerializedCountries) SELECT Id, SerializedCountries FROM Games"
            );

            migrationBuilder.DropColumn(
                name: "SerializedCountries",
                table: "Games");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Map");

            migrationBuilder.AddColumn<string>(
                name: "SerializedCountries",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
