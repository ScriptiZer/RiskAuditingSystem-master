using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddBaseToResourceList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "AuditResourcesLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AuditResourcesLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "AuditResourcesLists",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AuditResourcesLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AuditResourcesLists",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AuditResourcesLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AuditResourcesLists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AuditResourcesLists",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "AuditResourcesLists");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AuditResourcesLists");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "AuditResourcesLists");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AuditResourcesLists");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AuditResourcesLists");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AuditResourcesLists");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AuditResourcesLists");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AuditResourcesLists");
        }
    }
}
