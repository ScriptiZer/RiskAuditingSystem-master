using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddDraftTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "AuditResourcesListStartEndDates",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "AuditResourcesListStartEndDates",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "AuditResourceId",
                table: "AuditResourcesListStartEndDates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "DraftAuditPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    FunctionId = table.Column<int>(type: "int", nullable: true),
                    IsOneYear = table.Column<bool>(type: "bit", nullable: true),
                    IsTwoYear = table.Column<bool>(type: "bit", nullable: true),
                    IsThreeYear = table.Column<bool>(type: "bit", nullable: true),
                    IA = table.Column<bool>(type: "bit", nullable: true),
                    GovAudit = table.Column<bool>(type: "bit", nullable: true),
                    ELC = table.Column<bool>(type: "bit", nullable: true),
                    RpetitiveFindings = table.Column<bool>(type: "bit", nullable: true),
                    Insource = table.Column<int>(type: "int", nullable: true),
                    Outsource = table.Column<int>(type: "int", nullable: true),
                    Manager = table.Column<int>(type: "int", nullable: true),
                    AuditResourceId = table.Column<int>(type: "int", nullable: true),
                    AuditResourceListId = table.Column<int>(type: "int", nullable: true),
                    AuditResourcesListId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftAuditPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftAuditPlans_AuditResourcesLists_AuditResourcesListId",
                        column: x => x.AuditResourcesListId,
                        principalTable: "AuditResourcesLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DraftAuditPlans_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DraftAuditPlans_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DraftAuditPlans_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditPlans_AuditResourcesListId",
                table: "DraftAuditPlans",
                column: "AuditResourcesListId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditPlans_CompanyId",
                table: "DraftAuditPlans",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditPlans_DepartmentId",
                table: "DraftAuditPlans",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditPlans_FunctionId",
                table: "DraftAuditPlans",
                column: "FunctionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftAuditPlans");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "AuditResourcesListStartEndDates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "AuditResourcesListStartEndDates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuditResourceId",
                table: "AuditResourcesListStartEndDates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
