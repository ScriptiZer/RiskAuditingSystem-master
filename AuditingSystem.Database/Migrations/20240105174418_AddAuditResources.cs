using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddAuditResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuditResourcesId",
                table: "Years",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuditResourcesId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuditResourcesId1",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuditResourcesId2",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuditResourcesId",
                table: "Quarters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditResources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    FunctionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditResources", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Years_AuditResourcesId",
                table: "Years",
                column: "AuditResourcesId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuditResourcesId",
                table: "Users",
                column: "AuditResourcesId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuditResourcesId1",
                table: "Users",
                column: "AuditResourcesId1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuditResourcesId2",
                table: "Users",
                column: "AuditResourcesId2");

            migrationBuilder.CreateIndex(
                name: "IX_Quarters_AuditResourcesId",
                table: "Quarters",
                column: "AuditResourcesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quarters_AuditResources_AuditResourcesId",
                table: "Quarters",
                column: "AuditResourcesId",
                principalTable: "AuditResources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AuditResources_AuditResourcesId",
                table: "Users",
                column: "AuditResourcesId",
                principalTable: "AuditResources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AuditResources_AuditResourcesId1",
                table: "Users",
                column: "AuditResourcesId1",
                principalTable: "AuditResources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AuditResources_AuditResourcesId2",
                table: "Users",
                column: "AuditResourcesId2",
                principalTable: "AuditResources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Years_AuditResources_AuditResourcesId",
                table: "Years",
                column: "AuditResourcesId",
                principalTable: "AuditResources",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quarters_AuditResources_AuditResourcesId",
                table: "Quarters");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AuditResources_AuditResourcesId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AuditResources_AuditResourcesId1",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_AuditResources_AuditResourcesId2",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Years_AuditResources_AuditResourcesId",
                table: "Years");

            migrationBuilder.DropTable(
                name: "AuditResources");

            migrationBuilder.DropIndex(
                name: "IX_Years_AuditResourcesId",
                table: "Years");

            migrationBuilder.DropIndex(
                name: "IX_Users_AuditResourcesId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AuditResourcesId1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AuditResourcesId2",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Quarters_AuditResourcesId",
                table: "Quarters");

            migrationBuilder.DropColumn(
                name: "AuditResourcesId",
                table: "Years");

            migrationBuilder.DropColumn(
                name: "AuditResourcesId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AuditResourcesId1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AuditResourcesId2",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AuditResourcesId",
                table: "Quarters");
        }
    }
}
