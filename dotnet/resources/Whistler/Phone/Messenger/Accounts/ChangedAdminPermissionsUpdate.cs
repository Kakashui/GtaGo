using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Accounts
{
    internal class ChangedAdminPermissionsUpdate : IUpdate<Account>
    {
        public Account UpdateTarget { get; }
        public int ChatId { get; }
        public AccountToChat AccountToChat { get; }

        public ChangedAdminPermissionsUpdate(Account updateTarget, int chatId, AccountToChat accountToChat)
        {
            UpdateTarget = updateTarget;
            ChatId = chatId;
            AccountToChat = accountToChat;
        }
    }

    internal class ChangedAdminPermissionsHandler : IUpdateHandler<ChangedAdminPermissionsUpdate, Account>
    {
        public async Task Handle(Player subscriber, ChangedAdminPermissionsUpdate update)
        {
            if (!(subscriber is ExtPlayer player)) return;

            player.TriggerCefAction("smartphone/messagePage/msg_setMyAdminPermissionsForChat",
                JsonConvert.SerializeObject(new { ChatId = update.ChatId, AdminLvl = update.AccountToChat.AdminLvl, Permissions = update.AccountToChat.Permissions }));
        }
    }
}
