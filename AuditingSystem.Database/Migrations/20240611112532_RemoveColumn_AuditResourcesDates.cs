using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class RemoveColumn_AuditResourcesDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToEndActualId",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.DropColumn(
                name: "AssignedToStartActualId",
                table: "AuditResourcesListStartEndDates");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
