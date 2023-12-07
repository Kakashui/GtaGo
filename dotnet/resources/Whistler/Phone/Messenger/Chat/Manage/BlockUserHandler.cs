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
    internal class BlockUserHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(BlockUserHandler));

        [RemoteEvent("phone::msg::manage::blockUser")]
        public async Task HandleBlockUserInChatAsync(ExtPlayer player, int chatId, int userId)
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

                    if (subscriberToChat.AdminLvl == AdminLvl.Owner)
                    {
                        Notify.SendError(player, "access:err");
                        return;
                    }

                    if (subscriberToChat.AdminLvl == AdminLvl.Administrator 
                        && !accountToChat.Permissions.Contains(Permission.PurposingAdmins))
                    {
                        Notify.SendError(player, "access:err");
                        return;
                    }

                    subscriberToChat.AdminLvl = AdminLvl.None;
                    subscriberToChat.Permissions = new List<Permission>();
                    subscriberToChat.IsBlocked = true;
                    subscriberToChat.IsLeaved = true;

                    subscriberToChat.BlockedById = account.Id;

                    await context.SaveChangesAsync();
                    await UpdatR.SendUpdate(subscriberToChat.Account, new AccountBlockInChatUpdate(subscriberToChat.Account, chatId, true));

                    var chat = await context.Chats.FindAsync(chatId);

                    await UpdatR.SendUpdate(chat, new UserBlockedUpdate(chat, userId, subscriberToChat.Account.DisplayedName, account.DisplayedName));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::manage::blockUser ({chatId}, {userId}) - " + e.ToString()); }
        }
    }
}
