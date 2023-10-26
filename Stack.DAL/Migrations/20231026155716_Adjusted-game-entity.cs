using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stack.DAL.Migrations
{
    public partial class Adjustedgameentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Groups_GroupID",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Member_Game_GameID",
                table: "Game_Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Member_Group_Member_GroupMemberID",
                table: "Game_Member");

            migrationBuilder.DropForeignKey(
                name: "FK_GameRound_Game_GameID",
                table: "GameRound");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameRound",
                table: "GameRound");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Game_Member",
                table: "Game_Member");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Game",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Game");

            migrationBuilder.RenameTable(
                name: "GameRound",
                newName: "GameRounds");

            migrationBuilder.RenameTable(
                name: "Game_Member",
                newName: "Game_Members");

            migrationBuilder.RenameTable(
                name: "Game",
                newName: "Games");

            migrationBuilder.RenameIndex(
                name: "IX_GameRound_GameID",
                table: "GameRounds",
                newName: "IX_GameRounds_GameID");

            migrationBuilder.RenameIndex(
                name: "IX_Game_Member_GroupMemberID",
                table: "Game_Members",
                newName: "IX_Game_Members_GroupMemberID");

            migrationBuilder.RenameIndex(
                name: "IX_Game_Member_GameID",
                table: "Game_Members",
                newName: "IX_Game_Members_GameID");

            migrationBuilder.RenameIndex(
                name: "IX_Game_GroupID",
                table: "Games",
                newName: "IX_Games_GroupID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameRounds",
                table: "GameRounds",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Game_Members",
                table: "Game_Members",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Members_Games_GameID",
                table: "Game_Members",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Members_Group_Member_GroupMemberID",
                table: "Game_Members",
                column: "GroupMemberID",
                principalTable: "Group_Member",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameRounds_Games_GameID",
                table: "GameRounds",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Groups_GroupID",
                table: "Games",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Members_Games_GameID",
                table: "Game_Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Members_Group_Member_GroupMemberID",
                table: "Game_Members");

            migrationBuilder.DropForeignKey(
                name: "FK_GameRounds_Games_GameID",
                table: "GameRounds");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Groups_GroupID",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameRounds",
                table: "GameRounds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Game_Members",
                table: "Game_Members");

            migrationBuilder.RenameTable(
                name: "Games",
                newName: "Game");

            migrationBuilder.RenameTable(
                name: "GameRounds",
                newName: "GameRound");

            migrationBuilder.RenameTable(
                name: "Game_Members",
                newName: "Game_Member");

            migrationBuilder.RenameIndex(
                name: "IX_Games_GroupID",
                table: "Game",
                newName: "IX_Game_GroupID");

            migrationBuilder.RenameIndex(
                name: "IX_GameRounds_GameID",
                table: "GameRound",
                newName: "IX_GameRound_GameID");

            migrationBuilder.RenameIndex(
                name: "IX_Game_Members_GroupMemberID",
                table: "Game_Member",
                newName: "IX_Game_Member_GroupMemberID");

            migrationBuilder.RenameIndex(
                name: "IX_Game_Members_GameID",
                table: "Game_Member",
                newName: "IX_Game_Member_GameID");

            migrationBuilder.AddColumn<long>(
                name: "Total",
                table: "Game",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Game",
                table: "Game",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameRound",
                table: "GameRound",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Game_Member",
                table: "Game_Member",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Groups_GroupID",
                table: "Game",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Member_Game_GameID",
                table: "Game_Member",
                column: "GameID",
                principalTable: "Game",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Member_Group_Member_GroupMemberID",
                table: "Game_Member",
                column: "GroupMemberID",
                principalTable: "Group_Member",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameRound_Game_GameID",
                table: "GameRound",
                column: "GameID",
                principalTable: "Game",
                principalColumn: "ID");
        }
    }
}
