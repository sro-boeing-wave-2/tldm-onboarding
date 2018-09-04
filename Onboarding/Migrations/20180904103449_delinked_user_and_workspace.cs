using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Onboarding.Migrations
{
    public partial class delinked_user_and_workspace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "WorkspaceName",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    UserAccountId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkspaceName_UserAccount_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceName_UserAccountId",
                table: "WorkspaceName",
                column: "UserAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkspaceName");

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
    }
}
