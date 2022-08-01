using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investment.API.Migrations
{
    public partial class changeDataType_passwordSalt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "varbinary(128)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(64)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "varbinary(64)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(128)");
        }
    }
}
