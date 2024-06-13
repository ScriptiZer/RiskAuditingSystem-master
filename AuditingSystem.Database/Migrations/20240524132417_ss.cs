using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class ss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
               name: "UpdatedDate",
               table: "AuditResourcesListStartEndDates",
               type: "datetime2(7)",
               nullable: true);
            migrationBuilder.AddColumn<int>(
              name: "CreatedByCompany",
              table: "AuditResourcesListStartEndDates",
              type: "int",
              nullable: true); 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
