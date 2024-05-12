using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLiveChat.Migrations
{
    /// <inheritdoc />
    public partial class _3rdmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMessage_Users_UserId",
                table: "UserMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMessage",
                table: "UserMessage");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserMessage");

            migrationBuilder.RenameTable(
                name: "UserMessage",
                newName: "UserMessages");

            migrationBuilder.RenameIndex(
                name: "IX_UserMessage_UserId",
                table: "UserMessages",
                newName: "IX_UserMessages_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMessages",
                table: "UserMessages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessages_Users_UserId",
                table: "UserMessages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMessages_Users_UserId",
                table: "UserMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMessages",
                table: "UserMessages");

            migrationBuilder.RenameTable(
                name: "UserMessages",
                newName: "UserMessage");

            migrationBuilder.RenameIndex(
                name: "IX_UserMessages_UserId",
                table: "UserMessage",
                newName: "IX_UserMessage_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserMessage",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMessage",
                table: "UserMessage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMessage_Users_UserId",
                table: "UserMessage",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
