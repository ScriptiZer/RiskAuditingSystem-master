using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class addQuartertoYearForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Years_Companies_CompanyId",
                table: "Years");

            migrationBuilder.DropForeignKey(
                name: "FK_Years_Departments_DepartmentId",
                table: "Years");

            migrationBuilder.DropIndex(
                name: "IX_Years_CompanyId",
                table: "Years");

            migrationBuilder.DropIndex(
                name: "IX_Years_DepartmentId",
                table: "Years");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Years");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Years");

            migrationBuilder.AddColumn<string>(
                name: "Quarters",
                table: "Years",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quarters",
                table: "Years");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Years",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Years",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Years_CompanyId",
                table: "Years",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Years_DepartmentId",
                table: "Years",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Years_Companies_CompanyId",
                table: "Years",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Years_Departments_DepartmentId",
                table: "Years",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
