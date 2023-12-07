using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Chat.Updates;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat
{
    internal class MarkMessageAsReadHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(MarkMessageAsReadHandler));

        [RemoteEvent("phone::msg::markAsRead")]
        public async Task HandleMarkMessageAsRead(ExtPlayer player, int chatId)
        {
            try
            {
                using (var context = DbManager.TemporaryContext)
                {
                    var account = player.Character.PhoneTemporary.Phone.Account;

                    var chat = await context.Chats.FindAsync(chatId);
                    if (chat == null)
                        return;

                    var accountToChat = await context.AccountsToChats.FindAsync(account.Id, chatId);
                    var nonReadMessages = await context.Messages
                        .Where(m => m.ChatId == chatId)
                        .Where(m => m.SenderId != account.Id)
                        .Where(m => !m.IsRead)
                        .ToListAsync();

                    if (nonReadMessages.Count == 0)
                        return;

                    var lastMessage = nonReadMessages
                        .OrderBy(m => m.CreatedAt)
                        .First();

                    accountToChat.LastReadMessageId = lastMessage.Id;

                    nonReadMessages.ForEach(m => m.IsRead = true);
                    await context.SaveChangesAsync();

                    player.TriggerCefEvent("smartphone/messagePage/msg_markChatMessagesAsRead", chatId);
                    await UpdatR.SendUpdate(chat, new ReadMessagesUpdate(chat, account.Id));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::markAsRead ({chatId}) - " + e.ToString()); }
        }
    }
}
