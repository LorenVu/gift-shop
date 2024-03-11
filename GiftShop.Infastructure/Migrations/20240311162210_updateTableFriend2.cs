using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiftShop.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateTableFriend2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Users_FriendID",
                table: "Friend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friend",
                table: "Friend");

            migrationBuilder.RenameTable(
                name: "Friend",
                newName: "Friends");

            migrationBuilder.RenameIndex(
                name: "IX_Friend_FriendID",
                table: "Friends",
                newName: "IX_Friends_FriendID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friends",
                table: "Friends",
                column: "FriendshipID");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_FriendID",
                table: "Friends",
                column: "FriendID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_FriendID",
                table: "Friends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friends",
                table: "Friends");

            migrationBuilder.RenameTable(
                name: "Friends",
                newName: "Friend");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_FriendID",
                table: "Friend",
                newName: "IX_Friend_FriendID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friend",
                table: "Friend",
                column: "FriendshipID");

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Users_FriendID",
                table: "Friend",
                column: "FriendID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
