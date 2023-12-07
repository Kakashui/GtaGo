using Microsoft.EntityFrameworkCore.Migrations;

namespace Whistler.Migrations
{
    public partial class BankAddCreditPaymentHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ClosedCredit",
                table: "efcore_bank_credit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "HistoryPayment",
                table: "efcore_bank_credit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HistoryPayment",
                table: "efcore_bank_credit");

            migrationBuilder.AlterColumn<int>(
                name: "ClosedCredit",
                table: "efcore_bank_credit",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool));
        }
    }
}
