using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Accounts;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat.Manage
{
    internal class AddAdminHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(AddAdminHandler));

        [RemoteEvent("phone::msg::manage::addAdmin")]
        public async Task HandleRemoveAdmin(ExtPlayer player, int chatId, int subscriberId)
        {
            try
            {
                var character = player.Character;
                var account = character.PhoneTemporary.Phone?.Account;

                if (account == null)
                    return;

                if (account.Id == subscriberId)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var accountToChat = await context.AccountsToChats.FindAsync(account.Id, chatId);
                    if (accountToChat == null || !accountToChat.Permissions.Contains(Permission.PurposingAdmins))
                        return;

                    var subscriberToChat = await context.AccountsToChats
                        .Include(a => a.Account)
                        .FirstOrDefaultAsync(ac => ac.AccountId == subscriberId && ac.ChatId == chatId);

                    if (subscriberToChat == null)
                        return;

                    subscriberToChat.Permissions = new List<Permission>();
                    subscriberToChat.AdminLvl = AdminLvl.Administrator;

                    await context.SaveChangesAsync();

                    player.TriggerCefAction("smartphone/messagePage/msg_addAdminForChat",
                        JsonConvert.SerializeObject(new { ChatId = chatId, AccountId = subscriberId }));

                    await UpdatR.SendUpdate(subscriberToChat.Account, new ChangedAdminPermissionsUpdate(subscriberToChat.Account, chatId, subscriberToChat));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::manage::addAdmin ({chatId}, {subscriberId}) - " + e.ToString()); }
        }
    }
}
