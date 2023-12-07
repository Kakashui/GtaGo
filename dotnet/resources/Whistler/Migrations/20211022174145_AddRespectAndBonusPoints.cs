using Microsoft.EntityFrameworkCore.Migrations;
using Whistler.SDK;

namespace Whistler.Migrations
{
    public partial class AddRespectAndBonusPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MySQL.QuerySync("ALTER TABLE `characters` " +
                "ADD COLUMN `respectPoints` INT NOT NULL DEFAULT 0 AFTER `banknew`," +
                "ADD COLUMN `bonusPoints` INT NOT NULL DEFAULT 0 AFTER `respectPoints`;" +
                "ALTER TABLE `families` " +
                "ADD COLUMN `f_respectPoints` INT NOT NULL DEFAULT 0 AFTER `f_points`;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            MySQL.QuerySync("ALTER TABLE `characters` " +
                "DROP COLUMN `respectPoints`, " +
                "DROP COLUMN `bonusPoints`; " +
                "ALTER TABLE `families` " +
                "DROP COLUMN `f_respectPoints`; ");
            
        }
    }
}
