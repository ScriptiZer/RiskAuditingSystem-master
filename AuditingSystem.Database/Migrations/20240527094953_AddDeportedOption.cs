using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddDeportedOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeported",
                table: "RiskAssessmentsList",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MigratedCompany",
                table: "RiskAssessmentsList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MigratedUser",
                table: "RiskAssessmentsList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SentToYear",
                table: "RiskAssessmentsList",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeported",
                table: "RiskAssessmentsList");

            migrationBuilder.DropColumn(
                name: "MigratedCompany",
                table: "RiskAssessmentsList");

            migrationBuilder.DropColumn(
                name: "MigratedUser",
                table: "RiskAssessmentsList");

            migrationBuilder.DropColumn(
                name: "SentToYear",
                table: "RiskAssessmentsList");
        }
    }
}
