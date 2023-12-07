using Microsoft.EntityFrameworkCore.Migrations;

namespace Whistler.Infrastructure.DataAccess.Migrations
{
    public partial class PhoneInviteCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InviteCode",
                table: "phones_msg_chats",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_phones_msg_chats_InviteCode",
                table: "phones_msg_chats",
                column: "InviteCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_phones_msg_chats_InviteCode",
                table: "phones_msg_chats");

            migrationBuilder.DropColumn(
                name: "InviteCode",
                table: "phones_msg_chats");
        }
    }
}
