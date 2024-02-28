using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class ChangeDTAssignedToActualUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToEndActual",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.DropColumn(
                name: "AssignedToStartActual",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.AddColumn<int>(
                name: "AssignedToEndActualId",
                table: "AuditResourcesListStartEndDates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedToStartActualId",
                table: "AuditResourcesListStartEndDates",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToEndActualId",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.DropColumn(
                name: "AssignedToStartActualId",
                table: "AuditResourcesListStartEndDates");

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
    }
}
