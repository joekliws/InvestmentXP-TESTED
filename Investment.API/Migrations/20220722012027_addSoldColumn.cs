using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investment.API.Migrations
{
    public partial class addSoldColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BoughtAt",
                table: "UserAssets",
                newName: "UtcBoughtAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "UtcSoldAt",
                table: "UserAssets",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UtcSoldAt",
                table: "UserAssets");

            migrationBuilder.RenameColumn(
                name: "UtcBoughtAt",
                table: "UserAssets",
                newName: "BoughtAt");
        }
    }
}
