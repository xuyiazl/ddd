using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Persistence.Migrations
{
    public partial class Update20210609 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "AdminUserInfo",
                type: "int",
                nullable: false,
                comment: "数据状态（1、正常 2、不显示 3、已删除）",
                oldClrType: typeof(int),
                oldType: "int(1)",
                oldComment: "数据状态（1、正常 2、不显示 3、已删除）");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "AdminUser",
                type: "int",
                nullable: false,
                comment: "数据状态（1、正常 2、不显示 3、已删除）",
                oldClrType: typeof(int),
                oldType: "int(1)",
                oldComment: "数据状态（1、正常 2、不显示 3、已删除）");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "AdminUserInfo",
                type: "int(1)",
                nullable: false,
                comment: "数据状态（1、正常 2、不显示 3、已删除）",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "数据状态（1、正常 2、不显示 3、已删除）");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "AdminUser",
                type: "int(1)",
                nullable: false,
                comment: "数据状态（1、正常 2、不显示 3、已删除）",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "数据状态（1、正常 2、不显示 3、已删除）");
        }
    }
}
