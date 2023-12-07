using GTANetworkAPI;
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
    internal class CreatePrivateChatHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(CreatePrivateChatHandler));

        [RemoteEvent("phone::msg::createPrivateChat")]
        public async Task CreatePrivateChat(ExtPlayer player, int companionAccountId)
        {
            try
            {
                var character = player.Character;
                var account = character.PhoneTemporary.Phone?.Account;

                if (account == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var companionAccount = await context.Accounts
                        .FindAsync(companionAccountId);
                    
                    if (companionAccount == null)
                    {
                        _logger.WriteError($"Player try to create private chat with non existing companion. ({companionAccountId})");
                        return;
                    }

                    var privateChat = await ChatsCommon.GetPrivateChat(context, account.Id, companionAccountId);
                    if (privateChat != null)
                    {
                        _logger.WriteError($"Player try to create private chat with already existing private chat. ({companionAccountId})");
                        return;
                    }

                    await ChatsCommon.CreatePrivateChat(context, account, companionAccount);
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::createPrivateChat ({companionAccountId}) - " + e.ToString()); }
        }
    }
}
