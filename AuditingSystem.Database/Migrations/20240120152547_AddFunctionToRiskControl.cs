using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddFunctionToRiskControl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FunctionId",
                table: "RiskIdentifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RiskIdentifications_FunctionId",
                table: "RiskIdentifications",
                column: "FunctionId");

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropIndex(
                name: "IX_RiskIdentifications_FunctionId",
                table: "RiskIdentifications");

            migrationBuilder.DropColumn(
                name: "FunctionId",
                table: "RiskIdentifications");
        }
    }
}
