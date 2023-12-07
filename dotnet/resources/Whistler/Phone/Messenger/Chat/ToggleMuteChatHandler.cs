using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.SDK;

namespace Whistler.Phone.Messenger.Chat
{
    internal class ToggleMuteChatHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ToggleMuteChatHandler));

        [RemoteEvent("phone::msg::muteChat")]
        public async Task MuteChat(ExtPlayer player, int chatId, bool toggle)
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
                        _logger.WriteError($"Player try to mute chat, but he is not in the chat now ({chatId}).");
                        return;
                    }

                    accountToChat.IsMuted = toggle;
                    await context.SaveChangesAsync();

                    player.TriggerCefEvent("smartphone/messagePage/msg_setChatMuted",
                        JsonConvert.SerializeObject(new { ChatId = chatId, IsMuted = toggle }));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::muteChat ({chatId}) - " + e.ToString()); }
        }
    }
}
