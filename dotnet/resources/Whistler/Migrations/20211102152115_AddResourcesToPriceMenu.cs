using Microsoft.EntityFrameworkCore.Migrations;
using Whistler.Jobs.SteelMaking;
using Whistler.SDK;

namespace Whistler.Migrations
{
    public partial class AddResourcesToPriceMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string query = "";
            foreach (var item in OreMiningSettings.ResourcePrices)
            {
                query += $"INSERT INTO `propsprices`(`name`, `price`) VALUES('Resources::{item.Key.ToString()}', {item.Value / Main.ServerConfig.DonateConfig.CoinToVirtual});\n ";
            }
            MySQL.QuerySync(query);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            MySQL.Query("DELETE FROM `propsprices` WHERE `name` like 'Resources::%'");
        }
    }
}
