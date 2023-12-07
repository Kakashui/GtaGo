using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Whistler.Infrastructure.DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "phones_blocks",
                columns: table => new
                {
                    SimCardId = table.Column<int>(nullable: false),
                    TargetNumber = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phones_blocks", x => new { x.SimCardId, x.TargetNumber });
                });

            migrationBuilder.CreateTable(
                name: "phones_msg_chats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Avatar = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phones_msg_chats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "phones_simcards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phones_simcards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "phones_callhistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FromSimCardId = table.Column<int>(nullable: false),
                    TargetNumber = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CallStatus = table.Column<int>(nullable: false),
                    Duration = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phones_callhistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_phones_callhistory_phones_simcards_FromSimCardId",
                        column: x => x.FromSimCardId,
                        principalTable: "phones_simcards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "phones_contacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HolderSimCardId = table.Column<int>(nullable: false),
                    TargetNumber = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phones_contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_phones_contacts_phones_simcards_HolderSimCardId",
                        column: x => x.HolderSimCardId,
                        principalTable: "phones_simcards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "phones_msg_accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    SimCardId = table.Column<int>(nullable: false),
                    DisplayedName = table.Column<string>(nullable: true),
                    IsNumberHided = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastVisit = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phones_msg_accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_phones_msg_accounts_phones_simcards_SimCardId",
                        column: x => x.SimCardId,
                        principalTable: "phones_simcards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Phones",
                columns: table => new
                {
                    CharacterUuid = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InstalledAppsIds = table.Column<string>(nullable: true),
                    SimCardId = table.Column<int>(nullable: true),
                    AccountId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phones", x => x.CharacterUuid);
                    table.ForeignKey(
                        name: "FK_Phones_phones_msg_accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "phones_msg_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Phones_phones_simcards_SimCardId",
                        column: x => x.SimCardId,
                        principalTable: "phones_simcards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "phones_msg_contacts",
                columns: table => new
                {
                    ContactId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HolderAccountId = table.Column<int>(nullable: false),
                    TargetAccountId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phones_msg_contacts", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_phones_msg_contacts_phones_msg_accounts_HolderAccountId",
                        column: x => x.HolderAccountId,
                        principalTable: "phones_msg_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_phones_msg_contacts_phones_msg_accounts_TargetAccountId",
                        column: x => x.TargetAccountId,
                        principalTable: "phones_msg_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "phones_msg_messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    SenderId = table.Column<int>(nullable: false),
                    ChatId = table.Column<int>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    Attachments = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Photo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phones_msg_messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_phones_msg_messages_phones_msg_chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "phones_msg_chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_phones_msg_messages_phones_msg_accounts_SenderId",
                        column: x => x.SenderId,
                        principalTable: "phones_msg_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "phones_msg_accountsToChat",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false),
                    ChatId = table.Column<int>(nullable: false),
                    IsLeaved = table.Column<bool>(nullable: false),
                    IsMuted = table.Column<bool>(nullable: false),
                    LastReadMessageId = table.Column<int>(nullable: true),
                    AdminLvl = table.Column<int>(nullable: false),
                    IsBlocked = table.Column<bool>(nullable: false),
                    Permissions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phones_msg_accountsToChat", x => new { x.AccountId, x.ChatId });
                    table.ForeignKey(
                        name: "FK_phones_msg_accountsToChat_phones_msg_accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "phones_msg_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_phones_msg_accountsToChat_phones_msg_chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "phones_msg_chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_phones_msg_accountsToChat_phones_msg_messages_LastReadMessag~",
                        column: x => x.LastReadMessageId,
                        principalTable: "phones_msg_messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Phones_AccountId",
                table: "Phones",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Phones_SimCardId",
                table: "Phones",
                column: "SimCardId");

            migrationBuilder.CreateIndex(
                name: "IX_phones_callhistory_FromSimCardId",
                table: "phones_callhistory",
                column: "FromSimCardId");

            migrationBuilder.CreateIndex(
                name: "IX_phones_callhistory_TargetNumber",
                table: "phones_callhistory",
                column: "TargetNumber");

            migrationBuilder.CreateIndex(
                name: "IX_phones_contacts_HolderSimCardId",
                table: "phones_contacts",
                column: "HolderSimCardId");

            migrationBuilder.CreateIndex(
                name: "IX_phones_contacts_TargetNumber",
                table: "phones_contacts",
                column: "TargetNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_phones_msg_accounts_SimCardId",
                table: "phones_msg_accounts",
                column: "SimCardId");

            migrationBuilder.CreateIndex(
                name: "IX_phones_msg_accountsToChat_ChatId",
                table: "phones_msg_accountsToChat",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_phones_msg_accountsToChat_LastReadMessageId",
                table: "phones_msg_accountsToChat",
                column: "LastReadMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_phones_msg_contacts_HolderAccountId",
                table: "phones_msg_contacts",
                column: "HolderAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_phones_msg_contacts_TargetAccountId",
                table: "phones_msg_contacts",
                column: "TargetAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_phones_msg_messages_ChatId",
                table: "phones_msg_messages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_phones_msg_messages_SenderId",
                table: "phones_msg_messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_phones_simcards_Number",
                table: "phones_simcards",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Phones");

            migrationBuilder.DropTable(
                name: "phones_blocks");

            migrationBuilder.DropTable(
                name: "phones_callhistory");

            migrationBuilder.DropTable(
                name: "phones_contacts");

            migrationBuilder.DropTable(
                name: "phones_msg_accountsToChat");

            migrationBuilder.DropTable(
                name: "phones_msg_contacts");

            migrationBuilder.DropTable(
                name: "phones_msg_messages");

            migrationBuilder.DropTable(
                name: "phones_msg_chats");

            migrationBuilder.DropTable(
                name: "phones_msg_accounts");

            migrationBuilder.DropTable(
                name: "phones_simcards");
        }
    }
}
