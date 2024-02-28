using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class addrolesPermissionsl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Add",
                table: "Permissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "Permissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Edit",
                table: "Permissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "View",
                table: "Permissions",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Add",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Delete",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Edit",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "View",
                table: "Permissions");
        }
    }
}
