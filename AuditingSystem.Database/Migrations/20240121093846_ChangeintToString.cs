using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class ChangeintToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditResourcesListStartEndDates_Quarters_QuarterId",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditResourcesListStartEndDates_Years_YearId",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.DropIndex(
                name: "IX_AuditResourcesListStartEndDates_QuarterId",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.DropIndex(
                name: "IX_AuditResourcesListStartEndDates_YearId",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.AlterColumn<string>(
                name: "YearId",
                table: "AuditResourcesListStartEndDates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "QuarterId",
                table: "AuditResourcesListStartEndDates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "QuarterId1",
                table: "AuditResourcesListStartEndDates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearId1",
                table: "AuditResourcesListStartEndDates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditResourcesListStartEndDates_QuarterId1",
                table: "AuditResourcesListStartEndDates",
                column: "QuarterId1");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResourcesListStartEndDates_YearId1",
                table: "AuditResourcesListStartEndDates",
                column: "YearId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResourcesListStartEndDates_Quarters_QuarterId1",
                table: "AuditResourcesListStartEndDates",
                column: "QuarterId1",
                principalTable: "Quarters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResourcesListStartEndDates_Years_YearId1",
                table: "AuditResourcesListStartEndDates",
                column: "YearId1",
                principalTable: "Years",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditResourcesListStartEndDates_Quarters_QuarterId1",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditResourcesListStartEndDates_Years_YearId1",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.DropIndex(
                name: "IX_AuditResourcesListStartEndDates_QuarterId1",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.DropIndex(
                name: "IX_AuditResourcesListStartEndDates_YearId1",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.DropColumn(
                name: "QuarterId1",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.DropColumn(
                name: "YearId1",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.AlterColumn<int>(
                name: "YearId",
                table: "AuditResourcesListStartEndDates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QuarterId",
                table: "AuditResourcesListStartEndDates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditResourcesListStartEndDates_QuarterId",
                table: "AuditResourcesListStartEndDates",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResourcesListStartEndDates_YearId",
                table: "AuditResourcesListStartEndDates",
                column: "YearId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResourcesListStartEndDates_Quarters_QuarterId",
                table: "AuditResourcesListStartEndDates",
                column: "QuarterId",
                principalTable: "Quarters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResourcesListStartEndDates_Years_YearId",
                table: "AuditResourcesListStartEndDates",
                column: "YearId",
                principalTable: "Years",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
