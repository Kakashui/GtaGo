using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Whistler.Infrastructure.DataAccess.Migrations
{
    public partial class PhoneNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "phones_news_advert",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SenderUUID = table.Column<int>(nullable: false),
                    EditorUUID = table.Column<int>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    DateCompleate = table.Column<DateTime>(nullable: false),
                    PhoneNumber = table.Column<int>(nullable: false),
                    MessengerLogin = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    PrimeAdvert = table.Column<bool>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phones_news_advert", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "phones_news_advert");
        }
    }
}
