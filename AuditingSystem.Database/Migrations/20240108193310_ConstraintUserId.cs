using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class ConstraintUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditResources_Users_UserId",
                table: "AuditResources");

            migrationBuilder.DropIndex(
                name: "IX_AuditResources_UserId",
                table: "AuditResources");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AuditResources_UserId",
                table: "AuditResources",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResources_Users_UserId",
                table: "AuditResources",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
