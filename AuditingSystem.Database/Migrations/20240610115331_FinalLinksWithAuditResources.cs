using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class FinalLinksWithAuditResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinalAuditPlans_DraftAuditPlans_DraftAuditPlanId",
                table: "FinalAuditPlans");

            migrationBuilder.DropIndex(
                name: "IX_FinalAuditPlans_DraftAuditPlanId",
                table: "FinalAuditPlans");

            migrationBuilder.DropColumn(
                name: "DraftAuditPlanId",
                table: "FinalAuditPlans");

            migrationBuilder.RenameColumn(
                name: "DratAuditPlanId",
                table: "FinalAuditPlans",
                newName: "AuditResourcesId");

            migrationBuilder.AlterColumn<int>(
                name: "Plan",
                table: "FinalAuditPlanLists",
                type: "int",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Actual",
                table: "FinalAuditPlanLists",
                type: "int",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinalAuditPlans_AuditResourcesId",
                table: "FinalAuditPlans",
                column: "AuditResourcesId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinalAuditPlans_AuditResources_AuditResourcesId",
                table: "FinalAuditPlans",
                column: "AuditResourcesId",
                principalTable: "AuditResources",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinalAuditPlans_AuditResources_AuditResourcesId",
                table: "FinalAuditPlans");

            migrationBuilder.DropIndex(
                name: "IX_FinalAuditPlans_AuditResourcesId",
                table: "FinalAuditPlans");

            migrationBuilder.RenameColumn(
                name: "AuditResourcesId",
                table: "FinalAuditPlans",
                newName: "DratAuditPlanId");

            migrationBuilder.AddColumn<int>(
                name: "DraftAuditPlanId",
                table: "FinalAuditPlans",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Plan",
                table: "FinalAuditPlanLists",
                type: "bit",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Actual",
                table: "FinalAuditPlanLists",
                type: "bit",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinalAuditPlans_DraftAuditPlanId",
                table: "FinalAuditPlans",
                column: "DraftAuditPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinalAuditPlans_DraftAuditPlans_DraftAuditPlanId",
                table: "FinalAuditPlans",
                column: "DraftAuditPlanId",
                principalTable: "DraftAuditPlans",
                principalColumn: "Id");
        }
    }
}
