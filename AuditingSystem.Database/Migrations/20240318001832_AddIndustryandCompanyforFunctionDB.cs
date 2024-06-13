using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddIndustryandCompanyforFunctionDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Functions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IndustryId",
                table: "Functions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Functions_CompanyId",
                table: "Functions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Functions_IndustryId",
                table: "Functions",
                column: "IndustryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Functions_Companies_CompanyId",
                table: "Functions",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Functions_Industries_IndustryId",
                table: "Functions",
                column: "IndustryId",
                principalTable: "Industries",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Functions_Companies_CompanyId",
                table: "Functions");

            migrationBuilder.DropForeignKey(
                name: "FK_Functions_Industries_IndustryId",
                table: "Functions");

            migrationBuilder.DropIndex(
                name: "IX_Functions_CompanyId",
                table: "Functions");

            migrationBuilder.DropIndex(
                name: "IX_Functions_IndustryId",
                table: "Functions");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Functions");

            migrationBuilder.DropColumn(
                name: "IndustryId",
                table: "Functions");
        }
    }
}
