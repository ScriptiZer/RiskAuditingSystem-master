using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class addrolesPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "Add",
                table: "RolesPermissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "RolesPermissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Edit",
                table: "RolesPermissions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "View",
                table: "RolesPermissions",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Add",
                table: "RolesPermissions");

            migrationBuilder.DropColumn(
                name: "Delete",
                table: "RolesPermissions");

            migrationBuilder.DropColumn(
                name: "Edit",
                table: "RolesPermissions");

            migrationBuilder.DropColumn(
                name: "View",
                table: "RolesPermissions");

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
    }
}
