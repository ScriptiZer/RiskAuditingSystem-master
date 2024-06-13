using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddActivityRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Activities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Activities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IndustryId",
                table: "Activities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_CompanyId",
                table: "Activities",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_DepartmentId",
                table: "Activities",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_IndustryId",
                table: "Activities",
                column: "IndustryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Companies_CompanyId",
                table: "Activities",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Departments_DepartmentId",
                table: "Activities",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Industries_IndustryId",
                table: "Activities",
                column: "IndustryId",
                principalTable: "Industries",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Companies_CompanyId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Departments_DepartmentId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Industries_IndustryId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_CompanyId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_DepartmentId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_IndustryId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "IndustryId",
                table: "Activities");
        }
    }
}
