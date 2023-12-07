using Microsoft.EntityFrameworkCore.Migrations;
using Whistler.SDK;

namespace Whistler.Migrations
{
    public partial class CreateAchievesAndContracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MySQL.QuerySync("CREATE TABLE `achievements` (" +
                "  `uuid` INT NOT NULL," +
                "  `achieveName` INT NOT NULL," +
                "  `currentLevel` INT NOT NULL DEFAULT 0," +
                "  `givenReward` TINYINT NOT NULL DEFAULT 0," +
                "  `dateCompleted` DATETIME NOT NULL," +
                "  PRIMARY KEY(`uuid`, `achieveName`));");


            MySQL.QuerySync("CREATE TABLE `contracts` (" +
                "  `ownerid` INT NOT NULL," +
                "  `ownerType` INT NOT NULL," +
                "  `contractName` INT NOT NULL," +
                "  `countCompleted` INT NOT NULL DEFAULT 0," +
                "  `currentLevel` INT NOT NULL DEFAULT 0," +
                "  `inProgress` TINYINT NOT NULL DEFAULT 0," +
                "  `expirationDate` DATETIME NOT NULL," +
                "  `countPlayersProgress` TEXT NULL," +
                "  PRIMARY KEY(`ownerid`, `ownerType`, `contractName`));");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            MySQL.Query("DROP TABLE `achievements`;");
            MySQL.Query("DROP TABLE `contracts`;");
        }
    }
}
