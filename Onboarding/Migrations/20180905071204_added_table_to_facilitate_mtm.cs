using Microsoft.EntityFrameworkCore.Migrations;

namespace Onboarding.Migrations
{
    public partial class added_table_to_facilitate_mtm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "UserWorkspace",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    WorkspaceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWorkspace", x => new { x.UserId, x.WorkspaceId });
                    table.ForeignKey(
                        name: "FK_UserWorkspace_UserAccount_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWorkspace_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkspace_WorkspaceId",
                table: "UserWorkspace",
                column: "WorkspaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWorkspace");

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
    }
}
