using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class LinksDutieswithSODType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SODTypeId",
                table: "SecondDuties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SODTypeId",
                table: "FirstDuties",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SecondDuties_SODTypeId",
                table: "SecondDuties",
                column: "SODTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FirstDuties_SODTypeId",
                table: "FirstDuties",
                column: "SODTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FirstDuties_SODTypes_SODTypeId",
                table: "FirstDuties",
                column: "SODTypeId",
                principalTable: "SODTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SecondDuties_SODTypes_SODTypeId",
                table: "SecondDuties",
                column: "SODTypeId",
                principalTable: "SODTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FirstDuties_SODTypes_SODTypeId",
                table: "FirstDuties");

            migrationBuilder.DropForeignKey(
                name: "FK_SecondDuties_SODTypes_SODTypeId",
                table: "SecondDuties");

            migrationBuilder.DropIndex(
                name: "IX_SecondDuties_SODTypeId",
                table: "SecondDuties");

            migrationBuilder.DropIndex(
                name: "IX_FirstDuties_SODTypeId",
                table: "FirstDuties");

            migrationBuilder.DropColumn(
                name: "SODTypeId",
                table: "SecondDuties");

            migrationBuilder.DropColumn(
                name: "SODTypeId",
                table: "FirstDuties");
        }
    }
}
