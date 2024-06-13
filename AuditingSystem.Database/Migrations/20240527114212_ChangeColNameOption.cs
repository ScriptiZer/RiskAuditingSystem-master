using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class ChangeColNameOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MigratedCompany",
                table: "RiskAssessmentsList");

            migrationBuilder.RenameColumn(
                name: "SentToYear",
                table: "RiskAssessmentsList",
                newName: "TransferToYear");

            migrationBuilder.RenameColumn(
                name: "MigratedUser",
                table: "RiskAssessmentsList",
                newName: "TransferByCompany");

            migrationBuilder.RenameColumn(
                name: "IsDeported",
                table: "RiskAssessmentsList",
                newName: "IsTransfer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransferToYear",
                table: "RiskAssessmentsList",
                newName: "SentToYear");

            migrationBuilder.RenameColumn(
                name: "TransferByCompany",
                table: "RiskAssessmentsList",
                newName: "MigratedUser");

            migrationBuilder.RenameColumn(
                name: "IsTransfer",
                table: "RiskAssessmentsList",
                newName: "IsDeported");

            migrationBuilder.AddColumn<int>(
                name: "MigratedCompany",
                table: "RiskAssessmentsList",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
