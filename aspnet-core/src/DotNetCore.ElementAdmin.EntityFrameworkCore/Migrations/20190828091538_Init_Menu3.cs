using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetCore.ElementAdmin.Migrations
{
    public partial class Init_Menu3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Menus",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Menus",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "Menus",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_UserId1",
                table: "Menus",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_AbpUsers_UserId1",
                table: "Menus",
                column: "UserId1",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AbpUsers_UserId1",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_UserId1",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Menus");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Menus",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
