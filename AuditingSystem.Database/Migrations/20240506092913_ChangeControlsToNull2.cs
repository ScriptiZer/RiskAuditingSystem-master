using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuditingSystem.Database.Migrations
{
    public partial class ChangeControlsToNull2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContactNo",
                table: "Users",
                nullable: true, 
                oldNullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldDefaultValue: ""); 

            migrationBuilder.AlterColumn<string>(
name: "Title",
table: "Users",
nullable: true, // تسمح بالقيم الفارغة (null)
oldNullable: false, // كانت محددة سابقاً على أنها غير قابلة للاحتواء على قيمة فارغة
oldClrType: typeof(string),
oldMaxLength: 50, // يمكن تحديد الحد الأقصى للطول هنا إذا لزم الأمر
oldDefaultValue: ""); // يمكن تحديد القيمة الافتراضية هنا إذا لزم الأمر

            migrationBuilder.AlterColumn<string>(
name: "Email",
table: "Users",
nullable: true, // تسمح بالقيم الفارغة (null)
oldNullable: false, // كانت محددة سابقاً على أنها غير قابلة للاحتواء على قيمة فارغة
oldClrType: typeof(string),
oldMaxLength: 50, // يمكن تحديد الحد الأقصى للطول هنا إذا لزم الأمر
oldDefaultValue: ""); // يمكن تحديد القيمة الافتراضية هنا إذا لزم الأمر

            migrationBuilder.AlterColumn<string>(
name: "Password",
table: "Users",
nullable: true, // تسمح بالقيم الفارغة (null)
oldNullable: false, // كانت محددة سابقاً على أنها غير قابلة للاحتواء على قيمة فارغة
oldClrType: typeof(string),
oldMaxLength: 50, // يمكن تحديد الحد الأقصى للطول هنا إذا لزم الأمر
oldDefaultValue: ""); // يمكن تحديد القيمة الافتراضية هنا إذا لزم الأمر

            migrationBuilder.AlterColumn<string>(
name: "ConfirmPassword",
table: "Users",
nullable: true, // تسمح بالقيم الفارغة (null)
oldNullable: false, // كانت محددة سابقاً على أنها غير قابلة للاحتواء على قيمة فارغة
oldClrType: typeof(string),
oldMaxLength: 50, // يمكن تحديد الحد الأقصى للطول هنا إذا لزم الأمر
oldDefaultValue: ""); // يمكن تحديد القيمة الافتراضية هنا إذا لزم الأمر

            migrationBuilder.AlterColumn<string>(
name: "RoleId",
table: "Users",
nullable: true, // تسمح بالقيم الفارغة (null)
oldNullable: false, // كانت محددة سابقاً على أنها غير قابلة للاحتواء على قيمة فارغة
oldClrType: typeof(string),
oldMaxLength: 50, // يمكن تحديد الحد الأقصى للطول هنا إذا لزم الأمر
oldDefaultValue: ""); // يمكن تحديد القيمة الافتراضية هنا إذا لزم الأمر

            migrationBuilder.AlterColumn<string>(
name: "CompanyId",
table: "Users",
nullable: true, // تسمح بالقيم الفارغة (null)
oldNullable: false, // كانت محددة سابقاً على أنها غير قابلة للاحتواء على قيمة فارغة
oldClrType: typeof(string),
oldMaxLength: 50, // يمكن تحديد الحد الأقصى للطول هنا إذا لزم الأمر
oldDefaultValue: ""); // يمكن تحديد القيمة الافتراضية هنا إذا لزم الأمر

            migrationBuilder.AlterColumn<string>(
name: "DepartmentId",
table: "Users",
nullable: true, // تسمح بالقيم الفارغة (null)
oldNullable: false, // كانت محددة سابقاً على أنها غير قابلة للاحتواء على قيمة فارغة
oldClrType: typeof(string),
oldMaxLength: 50, // يمكن تحديد الحد الأقصى للطول هنا إذا لزم الأمر
oldDefaultValue: ""); // يمكن تحديد القيمة الافتراضية هنا إذا لزم الأمر

            migrationBuilder.AlterColumn<string>(
name: "DepartmentId",
table: "Users",
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
