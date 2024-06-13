using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class RemoveImpactRiskAsALink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DraftAuditReports_RiskImpacts_RiskImpactId",
                table: "DraftAuditReports");

            migrationBuilder.DropIndex(
                name: "IX_DraftAuditReports_RiskImpactId",
                table: "DraftAuditReports");

            migrationBuilder.DropColumn(
                name: "RiskImpactId",
                table: "DraftAuditReports");

            migrationBuilder.AddColumn<string>(
                name: "RiskImpact",
                table: "DraftAuditReports",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiskImpact",
                table: "DraftAuditReports");

            migrationBuilder.AddColumn<int>(
                name: "RiskImpactId",
                table: "DraftAuditReports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditReports_RiskImpactId",
                table: "DraftAuditReports",
                column: "RiskImpactId");

            migrationBuilder.AddForeignKey(
                name: "FK_DraftAuditReports_RiskImpacts_RiskImpactId",
                table: "DraftAuditReports",
                column: "RiskImpactId",
                principalTable: "RiskImpacts",
                principalColumn: "Id");
        }
    }
}
