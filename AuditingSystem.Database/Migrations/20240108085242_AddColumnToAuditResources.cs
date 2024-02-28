using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddColumnToAuditResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditResources_Departments_DepartmentId",
                table: "AuditResources");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditResources_Functions_FunctionId",
                table: "AuditResources");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditResources_Users_UserId",
                table: "AuditResources");

            migrationBuilder.DropIndex(
                name: "IX_AuditResources_DepartmentId",
                table: "AuditResources");

            migrationBuilder.DropIndex(
                name: "IX_AuditResources_FunctionId",
                table: "AuditResources");

            migrationBuilder.DropColumn(
                name: "Insources",
                table: "Years");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "Years");

            migrationBuilder.DropColumn(
                name: "Outsources",
                table: "Years");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AuditResources",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FunctionId",
                table: "AuditResources",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentId",
                table: "AuditResources",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

       

            

            migrationBuilder.AddColumn<string>(
                name: "InsourceId",
                table: "AuditResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "AuditResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutsourceId",
                table: "AuditResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalActual",
                table: "AuditResources",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPlan",
                table: "AuditResources",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
 
               
            migrationBuilder.AddForeignKey(
                name: "FK_AuditResources_Users_UserId",
                table: "AuditResources",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {  

            migrationBuilder.DropForeignKey(
                name: "FK_AuditResources_Users_UserId",
                table: "AuditResources");

             

            migrationBuilder.DropColumn(
                name: "InsourceId",
                table: "AuditResources");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "AuditResources");

            migrationBuilder.DropColumn(
                name: "OutsourceId",
                table: "AuditResources");

            migrationBuilder.DropColumn(
                name: "TotalActual",
                table: "AuditResources");

            migrationBuilder.DropColumn(
                name: "TotalPlan",
                table: "AuditResources");

        

            migrationBuilder.AddColumn<string>(
                name: "Insources",
                table: "Years",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manager",
                table: "Years",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Outsources",
                table: "Years",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AuditResources",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FunctionId",
                table: "AuditResources",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "AuditResources",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditResources_DepartmentId",
                table: "AuditResources",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResources_FunctionId",
                table: "AuditResources",
                column: "FunctionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResources_Departments_DepartmentId",
                table: "AuditResources",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResources_Functions_FunctionId",
                table: "AuditResources",
                column: "FunctionId",
                principalTable: "Functions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResources_Users_UserId",
                table: "AuditResources",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
