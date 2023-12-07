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

namespace Whistler.Phone.Messenger.Chat.Manage
{
    internal class InviteToChatHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(InviteToChatHandler));

        [RemoteEvent("phone::msg::manage::inviteToChat")]
        public async Task HandleInviteToChat(ExtPlayer player, int chatId, string idsToInviteJson)
        {
            try
            {
                var character = player.Character;
                var account = character.PhoneTemporary.Phone?.Account;

                if (account == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var chat = await context.Chats.FindAsync(chatId);
                    await ChatsCommon.InviteContactsToChat(account.Id, idsToInviteJson, chat);
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::manage::inviteToChat ({idsToInviteJson}) - " + e.ToString()); }
        }
    }
}
