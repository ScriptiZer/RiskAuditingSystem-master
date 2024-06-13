using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class modify_addColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BudgetOperationInsources",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "BudgetOperationManager",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "BudgetOperationOutsources",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "BudgetOperationPlanYear",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "BudgetType",
                table: "AuditBudgetOperation");

            migrationBuilder.AddColumn<int>(
                name: "OperationBudgetTypeId",
                table: "AuditBudgetOperation",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditBudgetOperation_OperationBudgetTypeId",
                table: "AuditBudgetOperation",
                column: "OperationBudgetTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditBudgetOperation_OperationBudgetTypes_OperationBudgetTypeId",
                table: "AuditBudgetOperation",
                column: "OperationBudgetTypeId",
                principalTable: "OperationBudgetTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditBudgetOperation_OperationBudgetTypes_OperationBudgetTypeId",
                table: "AuditBudgetOperation");

            migrationBuilder.DropIndex(
                name: "IX_AuditBudgetOperation_OperationBudgetTypeId",
                table: "AuditBudgetOperation");

            migrationBuilder.DropColumn(
                name: "OperationBudgetTypeId",
                table: "AuditBudgetOperation");

            migrationBuilder.AddColumn<string>(
                name: "BudgetOperationInsources",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BudgetOperationManager",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BudgetOperationOutsources",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BudgetOperationPlanYear",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BudgetType",
                table: "AuditBudgetOperation",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
