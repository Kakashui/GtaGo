using Microsoft.EntityFrameworkCore.Migrations;

namespace Whistler.Infrastructure.DataAccess.Migrations
{
    public partial class PhoneAddBlockeByAtAccountsToChatsw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlockedById",
                table: "phones_msg_accountsToChat",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_phones_msg_accountsToChat_BlockedById",
                table: "phones_msg_accountsToChat",
                column: "BlockedById");

            migrationBuilder.AddForeignKey(
                name: "FK_phones_msg_accountsToChat_phones_msg_accounts_BlockedById",
                table: "phones_msg_accountsToChat",
                column: "BlockedById",
                principalTable: "phones_msg_accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_phones_msg_accountsToChat_phones_msg_accounts_BlockedById",
                table: "phones_msg_accountsToChat");

            migrationBuilder.DropIndex(
                name: "IX_phones_msg_accountsToChat_BlockedById",
                table: "phones_msg_accountsToChat");

            migrationBuilder.DropColumn(
                name: "BlockedById",
                table: "phones_msg_accountsToChat");
        }
    }
}
