using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Whistler.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "efcore_bank_account",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UUID = table.Column<int>(nullable: false),
                    BankId = table.Column<int>(nullable: false),
                    Number = table.Column<long>(nullable: false),
                    OwnerType = table.Column<int>(nullable: false),
                    Balance = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_efcore_bank_account", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "efcore_bank_credit",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UUID = table.Column<int>(nullable: false),
                    BankId = table.Column<int>(nullable: false),
                    TypePayment = table.Column<int>(nullable: false),
                    Indebtedness = table.Column<int>(nullable: false),
                    LeftPayments = table.Column<int>(nullable: false),
                    PledgeId = table.Column<int>(nullable: false),
                    PledgeType = table.Column<int>(nullable: false),
                    PayedAmount = table.Column<int>(nullable: false),
                    Percents = table.Column<int>(nullable: false),
                    ClosedCredit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_efcore_bank_credit", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "efcore_bank_deposit",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UUID = table.Column<int>(nullable: false),
                    BankId = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    Profit = table.Column<int>(nullable: false),
                    DepositMoney = table.Column<int>(nullable: false),
                    AddedMoney = table.Column<int>(nullable: false),
                    DepositTypes = table.Column<int>(nullable: false),
                    HoursInInterval = table.Column<int>(nullable: false),
                    DepositFullTime = table.Column<int>(nullable: false),
                    ClosedDeposit = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_efcore_bank_deposit", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "efcore_bank_transact",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    From = table.Column<int>(nullable: false),
                    FromType = table.Column<int>(nullable: false),
                    To = table.Column<int>(nullable: false),
                    ToType = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    Tax = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_efcore_bank_transact", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "efcore_bank_account");

            migrationBuilder.DropTable(
                name: "efcore_bank_credit");

            migrationBuilder.DropTable(
                name: "efcore_bank_deposit");

            migrationBuilder.DropTable(
                name: "efcore_bank_transact");
        }
    }
}
