using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddIndustryFieldForDeptDBRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Departments_IndustryId",
                table: "Departments",
                column: "IndustryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Industries_IndustryId",
                table: "Departments",
                column: "IndustryId",
                principalTable: "Industries",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Industries_IndustryId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_IndustryId",
                table: "Departments");
        }
    }
}
