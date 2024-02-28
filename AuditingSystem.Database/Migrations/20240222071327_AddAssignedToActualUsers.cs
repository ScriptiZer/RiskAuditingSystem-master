using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddAssignedToActualUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedToEndActual",
                table: "AuditResourcesListStartEndDates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedToStartActual",
                table: "AuditResourcesListStartEndDates",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToEndActual",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.DropColumn(
                name: "AssignedToStartActual",
                table: "AuditResourcesListStartEndDates");
        }
    }
}
