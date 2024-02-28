using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class changeDBAuditResourcesList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditResourcesList_AuditResources_AuditResourcesId",
                table: "AuditResourcesList");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditResourcesList_Quarters_QuarterId",
                table: "AuditResourcesList");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditResourcesList_Users_UserId",
                table: "AuditResourcesList");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditResourcesList_Years_YearId",
                table: "AuditResourcesList");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuditResourcesList",
                table: "AuditResourcesList");

            migrationBuilder.DropIndex(
                name: "IX_AuditResourcesList_QuarterId",
                table: "AuditResourcesList");

            migrationBuilder.DropColumn(
                name: "InsourceUserId",
                table: "AuditResourcesList");

            migrationBuilder.DropColumn(
                name: "ManagerUserId",
                table: "AuditResourcesList");

            migrationBuilder.DropColumn(
                name: "OutsourceUserId",
                table: "AuditResourcesList");

            migrationBuilder.DropColumn(
                name: "QuarterId",
                table: "AuditResourcesList");

            migrationBuilder.RenameTable(
                name: "AuditResourcesList",
                newName: "AuditResourcesLists");

            migrationBuilder.RenameIndex(
                name: "IX_AuditResourcesList_YearId",
                table: "AuditResourcesLists",
                newName: "IX_AuditResourcesLists_YearId");

            migrationBuilder.RenameIndex(
                name: "IX_AuditResourcesList_UserId",
                table: "AuditResourcesLists",
                newName: "IX_AuditResourcesLists_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AuditResourcesList_AuditResourcesId",
                table: "AuditResourcesLists",
                newName: "IX_AuditResourcesLists_AuditResourcesId");

            migrationBuilder.AlterColumn<int>(
                name: "YearId",
                table: "AuditResourcesLists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AuditResourcesLists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceType",
                table: "AuditResourcesLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuditResourcesLists",
                table: "AuditResourcesLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResourcesLists_AuditResources_AuditResourcesId",
                table: "AuditResourcesLists",
                column: "AuditResourcesId",
                principalTable: "AuditResources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResourcesLists_Users_UserId",
                table: "AuditResourcesLists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResourcesLists_Years_YearId",
                table: "AuditResourcesLists",
                column: "YearId",
                principalTable: "Years",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditResourcesLists_AuditResources_AuditResourcesId",
                table: "AuditResourcesLists");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditResourcesLists_Users_UserId",
                table: "AuditResourcesLists");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditResourcesLists_Years_YearId",
                table: "AuditResourcesLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuditResourcesLists",
                table: "AuditResourcesLists");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "AuditResourcesLists");

            migrationBuilder.RenameTable(
                name: "AuditResourcesLists",
                newName: "AuditResourcesList");

            migrationBuilder.RenameIndex(
                name: "IX_AuditResourcesLists_YearId",
                table: "AuditResourcesList",
                newName: "IX_AuditResourcesList_YearId");

            migrationBuilder.RenameIndex(
                name: "IX_AuditResourcesLists_UserId",
                table: "AuditResourcesList",
                newName: "IX_AuditResourcesList_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AuditResourcesLists_AuditResourcesId",
                table: "AuditResourcesList",
                newName: "IX_AuditResourcesList_AuditResourcesId");

            migrationBuilder.AlterColumn<int>(
                name: "YearId",
                table: "AuditResourcesList",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AuditResourcesList",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "InsourceUserId",
                table: "AuditResourcesList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ManagerUserId",
                table: "AuditResourcesList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OutsourceUserId",
                table: "AuditResourcesList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuarterId",
                table: "AuditResourcesList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuditResourcesList",
                table: "AuditResourcesList",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResourcesList_QuarterId",
                table: "AuditResourcesList",
                column: "QuarterId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResourcesList_AuditResources_AuditResourcesId",
                table: "AuditResourcesList",
                column: "AuditResourcesId",
                principalTable: "AuditResources",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResourcesList_Quarters_QuarterId",
                table: "AuditResourcesList",
                column: "QuarterId",
                principalTable: "Quarters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResourcesList_Users_UserId",
                table: "AuditResourcesList",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditResourcesList_Years_YearId",
                table: "AuditResourcesList",
                column: "YearId",
                principalTable: "Years",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
