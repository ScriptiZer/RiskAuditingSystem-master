﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class RetrievePlanEndDate_AuditResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PlanEndDate",
                table: "AuditResourcesListStartEndDates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlanStartDate",
                table: "AuditResourcesListStartEndDates",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanEndDate",
                table: "AuditResourcesListStartEndDates");

            migrationBuilder.DropColumn(
                name: "PlanStartDate",
                table: "AuditResourcesListStartEndDates");
        }
    }
}
