using Microsoft.EntityFrameworkCore.Migrations;

namespace CiellosAzureDashboard.Migrations
{
    public partial class addLogging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    logId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    resourcegroup = table.Column<string>(nullable: true),
                    vmname = table.Column<string>(nullable: true),
                    subscription = table.Column<string>(nullable: true),
                    key = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    ip = table.Column<string>(nullable: true),
                    city = table.Column<string>(nullable: true),
                    region = table.Column<string>(nullable: true),
                    country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.logId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}
