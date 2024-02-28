using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class addColumnsToCalendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BalancefromPreviousYear",
                table: "Calendars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DaysNumberInYear",
                table: "Calendars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstimatedSickLeaves",
                table: "Calendars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HolidaysNumber",
                table: "Calendars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NoofInternationalHlidays",
                table: "Calendars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NoofLeavesDays",
                table: "Calendars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpecialWorkingHours",
                table: "Calendars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Weekends",
                table: "Calendars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkingDays",
                table: "Calendars",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalancefromPreviousYear",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "DaysNumberInYear",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "EstimatedSickLeaves",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "HolidaysNumber",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "NoofInternationalHlidays",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "NoofLeavesDays",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "SpecialWorkingHours",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "Weekends",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "WorkingDays",
                table: "Calendars");
        }
    }
}
