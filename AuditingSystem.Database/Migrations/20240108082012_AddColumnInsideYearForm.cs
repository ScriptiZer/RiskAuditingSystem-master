using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddColumnInsideYearForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            


            migrationBuilder.AddColumn<string>(
                name: "Insources",
                table: "Years",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manager",
                table: "Years",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Outsources",
                table: "Years",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Insources",
                table: "Years");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "Years");

            migrationBuilder.DropColumn(
                name: "Outsources",
                table: "Years");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Years",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Years_UserId",
                table: "Years",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Years_Users_UserId",
                table: "Years",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
