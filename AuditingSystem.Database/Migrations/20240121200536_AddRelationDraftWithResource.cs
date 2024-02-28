using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddRelationDraftWithResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuditResourcesId",
                table: "DraftAuditPlans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DraftAuditPlans_AuditResourcesId",
                table: "DraftAuditPlans",
                column: "AuditResourcesId");

            migrationBuilder.AddForeignKey(
                name: "FK_DraftAuditPlans_AuditResources_AuditResourcesId",
                table: "DraftAuditPlans",
                column: "AuditResourcesId",
                principalTable: "AuditResources",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DraftAuditPlans_AuditResources_AuditResourcesId",
                table: "DraftAuditPlans");

            migrationBuilder.DropIndex(
                name: "IX_DraftAuditPlans_AuditResourcesId",
                table: "DraftAuditPlans");

            migrationBuilder.DropColumn(
                name: "AuditResourcesId",
                table: "DraftAuditPlans");
        }
    }
}
