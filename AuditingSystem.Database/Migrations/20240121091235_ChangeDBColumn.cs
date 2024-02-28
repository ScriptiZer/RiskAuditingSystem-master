using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class ChangeDBColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quarters_AuditResources_AuditResourcesId",
                table: "Quarters");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AuditResources_AuditResourcesId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AuditResources_AuditResourcesId1",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AuditResources_AuditResourcesId2",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Years_AuditResources_AuditResourcesId",
                table: "Years");

            migrationBuilder.DropIndex(
                name: "IX_Years_AuditResourcesId",
                table: "Years");

            migrationBuilder.DropIndex(
                name: "IX_Users_AuditResourcesId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AuditResourcesId1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AuditResourcesId2",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Quarters_AuditResourcesId",
                table: "Quarters");

            migrationBuilder.DropColumn(
                name: "AuditResourcesId",
                table: "Years");

            migrationBuilder.DropColumn(
                name: "AuditResourcesId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AuditResourcesId1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AuditResourcesId2",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AuditResourcesId",
                table: "Quarters");

            migrationBuilder.DropColumn(
                name: "InsourceId",
                table: "AuditResources");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "AuditResources");

            migrationBuilder.DropColumn(
                name: "OutsourceId",
                table: "AuditResources");

            migrationBuilder.DropColumn(
                name: "QuartersId",
                table: "AuditResources");

            migrationBuilder.DropColumn(
                name: "TotalActual",
                table: "AuditResources");

            migrationBuilder.DropColumn(
                name: "TotalPlan",
                table: "AuditResources");

            migrationBuilder.DropColumn(
                name: "YearsId",
                table: "AuditResources");

            migrationBuilder.CreateTable(
                name: "AuditResourcesListStartEndDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditResourceId = table.Column<int>(type: "int", nullable: false),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    QuarterId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditResourcesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditResourcesListStartEndDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditResourcesListStartEndDates_AuditResources_AuditResourcesId",
                        column: x => x.AuditResourcesId,
                        principalTable: "AuditResources",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuditResourcesListStartEndDates_Quarters_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "Quarters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditResourcesListStartEndDates_Years_YearId",
                        column: x => x.YearId,
                        principalTable: "Years",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditResourcesListStartEndDates_AuditResourcesId",
                table: "AuditResourcesListStartEndDates",
                column: "AuditResourcesId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResourcesListStartEndDates_QuarterId",
                table: "AuditResourcesListStartEndDates",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResourcesListStartEndDates_YearId",
                table: "AuditResourcesListStartEndDates",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditResourcesListStartEndDates");

            migrationBuilder.AddColumn<int>(
                name: "AuditResourcesId",
                table: "Years",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuditResourcesId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuditResourcesId1",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuditResourcesId2",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuditResourcesId",
                table: "Quarters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsourceId",
                table: "AuditResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "AuditResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutsourceId",
                table: "AuditResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuartersId",
                table: "AuditResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalActual",
                table: "AuditResources",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalPlan",
                table: "AuditResources",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YearsId",
                table: "AuditResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Years_AuditResourcesId",
                table: "Years",
                column: "AuditResourcesId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuditResourcesId",
                table: "Users",
                column: "AuditResourcesId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuditResourcesId1",
                table: "Users",
                column: "AuditResourcesId1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuditResourcesId2",
                table: "Users",
                column: "AuditResourcesId2");

            migrationBuilder.CreateIndex(
                name: "IX_Quarters_AuditResourcesId",
                table: "Quarters",
                column: "AuditResourcesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quarters_AuditResources_AuditResourcesId",
                table: "Quarters",
                column: "AuditResourcesId",
                principalTable: "AuditResources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AuditResources_AuditResourcesId",
                table: "Users",
                column: "AuditResourcesId",
                principalTable: "AuditResources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AuditResources_AuditResourcesId1",
                table: "Users",
                column: "AuditResourcesId1",
                principalTable: "AuditResources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AuditResources_AuditResourcesId2",
                table: "Users",
                column: "AuditResourcesId2",
                principalTable: "AuditResources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Years_AuditResources_AuditResourcesId",
                table: "Years",
                column: "AuditResourcesId",
                principalTable: "AuditResources",
                principalColumn: "Id");
        }
    }
}
