using Microsoft.EntityFrameworkCore.Migrations;
using Whistler.SDK;

namespace Whistler.Migrations
{
    public partial class ClearStartQuest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MySQL.QuerySync("UPDATE `characters` SET `queststage` = 9 WHERE 1=1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
