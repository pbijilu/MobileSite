using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileSite.Data.Migrations
{
    public partial class idmixxcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdMixx",
                table: "Items",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdMixx",
                table: "Items");
        }
    }
}
