using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CiellosAzureDashboard.Migrations
{
    public partial class Add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    StopTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleVM",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VMId = table.Column<int>(nullable: false),
                    ScheduleId = table.Column<int>(nullable: false),
                    SchedulesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleVM", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleVM_Schedules_SchedulesId",
                        column: x => x.SchedulesId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduleVM_VMs_VMId",
                        column: x => x.VMId,
                        principalTable: "VMs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleVM_SchedulesId",
                table: "ScheduleVM",
                column: "SchedulesId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleVM_VMId",
                table: "ScheduleVM",
                column: "VMId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleVM");

            migrationBuilder.DropTable(
                name: "Schedules");
        }
    }
}
