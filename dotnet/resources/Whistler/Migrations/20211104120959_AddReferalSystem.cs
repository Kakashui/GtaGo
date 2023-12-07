using Microsoft.EntityFrameworkCore.Migrations;
using Whistler.SDK;

namespace Whistler.Migrations
{
    public partial class AddReferalSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MySQL.QuerySync("ALTER TABLE `characters` " +
                "ADD COLUMN `promocode` TEXT NULL," +
                "ADD COLUMN `promocodeUsed` INT(11) NOT NULL DEFAULT 0 AFTER `promocode`," +
                "ADD COLUMN `promocodeActivated` INT(11) NOT NULL DEFAULT 0 AFTER `promocodeUsed`;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            MySQL.QuerySync("ALTER TABLE `characters` " +
                "DROP COLUMN `promocodeUsed`," +
                "DROP COLUMN `promocodeActivated`," +
                "DROP COLUMN `promocode`; ");
        }
    }
}
