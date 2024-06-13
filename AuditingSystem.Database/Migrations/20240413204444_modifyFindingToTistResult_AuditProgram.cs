using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class modifyFindingToTistResult_AuditProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Finding",
                table: "AuditProgramLists",
                newName: "TestResult");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TestResult",
                table: "AuditProgramLists",
                newName: "Finding");
        }
    }
}
