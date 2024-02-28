using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class ChangeBudgetRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AuditBudgets");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AuditBudgets");

            migrationBuilder.AddColumn<int>(
                name: "AuditResourcesListId",
                table: "AuditBudgets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditBudgets_AuditResourcesListId",
                table: "AuditBudgets",
                column: "AuditResourcesListId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditBudgets_UserId",
                table: "AuditBudgets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditBudgets_AuditResourcesLists_AuditResourcesListId",
                table: "AuditBudgets",
                column: "AuditResourcesListId",
                principalTable: "AuditResourcesLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditBudgets_Users_UserId",
                table: "AuditBudgets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditBudgets_AuditResourcesLists_AuditResourcesListId",
                table: "AuditBudgets");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditBudgets_Users_UserId",
                table: "AuditBudgets");

            migrationBuilder.DropIndex(
                name: "IX_AuditBudgets_AuditResourcesListId",
                table: "AuditBudgets");

            migrationBuilder.DropIndex(
                name: "IX_AuditBudgets_UserId",
                table: "AuditBudgets");

            migrationBuilder.DropColumn(
                name: "AuditResourcesListId",
                table: "AuditBudgets");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "AuditBudgets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "AuditBudgets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
