using Microsoft.EntityFrameworkCore.Migrations;

namespace Whistler.Infrastructure.DataAccess.Migrations
{
    public partial class AddPhoneBankAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BankNumber",
                table: "phones_simcards",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankNumber",
                table: "phones_simcards");
        }
    }
}
