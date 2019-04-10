using Microsoft.EntityFrameworkCore.Migrations;

namespace CiellosAzureDashboard.Migrations
{
    public partial class addLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Link",
                columns: table => new
                {
                    linkId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    linkName = table.Column<string>(nullable: true),
                    linkUrl = table.Column<string>(nullable: true),
                    DashboardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Link", x => x.linkId);
                    table.ForeignKey(
                        name: "FK_Link_Dashboards_DashboardId",
                        column: x => x.DashboardId,
                        principalTable: "Dashboards",
                        principalColumn: "DashboardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Link_DashboardId",
                table: "Link",
                column: "DashboardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Link");
        }
    }
}
