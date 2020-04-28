using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperaPlus.Web.Migrations
{
    public partial class UpdateAllianceJoinRequestLogic2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_ApprovedByUserId",
                table: "AllianceJoinRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_DeniedByUserId",
                table: "AllianceJoinRequest");

            migrationBuilder.AddForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_ApprovedByUserId",
                table: "AllianceJoinRequest",
                column: "ApprovedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_DeniedByUserId",
                table: "AllianceJoinRequest",
                column: "DeniedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_ApprovedByUserId",
                table: "AllianceJoinRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_DeniedByUserId",
                table: "AllianceJoinRequest");

            migrationBuilder.AddForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_ApprovedByUserId",
                table: "AllianceJoinRequest",
                column: "ApprovedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_DeniedByUserId",
                table: "AllianceJoinRequest",
                column: "DeniedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
