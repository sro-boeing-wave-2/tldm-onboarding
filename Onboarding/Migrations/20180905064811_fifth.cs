using Microsoft.EntityFrameworkCore.Migrations;

namespace Onboarding.Migrations
{
    public partial class fifth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkspaceId",
                table: "UserAccount",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_WorkspaceId",
                table: "UserAccount",
                column: "WorkspaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccount_Workspace_WorkspaceId",
                table: "UserAccount",
                column: "WorkspaceId",
                principalTable: "Workspace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccount_Workspace_WorkspaceId",
                table: "UserAccount");

            migrationBuilder.DropIndex(
                name: "IX_UserAccount_WorkspaceId",
                table: "UserAccount");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "UserAccount");
        }
    }
}
