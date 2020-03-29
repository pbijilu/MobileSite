using Microsoft.EntityFrameworkCore.Migrations;

namespace MobileSite.Data.Migrations
{
    public partial class addGoodId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GoodId",
                table: "Goods",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoodId",
                table: "Goods");
        }
    }
}
