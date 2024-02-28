using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddAuditResourcesRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AuditResources_CompanyId",
                table: "AuditResources",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResources_DepartmentId",
                table: "AuditResources",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResources_FunctionId",
                table: "AuditResources",
                column: "FunctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResources_Companies_CompanyId",
                table: "AuditResources",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResources_Departments_DepartmentId",
                table: "AuditResources",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResources_Functions_FunctionId",
                table: "AuditResources",
                column: "FunctionId",
                principalTable: "Functions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditResources_Companies_CompanyId",
                table: "AuditResources");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditResources_Departments_DepartmentId",
                table: "AuditResources");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditResources_Functions_FunctionId",
                table: "AuditResources");

            migrationBuilder.DropIndex(
                name: "IX_AuditResources_CompanyId",
                table: "AuditResources");

            migrationBuilder.DropIndex(
                name: "IX_AuditResources_DepartmentId",
                table: "AuditResources");

            migrationBuilder.DropIndex(
                name: "IX_AuditResources_FunctionId",
                table: "AuditResources");
        }
    }
}
