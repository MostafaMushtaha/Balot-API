using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stack.DAL.Migrations
{
    public partial class Addedgamestatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Game",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Game");
        }
    }
}
