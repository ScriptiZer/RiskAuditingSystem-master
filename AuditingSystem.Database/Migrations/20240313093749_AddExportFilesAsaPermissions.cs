using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddExportFilesAsaPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ExportToExcel",
                table: "RolesPermissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExportToPDF",
                table: "RolesPermissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExportToWord",
                table: "RolesPermissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExportToExcel",
                table: "Permissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExportToPDF",
                table: "Permissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExportToWord",
                table: "Permissions",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExportToExcel",
                table: "RolesPermissions");

            migrationBuilder.DropColumn(
                name: "ExportToPDF",
                table: "RolesPermissions");

            migrationBuilder.DropColumn(
                name: "ExportToWord",
                table: "RolesPermissions");

            migrationBuilder.DropColumn(
                name: "ExportToExcel",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "ExportToPDF",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "ExportToWord",
                table: "Permissions");
        }
    }
}
