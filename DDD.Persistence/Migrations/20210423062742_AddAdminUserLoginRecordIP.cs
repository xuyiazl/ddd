using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Persistence.Migrations
{
    public partial class AddAdminUserLoginRecordIP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ip",
                table: "AdminUserLoginRecord",
                type: "varchar(20)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ip",
                table: "AdminUserLoginRecord");
        }
    }
}
