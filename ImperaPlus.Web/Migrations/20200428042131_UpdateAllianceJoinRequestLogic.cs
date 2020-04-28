using Microsoft.EntityFrameworkCore.Migrations;

namespace ImperaPlus.Web.Migrations
{
    public partial class UpdateAllianceJoinRequestLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_ApprovedByUserId",
                table: "AllianceJoinRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_DeniedByUserId",
                table: "AllianceJoinRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_RequestedByUserId",
                table: "AllianceJoinRequest");

            migrationBuilder.AlterColumn<string>(
                name: "RequestedByUserId",
                table: "AllianceJoinRequest",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_RequestedByUserId",
                table: "AllianceJoinRequest",
                column: "RequestedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_ApprovedByUserId",
                table: "AllianceJoinRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_DeniedByUserId",
                table: "AllianceJoinRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_RequestedByUserId",
                table: "AllianceJoinRequest");

            migrationBuilder.AlterColumn<string>(
                name: "RequestedByUserId",
                table: "AllianceJoinRequest",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_ApprovedByUserId",
                table: "AllianceJoinRequest",
                column: "ApprovedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_DeniedByUserId",
                table: "AllianceJoinRequest",
                column: "DeniedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AllianceJoinRequest_AspNetUsers_RequestedByUserId",
                table: "AllianceJoinRequest",
                column: "RequestedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
