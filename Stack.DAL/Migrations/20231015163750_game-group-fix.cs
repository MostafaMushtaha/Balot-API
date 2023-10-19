using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stack.DAL.Migrations
{
    public partial class gamegroupfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Member_Game_GameID",
                table: "Game_Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Member_Game_GameID2",
                table: "Game_Member");

            migrationBuilder.DropIndex(
                name: "IX_Game_Member_GameID",
                table: "Game_Member");

            migrationBuilder.DropIndex(
                name: "IX_Game_Member_GameID2",
                table: "Game_Member");

            migrationBuilder.RenameColumn(
                name: "GameID2",
                table: "Game_Member",
                newName: "GameID1");

            migrationBuilder.AlterColumn<long>(
                name: "GroupID",
                table: "Game",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_Member_GameID",
                table: "Game_Member",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Game_Member_GameID1",
                table: "Game_Member",
                column: "GameID1",
                unique: true,
                filter: "[GameID1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Member_Game_GameID",
                table: "Game_Member",
                column: "GameID",
                principalTable: "Game",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Member_Game_GameID1",
                table: "Game_Member",
                column: "GameID1",
                principalTable: "Game",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Member_Game_GameID",
                table: "Game_Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Member_Game_GameID1",
                table: "Game_Member");

            migrationBuilder.DropIndex(
                name: "IX_Game_Member_GameID",
                table: "Game_Member");

            migrationBuilder.DropIndex(
                name: "IX_Game_Member_GameID1",
                table: "Game_Member");

            migrationBuilder.RenameColumn(
                name: "GameID1",
                table: "Game_Member",
                newName: "GameID2");

            migrationBuilder.AlterColumn<long>(
                name: "GroupID",
                table: "Game",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Game_Member_GameID",
                table: "Game_Member",
                column: "GameID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_Member_GameID2",
                table: "Game_Member",
                column: "GameID2");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Member_Game_GameID",
                table: "Game_Member",
                column: "GameID",
                principalTable: "Game",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Member_Game_GameID2",
                table: "Game_Member",
                column: "GameID2",
                principalTable: "Game",
                principalColumn: "ID");
        }
    }
}
