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
using Whistler.Phone.Messenger.Chat.Updates;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat.Manage
{
    internal class UnblockUserHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(UnblockUserHandler));

        [RemoteEvent("phone::msg::manage::unblockUser")]
        public async Task HandleUnblockUser(ExtPlayer player, int chatId, int userId)
        {
            try
            {
                var character = player.Character;
                var account = character.PhoneTemporary.Phone?.Account;

                if (account == null)
                    return;

                if (account.Id == userId)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var accountToChat = await context.AccountsToChats.FindAsync(account.Id, chatId);
                    if (accountToChat == null || !accountToChat.Permissions.Contains(Permission.BlockingAccounts))
                    {
                        Notify.SendError(player, "access:err");
                        return;
                    }

                    var subscriberToChat = await context.AccountsToChats
                        .Include(a => a.Account)
                        .FirstOrDefaultAsync(ac => ac.AccountId == userId && ac.ChatId == chatId);

                    if (subscriberToChat == null)
                        return;

                    subscriberToChat.IsBlocked = false;
                    subscriberToChat.BlockedById = null;

                    await context.SaveChangesAsync();

                    var chat = await context.Chats.FindAsync(chatId);
                    await UpdatR.SendUpdate(chat, new UserUnblockedUpdate(chat, userId));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::manage::unblockUser ({chatId}, {userId}) - " + e.ToString()); }
        }
    }
}
