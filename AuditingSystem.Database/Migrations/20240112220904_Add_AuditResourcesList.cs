using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class Add_AuditResourcesList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditResourcesList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditResourceId = table.Column<int>(type: "int", nullable: false),
                    AuditResourcesId = table.Column<int>(type: "int", nullable: true),
                    YearId = table.Column<int>(type: "int", nullable: false),
                    QuarterId = table.Column<int>(type: "int", nullable: false),
                    InsourceUserId = table.Column<int>(type: "int", nullable: false),
                    OutsourceUserId = table.Column<int>(type: "int", nullable: false),
                    ManagerUserId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Plan = table.Column<int>(type: "int", nullable: false),
                    Actual = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditResourcesList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditResourcesList_AuditResources_AuditResourcesId",
                        column: x => x.AuditResourcesId,
                        principalTable: "AuditResources",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuditResourcesList_Quarters_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "Quarters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditResourcesList_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuditResourcesList_Years_YearId",
                        column: x => x.YearId,
                        principalTable: "Years",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditResourcesList_AuditResourcesId",
                table: "AuditResourcesList",
                column: "AuditResourcesId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResourcesList_QuarterId",
                table: "AuditResourcesList",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResourcesList_UserId",
                table: "AuditResourcesList",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResourcesList_YearId",
                table: "AuditResourcesList",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditResourcesList");
        }
    }
}
