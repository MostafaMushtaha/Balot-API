using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stack.DAL.Migrations
{
    public partial class adjustedreferencenumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Group_GroupID",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Group_Member_Group_GroupID",
                table: "Group_Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_Group_GroupID",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Stats_AspNetUsers_UserID",
                table: "Stats");

            migrationBuilder.DropIndex(
                name: "IX_Stats_UserID",
                table: "Stats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Group",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Stats");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "Group");

            migrationBuilder.RenameTable(
                name: "Group",
                newName: "Groups");

            migrationBuilder.AddColumn<long>(
                name: "GroupMemberID",
                table: "Stats",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groups",
                table: "Groups",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Stats_GroupMemberID",
                table: "Stats",
                column: "GroupMemberID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Groups_GroupID",
                table: "Game",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Member_Groups_GroupID",
                table: "Group_Member",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Groups_GroupID",
                table: "Media",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stats_Group_Member_GroupMemberID",
                table: "Stats",
                column: "GroupMemberID",
                principalTable: "Group_Member",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Groups_GroupID",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Group_Member_Groups_GroupID",
                table: "Group_Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_Groups_GroupID",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Stats_Group_Member_GroupMemberID",
                table: "Stats");

            migrationBuilder.DropIndex(
                name: "IX_Stats_GroupMemberID",
                table: "Stats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groups",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupMemberID",
                table: "Stats");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Groups",
                newName: "Group");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Stats",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "Group",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Group",
                table: "Group",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Stats_UserID",
                table: "Stats",
                column: "UserID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Group_GroupID",
                table: "Game",
                column: "GroupID",
                principalTable: "Group",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Member_Group_GroupID",
                table: "Group_Member",
                column: "GroupID",
                principalTable: "Group",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Group_GroupID",
                table: "Media",
                column: "GroupID",
                principalTable: "Group",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stats_AspNetUsers_UserID",
                table: "Stats",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
