using Microsoft.EntityFrameworkCore.Migrations;

namespace Onboarding.Migrations
{
    public partial class fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserAccountId",
                table: "Workspace",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workspace_UserAccountId",
                table: "Workspace",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workspace_UserAccount_UserAccountId",
                table: "Workspace",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workspace_UserAccount_UserAccountId",
                table: "Workspace");

            migrationBuilder.DropIndex(
                name: "IX_Workspace_UserAccountId",
                table: "Workspace");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                table: "Workspace");
        }
    }
}
