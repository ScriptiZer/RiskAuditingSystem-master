using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddNewFirstSecondColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mitigation",
                table: "FirstSecondDuties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Risk",
                table: "FirstSecondDuties",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mitigation",
                table: "FirstSecondDuties");

            migrationBuilder.DropColumn(
                name: "Risk",
                table: "FirstSecondDuties");
        }
    }
}
