using Microsoft.EntityFrameworkCore.Migrations;
using Whistler.SDK;

namespace Whistler.Migrations
{
    public partial class AddReferalSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MySQL.QuerySync("ALTER TABLE `characters` " +
                "ADD COLUMN `mypromocode` TEXT NULL," +
                "ADD COLUMN `countUseMyPromocode` INT(11) NOT NULL DEFAULT 0 AFTER `mypromocode`; ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            MySQL.QuerySync("ALTER TABLE `characters` " +
                "DROP COLUMN `countUseMyPromocode`," +
                "DROP COLUMN `mypromocode`; ");
        }
    }
}
