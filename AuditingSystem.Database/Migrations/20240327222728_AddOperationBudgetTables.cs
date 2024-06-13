using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddOperationBudgetTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
