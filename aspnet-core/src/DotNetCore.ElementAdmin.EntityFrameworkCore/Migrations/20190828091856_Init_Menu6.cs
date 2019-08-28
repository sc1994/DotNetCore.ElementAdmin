using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetCore.ElementAdmin.Migrations
{
    public partial class Init_Menu6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AbpUsers_UserId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_UserId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Menus");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_RoleId",
                table: "Menus",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_AbpRoles_RoleId",
                table: "Menus",
                column: "RoleId",
                principalTable: "AbpRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_AbpRoles_RoleId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_RoleId",
                table: "Menus");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Menus",
                nullable: true);

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
    }
}
