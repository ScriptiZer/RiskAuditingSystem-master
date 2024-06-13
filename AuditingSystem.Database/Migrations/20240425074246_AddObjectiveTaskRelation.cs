using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddObjectiveTaskRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FunctionId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IndustryId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Objectives",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Objectives",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FunctionId",
                table: "Objectives",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IndustryId",
                table: "Objectives",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ActivityId",
                table: "Tasks",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CompanyId",
                table: "Tasks",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_DepartmentId",
                table: "Tasks",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_FunctionId",
                table: "Tasks",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_IndustryId",
                table: "Tasks",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_Objectives_CompanyId",
                table: "Objectives",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Objectives_DepartmentId",
                table: "Objectives",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Objectives_FunctionId",
                table: "Objectives",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Objectives_IndustryId",
                table: "Objectives",
                column: "IndustryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Objectives_Companies_CompanyId",
                table: "Objectives",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Objectives_Departments_DepartmentId",
                table: "Objectives",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Objectives_Functions_FunctionId",
                table: "Objectives",
                column: "FunctionId",
                principalTable: "Functions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Objectives_Industries_IndustryId",
                table: "Objectives",
                column: "IndustryId",
                principalTable: "Industries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Activities_ActivityId",
                table: "Tasks",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Companies_CompanyId",
                table: "Tasks",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Departments_DepartmentId",
                table: "Tasks",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Functions_FunctionId",
                table: "Tasks",
                column: "FunctionId",
                principalTable: "Functions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Industries_IndustryId",
                table: "Tasks",
                column: "IndustryId",
                principalTable: "Industries",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_Companies_CompanyId",
                table: "Objectives");

            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_Departments_DepartmentId",
                table: "Objectives");

            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_Functions_FunctionId",
                table: "Objectives");

            migrationBuilder.DropForeignKey(
                name: "FK_Objectives_Industries_IndustryId",
                table: "Objectives");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Activities_ActivityId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Companies_CompanyId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Departments_DepartmentId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Functions_FunctionId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Industries_IndustryId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ActivityId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_CompanyId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_DepartmentId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_FunctionId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_IndustryId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Objectives_CompanyId",
                table: "Objectives");

            migrationBuilder.DropIndex(
                name: "IX_Objectives_DepartmentId",
                table: "Objectives");

            migrationBuilder.DropIndex(
                name: "IX_Objectives_FunctionId",
                table: "Objectives");

            migrationBuilder.DropIndex(
                name: "IX_Objectives_IndustryId",
                table: "Objectives");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "FunctionId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "IndustryId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Objectives");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Objectives");

            migrationBuilder.DropColumn(
                name: "FunctionId",
                table: "Objectives");

            migrationBuilder.DropColumn(
                name: "IndustryId",
                table: "Objectives");
        }
    }
}
