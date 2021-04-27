using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Persistence.Migrations
{
    public partial class AddBaseMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "AdminUser");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_At",
                table: "AdminUserInfo",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "添加日期");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted_At",
                table: "AdminUserInfo",
                type: "datetime",
                nullable: true,
                comment: "删除日期");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AdminUserInfo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "数据状态（1、正常 2、不显示 3、已删除）");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated_At",
                table: "AdminUserInfo",
                type: "datetime",
                nullable: true,
                comment: "最后修改日期");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "AdminUser",
                type: "int",
                nullable: false,
                comment: "数据状态（1、正常 2、不显示 3、已删除）",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_At",
                table: "AdminUser",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "添加日期");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted_At",
                table: "AdminUser",
                type: "datetime",
                nullable: true,
                comment: "删除日期");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated_At",
                table: "AdminUser",
                type: "datetime",
                nullable: true,
                comment: "最后修改日期");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created_At",
                table: "AdminUserInfo");

            migrationBuilder.DropColumn(
                name: "Deleted_At",
                table: "AdminUserInfo");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AdminUserInfo");

            migrationBuilder.DropColumn(
                name: "Updated_At",
                table: "AdminUserInfo");

            migrationBuilder.DropColumn(
                name: "Created_At",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "Deleted_At",
                table: "AdminUser");

            migrationBuilder.DropColumn(
                name: "Updated_At",
                table: "AdminUser");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "AdminUser",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "数据状态（1、正常 2、不显示 3、已删除）");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "AdminUser",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
