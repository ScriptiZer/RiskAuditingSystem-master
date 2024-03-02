using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class addCompanyandDepartmentforRiskAssessments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "RiskIdentifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "RiskIdentifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RiskIdentifications_CompanyId",
                table: "RiskIdentifications",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskIdentifications_DepartmentId",
                table: "RiskIdentifications",
                column: "DepartmentId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropIndex(
                name: "IX_RiskIdentifications_CompanyId",
                table: "RiskIdentifications");

            migrationBuilder.DropIndex(
                name: "IX_RiskIdentifications_DepartmentId",
                table: "RiskIdentifications");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "RiskIdentifications");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "RiskIdentifications");
        }
    }
}
