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
    internal class UpdateChatHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(UpdateChatHandler));

        [RemoteEvent("phone::msg::updateChat")]
        public async Task UpdateChatAsync(ExtPlayer player, int chatId, string name, string description, string avatar)
        {
            try
            {
                var character = player.Character;
                var account = character.PhoneTemporary.Phone.Account;

                if (account == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var accountToChat = await context.AccountsToChats.FindAsync(account.Id, chatId);
                    var isHavePermission = accountToChat.Permissions.Contains(Permission.ChangingGroupProfile);

                    if (!isHavePermission)
                    {
                        _logger.WriteError($"Player try to create post without permissions. ({chatId})");
                        return;
                    }

                    var chat = await context.Chats.FindAsync(chatId);

                    chat.Name = name;
                    chat.Description = description;
                    chat.Avatar = avatar;

                    await context.SaveChangesAsync();
                    await UpdatR.SendUpdate(chat, new ChatUpdate(chat));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::createChannel ({name}, {description}, {avatar}) - " + e.ToString()); }
        }
    }
}
