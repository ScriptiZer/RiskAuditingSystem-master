using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class addDraftAuditPlanLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditPlanLists_DraftAuditPlanId",
                table: "DraftAuditPlanLists",
                column: "DraftAuditPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_DraftAuditPlanLists_DraftAuditPlans_DraftAuditPlanId",
                table: "DraftAuditPlanLists",
                column: "DraftAuditPlanId",
                principalTable: "DraftAuditPlans",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DraftAuditPlanLists_DraftAuditPlans_DraftAuditPlanId",
                table: "DraftAuditPlanLists");

            migrationBuilder.DropIndex(
                name: "IX_DraftAuditPlanLists_DraftAuditPlanId",
                table: "DraftAuditPlanLists");
        }
    }
}
