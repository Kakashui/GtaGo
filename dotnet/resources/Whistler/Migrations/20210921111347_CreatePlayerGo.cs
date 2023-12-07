using Microsoft.EntityFrameworkCore.Migrations;
using Whistler.SDK;

namespace Whistler.Migrations
{
    public partial class CreateExtPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MySQL.QuerySync("ALTER TABLE `whistler`.`accounts` " +
                "ADD COLUMN `socialclubid` BIGINT(20) UNSIGNED NOT NULL DEFAULT 0 AFTER `usedbonuses`;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            MySQL.QuerySync("ALTER TABLE `whistler`.`accounts` " +
                "DROP COLUMN `socialclubid`;");
        }
    }
}
