using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddAuditPrograms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditPrograms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    AuditorId = table.Column<int>(type: "int", nullable: false),
                    ActualDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Period = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_AuditPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditPrograms_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditPrograms_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditPrograms_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuditProgramLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditProgramId = table.Column<int>(type: "int", nullable: false),
                    MajorProcess = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubProcess = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskCategoryId = table.Column<int>(type: "int", nullable: false),
                    RiskDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControlDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_AuditProgramLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditProgramLists_AuditPrograms_AuditProgramId",
                        column: x => x.AuditProgramId,
                        principalTable: "AuditPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditProgramLists_RiskCategories_RiskCategoryId",
                        column: x => x.RiskCategoryId,
                        principalTable: "RiskCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditProgramLists_AuditProgramId",
                table: "AuditProgramLists",
                column: "AuditProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditProgramLists_RiskCategoryId",
                table: "AuditProgramLists",
                column: "RiskCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPrograms_CompanyId",
                table: "AuditPrograms",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPrograms_DepartmentId",
                table: "AuditPrograms",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPrograms_UserId",
                table: "AuditPrograms",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditProgramLists");

            migrationBuilder.DropTable(
                name: "AuditPrograms");
        }
    }
}
