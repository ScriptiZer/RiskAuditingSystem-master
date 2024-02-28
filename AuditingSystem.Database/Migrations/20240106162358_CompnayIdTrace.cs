using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class CompnayIdTrace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Years",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Years_CompanyId",
                table: "Years",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Years_Companies_CompanyId",
                table: "Years",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Years_Companies_CompanyId",
                table: "Years");

            migrationBuilder.DropIndex(
                name: "IX_Years_CompanyId",
                table: "Years");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Years");
        }
    }
}
