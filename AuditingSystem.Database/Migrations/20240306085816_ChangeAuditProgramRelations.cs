using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class ChangeAuditProgramRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditProgramLists_RiskCategories_RiskCategoryId",
                table: "AuditProgramLists");

            migrationBuilder.DropIndex(
                name: "IX_AuditProgramLists_RiskCategoryId",
                table: "AuditProgramLists");

            migrationBuilder.DropColumn(
                name: "ControlDescription",
                table: "AuditProgramLists");

            migrationBuilder.DropColumn(
                name: "MajorProcess",
                table: "AuditProgramLists");

            migrationBuilder.DropColumn(
                name: "RiskDescription",
                table: "AuditProgramLists");

            migrationBuilder.DropColumn(
                name: "SubProcess",
                table: "AuditProgramLists");

            migrationBuilder.DropColumn(
                name: "TestDescription",
                table: "AuditProgramLists");

            migrationBuilder.RenameColumn(
                name: "RiskCategoryId",
                table: "AuditProgramLists",
                newName: "RiskIdenticationId");

            migrationBuilder.AddColumn<int>(
                name: "ControlId",
                table: "AuditProgramLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RiskIdentificationId",
                table: "AuditProgramLists",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditProgramLists_ControlId",
                table: "AuditProgramLists",
                column: "ControlId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProgramLists_RiskIdentificationId",
                table: "AuditProgramLists",
                column: "RiskIdentificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProgramLists_Controls_ControlId",
                table: "AuditProgramLists",
                column: "ControlId",
                principalTable: "Controls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProgramLists_RiskIdentifications_RiskIdentificationId",
                table: "AuditProgramLists",
                column: "RiskIdentificationId",
                principalTable: "RiskIdentifications",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditProgramLists_Controls_ControlId",
                table: "AuditProgramLists");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditProgramLists_RiskIdentifications_RiskIdentificationId",
                table: "AuditProgramLists");

            migrationBuilder.DropIndex(
                name: "IX_AuditProgramLists_ControlId",
                table: "AuditProgramLists");

            migrationBuilder.DropIndex(
                name: "IX_AuditProgramLists_RiskIdentificationId",
                table: "AuditProgramLists");

            migrationBuilder.DropColumn(
                name: "ControlId",
                table: "AuditProgramLists");

            migrationBuilder.DropColumn(
                name: "RiskIdentificationId",
                table: "AuditProgramLists");

            migrationBuilder.RenameColumn(
                name: "RiskIdenticationId",
                table: "AuditProgramLists",
                newName: "RiskCategoryId");

            migrationBuilder.AddColumn<string>(
                name: "ControlDescription",
                table: "AuditProgramLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MajorProcess",
                table: "AuditProgramLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RiskDescription",
                table: "AuditProgramLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubProcess",
                table: "AuditProgramLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TestDescription",
                table: "AuditProgramLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditProgramLists_RiskCategoryId",
                table: "AuditProgramLists",
                column: "RiskCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProgramLists_RiskCategories_RiskCategoryId",
                table: "AuditProgramLists",
                column: "RiskCategoryId",
                principalTable: "RiskCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
