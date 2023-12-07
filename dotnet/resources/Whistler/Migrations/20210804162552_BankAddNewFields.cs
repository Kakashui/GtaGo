using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Whistler.Migrations
{
    public partial class BankAddNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Create",
                table: "efcore_bank_credit",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<float>(
                name: "InterestRate",
                table: "efcore_bank_credit",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Create",
                table: "efcore_bank_credit");

            migrationBuilder.DropColumn(
                name: "InterestRate",
                table: "efcore_bank_credit");
        }
    }
}
