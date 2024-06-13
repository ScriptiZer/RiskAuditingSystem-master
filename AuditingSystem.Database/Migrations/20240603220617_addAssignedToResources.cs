using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class addAssignedToResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedToUserId",
                table: "AuditResources",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditResources_AssignedToUserId",
                table: "AuditResources",
                column: "AssignedToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResources_Users_AssignedToUserId",
                table: "AuditResources",
                column: "AssignedToUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditResources_Users_AssignedToUserId",
                table: "AuditResources");

            migrationBuilder.DropIndex(
                name: "IX_AuditResources_AssignedToUserId",
                table: "AuditResources");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "AuditResources");
        }
    }
}
