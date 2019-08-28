using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetCore.ElementAdmin.Migrations
{
    public partial class Init_Menu7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Menus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Menus",
                nullable: true);
        }
    }
}
