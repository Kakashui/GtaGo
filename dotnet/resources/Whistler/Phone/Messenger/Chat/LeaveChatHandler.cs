using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat
{
    internal class LeaveChatHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(LeaveChatHandler));

        [RemoteEvent("phone::msg::leaveChat")]
        public async Task LeaveChatAsync(ExtPlayer player, int chatId)
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
                    if (accountToChat == null)
                    {
                        _logger.WriteError($"Player try to leave from chat he is not in. ({chatId})");
                        return;
                    }

                    var chat = await context.Chats.FindAsync(chatId);

                    UpdatR.Unsubscribe(player, chat);
                    player.TriggerCefAction("smartphone/messagePage/msg_deleteChat", chatId);

                    accountToChat.IsLeaved = true;
                    await context.SaveChangesAsync();


                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::leaveChat ({chatId}) - " + e.ToString()); }
        }
    }
}
