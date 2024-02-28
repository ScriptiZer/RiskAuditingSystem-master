using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddAuditResourcesRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AuditResources",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditResources_UserId",
                table: "AuditResources",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResources_Users_UserId",
                table: "AuditResources",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditResources_Users_UserId",
                table: "AuditResources");

            migrationBuilder.DropIndex(
                name: "IX_AuditResources_UserId",
                table: "AuditResources");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AuditResources");
        }
    }
}
