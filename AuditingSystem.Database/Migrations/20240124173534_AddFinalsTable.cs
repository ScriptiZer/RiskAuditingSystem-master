using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddFinalsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinalAuditPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    FunctionId = table.Column<int>(type: "int", nullable: true),
                    Insource = table.Column<int>(type: "int", nullable: true),
                    Outsource = table.Column<int>(type: "int", nullable: true),
                    Manager = table.Column<int>(type: "int", nullable: true),
                    DratAuditPlanId = table.Column<int>(type: "int", nullable: true),
                    DraftAuditPlanId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_FinalAuditPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinalAuditPlans_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinalAuditPlans_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinalAuditPlans_DraftAuditPlans_DraftAuditPlanId",
                        column: x => x.DraftAuditPlanId,
                        principalTable: "DraftAuditPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinalAuditPlans_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FinalAuditPlanLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinalAuditPlanId = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quarter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Plan = table.Column<bool>(type: "bit", nullable: true),
                    Actual = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_FinalAuditPlanLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinalAuditPlanLists_FinalAuditPlans_FinalAuditPlanId",
                        column: x => x.FinalAuditPlanId,
                        principalTable: "FinalAuditPlans",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinalAuditPlanLists_FinalAuditPlanId",
                table: "FinalAuditPlanLists",
                column: "FinalAuditPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalAuditPlans_CompanyId",
                table: "FinalAuditPlans",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalAuditPlans_DepartmentId",
                table: "FinalAuditPlans",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalAuditPlans_DraftAuditPlanId",
                table: "FinalAuditPlans",
                column: "DraftAuditPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalAuditPlans_FunctionId",
                table: "FinalAuditPlans",
                column: "FunctionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalAuditPlanLists");

            migrationBuilder.DropTable(
                name: "FinalAuditPlans");
        }
    }
}
