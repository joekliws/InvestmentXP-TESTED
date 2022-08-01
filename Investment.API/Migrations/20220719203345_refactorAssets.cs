using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Investment.API.Migrations
{
    public partial class refactorAssets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Accounts_AccountId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_AccountId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Assets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Assets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AccountId",
                table: "Assets",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Accounts_AccountId",
                table: "Assets",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId");
        }
    }
}
