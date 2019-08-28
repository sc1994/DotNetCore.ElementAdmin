using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetCore.ElementAdmin.Migrations
{
    public partial class Init_Menu4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AbpUsers_UserId1",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_UserId1",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Menus");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Menus",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Menus",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_UserId",
                table: "Menus",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_AbpUsers_UserId",
                table: "Menus",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AbpUsers_UserId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_UserId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Menus");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Menus",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

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
    }
}
