using Microsoft.EntityFrameworkCore.Migrations;

namespace Whistler.Infrastructure.DataAccess.Migrations
{
    public partial class PhoneRemoveUniqueOnTargetNumberIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_phones_contacts_TargetNumber",
                table: "phones_contacts");

            migrationBuilder.CreateIndex(
                name: "IX_phones_contacts_TargetNumber",
                table: "phones_contacts",
                column: "TargetNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_phones_contacts_TargetNumber",
                table: "phones_contacts");

            migrationBuilder.CreateIndex(
                name: "IX_phones_contacts_TargetNumber",
                table: "phones_contacts",
                column: "TargetNumber",
                unique: true);
        }
    }
}
