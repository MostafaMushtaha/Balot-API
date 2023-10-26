using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stack.DAL.Migrations
{
    public partial class Adjustedstats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerLevel",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WinningStreak",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<long>(
                name: "PlayerLevel",
                table: "UserStats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "WinningStreak",
                table: "UserStats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "GroupMemberLevel",
                table: "Stats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "WinningStreak",
                table: "Stats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerLevel",
                table: "UserStats");

            migrationBuilder.DropColumn(
                name: "WinningStreak",
                table: "UserStats");

            migrationBuilder.DropColumn(
                name: "GroupMemberLevel",
                table: "Stats");

            migrationBuilder.DropColumn(
                name: "WinningStreak",
                table: "Stats");

            migrationBuilder.AddColumn<long>(
                name: "PlayerLevel",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "WinningStreak",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
