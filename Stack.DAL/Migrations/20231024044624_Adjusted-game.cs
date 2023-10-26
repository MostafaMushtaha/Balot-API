using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stack.DAL.Migrations
{
    public partial class Adjustedgame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Member_Game_GameID1",
                table: "Game_Member");

            migrationBuilder.DropIndex(
                name: "IX_Game_Member_GameID1",
                table: "Game_Member");

            migrationBuilder.DropColumn(
                name: "GameID1",
                table: "Game_Member");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GameID1",
                table: "Game_Member",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_Member_GameID1",
                table: "Game_Member",
                column: "GameID1",
                unique: true,
                filter: "[GameID1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Member_Game_GameID1",
                table: "Game_Member",
                column: "GameID1",
                principalTable: "Game",
                principalColumn: "ID");
        }
    }
}
