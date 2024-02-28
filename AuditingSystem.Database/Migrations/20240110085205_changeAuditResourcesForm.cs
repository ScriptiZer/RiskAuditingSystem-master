using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class changeAuditResourcesForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditResources_Departments_DepartmentId2",
                table: "AuditResources");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditResources_Functions_FunctionId2",
                table: "AuditResources");

            migrationBuilder.DropIndex(
                name: "IX_AuditResources_DepartmentId2",
                table: "AuditResources");

            migrationBuilder.DropIndex(
                name: "IX_AuditResources_FunctionId2",
                table: "AuditResources");

            migrationBuilder.DropColumn(
                name: "DepartmentId2",
                table: "AuditResources");

            migrationBuilder.DropColumn(
                name: "FunctionId2",
                table: "AuditResources");

            migrationBuilder.AlterColumn<int>(
                name: "FunctionId1",
                table: "AuditResources",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId1",
                table: "AuditResources",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditResources_DepartmentId1",
                table: "AuditResources",
                column: "DepartmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResources_FunctionId1",
                table: "AuditResources",
                column: "FunctionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResources_Departments_DepartmentId1",
                table: "AuditResources",
                column: "DepartmentId1",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResources_Functions_FunctionId1",
                table: "AuditResources",
                column: "FunctionId1",
                principalTable: "Functions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditResources_Departments_DepartmentId1",
                table: "AuditResources");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditResources_Functions_FunctionId1",
                table: "AuditResources");

            migrationBuilder.DropIndex(
                name: "IX_AuditResources_DepartmentId1",
                table: "AuditResources");

            migrationBuilder.DropIndex(
                name: "IX_AuditResources_FunctionId1",
                table: "AuditResources");

            migrationBuilder.AlterColumn<string>(
                name: "FunctionId1",
                table: "AuditResources",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId1",
                table: "AuditResources",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId2",
                table: "AuditResources",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FunctionId2",
                table: "AuditResources",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditResources_DepartmentId2",
                table: "AuditResources",
                column: "DepartmentId2");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResources_FunctionId2",
                table: "AuditResources",
                column: "FunctionId2");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResources_Departments_DepartmentId2",
                table: "AuditResources",
                column: "DepartmentId2",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResources_Functions_FunctionId2",
                table: "AuditResources",
                column: "FunctionId2",
                principalTable: "Functions",
                principalColumn: "Id");
        }
    }
}
