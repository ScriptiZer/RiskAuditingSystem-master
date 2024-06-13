using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddAuditReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DraftAuditReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndustryId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    FunctionId = table.Column<int>(type: "int", nullable: true),
                    FindingId = table.Column<int>(type: "int", nullable: true),
                    ObservationId = table.Column<int>(type: "int", nullable: true),
                    Significance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskImpactId = table.Column<int>(type: "int", nullable: true),
                    RiskCategoryId = table.Column<int>(type: "int", nullable: true),
                    Recomendation = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_DraftAuditReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftAuditReports_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DraftAuditReports_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DraftAuditReports_Findings_FindingId",
                        column: x => x.FindingId,
                        principalTable: "Findings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DraftAuditReports_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DraftAuditReports_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DraftAuditReports_Observations_ObservationId",
                        column: x => x.ObservationId,
                        principalTable: "Observations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DraftAuditReports_RiskCategories_RiskCategoryId",
                        column: x => x.RiskCategoryId,
                        principalTable: "RiskCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DraftAuditReports_RiskImpacts_RiskImpactId",
                        column: x => x.RiskImpactId,
                        principalTable: "RiskImpacts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientActionPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManagementAcceptance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    DraftAuditReportsId = table.Column<int>(type: "int", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_ClientActionPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientActionPlans_DraftAuditReports_DraftAuditReportsId",
                        column: x => x.DraftAuditReportsId,
                        principalTable: "DraftAuditReports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClientActionPlans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FinalAuditReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DraftAuditPlanId = table.Column<int>(type: "int", nullable: true),
                    ClientActionPlansId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_FinalAuditReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinalAuditReports_ClientActionPlans_ClientActionPlansId",
                        column: x => x.ClientActionPlansId,
                        principalTable: "ClientActionPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinalAuditReports_DraftAuditPlans_DraftAuditPlanId",
                        column: x => x.DraftAuditPlanId,
                        principalTable: "DraftAuditPlans",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientActionPlans_DraftAuditReportsId",
                table: "ClientActionPlans",
                column: "DraftAuditReportsId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientActionPlans_UserId",
                table: "ClientActionPlans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditReports_CompanyId",
                table: "DraftAuditReports",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditReports_DepartmentId",
                table: "DraftAuditReports",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditReports_FindingId",
                table: "DraftAuditReports",
                column: "FindingId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditReports_FunctionId",
                table: "DraftAuditReports",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditReports_IndustryId",
                table: "DraftAuditReports",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditReports_ObservationId",
                table: "DraftAuditReports",
                column: "ObservationId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditReports_RiskCategoryId",
                table: "DraftAuditReports",
                column: "RiskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditReports_RiskImpactId",
                table: "DraftAuditReports",
                column: "RiskImpactId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalAuditReports_ClientActionPlansId",
                table: "FinalAuditReports",
                column: "ClientActionPlansId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalAuditReports_DraftAuditPlanId",
                table: "FinalAuditReports",
                column: "DraftAuditPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalAuditReports");

            migrationBuilder.DropTable(
                name: "ClientActionPlans");

            migrationBuilder.DropTable(
                name: "DraftAuditReports");
        }
    }
}
