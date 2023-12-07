using Microsoft.EntityFrameworkCore.Migrations;
using Whistler.SDK;

namespace Whistler.Migrations
{
    public partial class AddBusinessHandlingShop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MySQL.QuerySync("INSERT INTO `bizsettings` " +
                "(`biztype`, `settings`, `bliptype`, `blipcolor`, `name`, `minimumpercentproduct`) " +
                "VALUES ('40', '[{\"Name\":\"Parts\",\"OrderPrice\":50,\"MaxMinType\":\"%\",\"MaxPrice\":200,\"MinPrice\":100,\"StockCapacity\":150000}]', '740', '43', 'Fine Tuning', '5'); " +
                "ALTER TABLE `businesses` " +
                "CHANGE COLUMN `money` `money` TEXT NULL ; ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            MySQL.QuerySync("DELETE FROM `bizsettings` WHERE `biztype` = 40");
        }
    }
}
