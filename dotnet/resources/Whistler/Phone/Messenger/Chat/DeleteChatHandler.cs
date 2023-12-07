using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Chat.Updates;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat
{
    internal class DeleteChatHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(DeleteChatHandler));

        [RemoteEvent("phone::msg::deleteChat")]
        public async Task DeleteChat(ExtPlayer player, int chatId)
        {
            try
            {
                var character = player.Character;
                var accountId = character.PhoneTemporary.Phone?.AccountId;

                if (accountId == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var accountToChat = await context.AccountsToChats.FindAsync(accountId, chatId);
                    if (accountToChat.AdminLvl != AdminLvl.Owner)
                    {
                        _logger.WriteError($"Player try to delete chat that he is not owner. ({chatId}, {accountToChat.AdminLvl})");
                        return;
                    }

                    var chat = await context.Chats.FindAsync(chatId);
                    await UpdatR.SendUpdate(chat, new DeleteChatUpdate(chat));

                    context.Chats.Remove(chat);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::deleteChat ({chatId}) - " + e.ToString()); }
        }
    }
}
