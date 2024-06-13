using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class AddNullFK_CompanyId_Budget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
    name: "CompanyId",
    table: "AuditBudgets",
    nullable: true, // تسمح بالقيم الفارغة (null)
    oldNullable: false, // كانت محددة سابقاً على أنها غير قابلة للاحتواء على قيمة فارغة
    oldClrType: typeof(string),
    oldMaxLength: 50, // يمكن تحديد الحد الأقصى للطول هنا إذا لزم الأمر
    oldDefaultValue: ""); // يمكن تحديد القيمة الافتراضية هنا إذا لزم الأمر

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
