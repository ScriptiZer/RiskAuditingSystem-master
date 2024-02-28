using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class addActualStartEndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "AuditResourcesListStartEndDates",
                newName: "PlanStartDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "AuditResourcesListStartEndDates",
                newName: "PlanEndDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualEndDate",
                table: "AuditResourcesListStartEndDates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualStartDate",
                table: "AuditResourcesListStartEndDates",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualEndDate",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.DropColumn(
                name: "ActualStartDate",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.RenameColumn(
                name: "PlanStartDate",
                table: "AuditResourcesListStartEndDates",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "PlanEndDate",
                table: "AuditResourcesListStartEndDates",
                newName: "EndDate");
        }
    }
}
