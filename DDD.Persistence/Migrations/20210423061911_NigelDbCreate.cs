using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Persistence.Migrations
{
    public partial class NigelDbCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Password = table.Column<string>(type: "varchar(50)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Picture = table.Column<string>(type: "varchar(250)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdminUserInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUserInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminUser_AdminUserInfo",
                        column: x => x.UserId,
                        principalTable: "AdminUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdminUserLoginRecord",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Mode = table.Column<string>(type: "varchar(10)", nullable: false),
                    LoginTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUserLoginRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminUser_AdminUserLoginRecord",
                        column: x => x.UserId,
                        principalTable: "AdminUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUserInfo_UserId",
                table: "AdminUserInfo",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminUserLoginRecord_UserId",
                table: "AdminUserLoginRecord",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUserInfo");

            migrationBuilder.DropTable(
                name: "AdminUserLoginRecord");

            migrationBuilder.DropTable(
                name: "AdminUser");
        }
    }
}
