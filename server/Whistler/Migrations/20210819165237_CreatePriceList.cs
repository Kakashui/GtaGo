using Microsoft.EntityFrameworkCore.Migrations;
using Whistler.SDK;
using Whistler.VehicleSystem.Models.Configs;

namespace Whistler.Migrations
{
    public partial class CreatePriceList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MySQL.QuerySync("CREATE TABLE `propsprices` ( " +
                "`id` INT NOT NULL AUTO_INCREMENT, " +
                "`name` TEXT NOT NULL, " +
                "`price` INT NOT NULL DEFAULT 0, " +
                "PRIMARY KEY(`id`)); ");
            string query = "";
            foreach (var item in VehicleConfigs.VehicleConfigList)
            {
                query += $"INSERT INTO `propsprices`(`name`, `price`) VALUES('Car::{item.Value.ModelName}', {item.Value.PriceInCoins});\n ";
            }
            MySQL.QuerySync(query);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            MySQL.Query("DROP TABLE `propsprices`;");
        }
    }
}
