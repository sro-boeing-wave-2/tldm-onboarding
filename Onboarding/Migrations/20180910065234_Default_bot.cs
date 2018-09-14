using Microsoft.EntityFrameworkCore.Migrations;

namespace Onboarding.Migrations
{
    public partial class Default_bot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bot",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Info = table.Column<string>(nullable: true),
                    Developer = table.Column<string>(nullable: true),
                    AppUrl = table.Column<string>(nullable: true),
                    LogoUrl = table.Column<string>(nullable: true),
                    WorkspaceId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bot_Workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bot_WorkspaceId",
                table: "Bot",
                column: "WorkspaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bot");
        }
    }
}
