using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class RemoveIndustryLinkFromDraftReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DraftAuditReports_Industries_IndustryId",
                table: "DraftAuditReports");

            migrationBuilder.DropIndex(
                name: "IX_DraftAuditReports_IndustryId",
                table: "DraftAuditReports");

            migrationBuilder.DropColumn(
                name: "IndustryId",
                table: "DraftAuditReports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IndustryId",
                table: "DraftAuditReports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditReports_IndustryId",
                table: "DraftAuditReports",
                column: "IndustryId");

            migrationBuilder.AddForeignKey(
                name: "FK_DraftAuditReports_Industries_IndustryId",
                table: "DraftAuditReports",
                column: "IndustryId",
                principalTable: "Industries",
                principalColumn: "Id");
        }
    }
}
