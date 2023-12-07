using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Domain.Phone;
using Whistler.Domain.Phone.Contacts;
using Whistler.Domain.Phone.Messenger;
using Whistler.Infrastructure.DataAccess;

namespace Whistler.Phone
{
    internal class TestDataSeed : Script
    {
        private bool IsTest = false;
        public TestDataSeed()
        {
            if (!IsTest)
                return;

            using (var context = DbManager.TemporaryContext)
            {
                Account account1 = InsertPlayerData(context, 1818666666, "MiLT", "matvey", 651695);
                Account account2 = InsertPlayerData(context, 1818555555, "Nemilt", "ne matvey", 184877);

                var chat = new Chat
                {
                    Name = "P",
                    Type = Domain.Phone.Messenger.ChatType.Private,
                    Description = "P",
                    Avatar = null
                };

                var accountToChat1 = new AccountToChat
                {
                    Account = account1,
                    Chat = chat,
                    IsLeaved = false,
                    IsMuted = false,
                    AdminLvl = 0,
                    IsBlocked = false
                };

                var accountToChat2 = new AccountToChat
                {
                    Account = account2,
                    Chat = chat,
                    IsLeaved = false,
                    IsMuted = false,
                    AdminLvl = 0,
                    IsBlocked = false
                };

                var msg1 = new Message
                {
                    Text = "Ayo, niggas",
                    CreatedAt = DateTime.Now.AddMinutes(-5),
                    Sender = account1,
                    Chat = chat
                };

                var msg2 = new Message
                {
                    Text = "by b*chs",
                    CreatedAt = DateTime.Now.AddMinutes(-1),
                    Sender = account1,
                    Chat = chat
                };

                context.Chats.Add(chat);
                context.AccountsToChats.Add(accountToChat1);
                context.AccountsToChats.Add(accountToChat2);

                context.Messages.Add(msg1);
                context.Messages.Add(msg2);

                context.SaveChanges();
            }
        }

        private static Account InsertPlayerData(ServerContext context, int simnum, string username, string displayedname, int characteruuid)
        {
            var simcard = new SimCard
            {
                Contacts = new List<Contact>(),
                Number = simnum
            };

            var account = new Account
            {
                Username = username,
                SimCard = simcard,
                DisplayedName = displayedname,
                IsNumberHided = false,
                LastVisit = DateTime.Now
            };

            var phone = new Domain.Phone.Phone()
            {
                CharacterUuid = characteruuid,
                SimCard = simcard,
                Account = account,
                InstalledAppsIds = new List<AppId>()
            };

            context.SimCards.Add(simcard);
            context.Accounts.Add(account);
            context.Phones.Add(phone);

            context.SaveChanges();

            return account;
        }
    }
}
