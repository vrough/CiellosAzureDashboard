using Microsoft.EntityFrameworkCore.Migrations;

namespace CiellosAzureDashboard.Migrations
{
    public partial class VMStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InactiveVMs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VMId = table.Column<string>(nullable: true),
                    DashboardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InactiveVMs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VMs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VMId = table.Column<string>(nullable: true),
                    VMName = table.Column<string>(nullable: true),
                    ResourceGroupName = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<string>(nullable: true),
                    VMSize = table.Column<string>(nullable: true),
                    PowerState = table.Column<string>(nullable: true),
                    ProvisioningState = table.Column<string>(nullable: true),
                    ApplicationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VMs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InactiveVMs");

            migrationBuilder.DropTable(
                name: "VMs");
        }
    }
}
