using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class RemoveBudgetOperation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditBudgetOperationList");

            migrationBuilder.DropTable(
                name: "AuditBudgetOperation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditBudgetOperation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationBudgetTypeId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    BudgetType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceId = table.Column<int>(type: "int", nullable: true),
                    TotalActual = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalEstmated = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Variance = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditBudgetOperation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditBudgetOperation_OperationBudgetTypes_OperationBudgetTypeId",
                        column: x => x.OperationBudgetTypeId,
                        principalTable: "OperationBudgetTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuditBudgetOperation_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuditBudgetOperationList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditBudgetOperationId = table.Column<int>(type: "int", nullable: true),
                    Actual = table.Column<double>(type: "float", nullable: true),
                    BudgetOperationId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estimated = table.Column<double>(type: "float", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quarter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditBudgetOperationList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditBudgetOperationList_AuditBudgetOperation_AuditBudgetOperationId",
                        column: x => x.AuditBudgetOperationId,
                        principalTable: "AuditBudgetOperation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditBudgetOperation_OperationBudgetTypeId",
                table: "AuditBudgetOperation",
                column: "OperationBudgetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditBudgetOperation_UserId",
                table: "AuditBudgetOperation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditBudgetOperationList_AuditBudgetOperationId",
                table: "AuditBudgetOperationList",
                column: "AuditBudgetOperationId");
        }
    }
}
