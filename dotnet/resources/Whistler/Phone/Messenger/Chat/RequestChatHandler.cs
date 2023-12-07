using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Dtos;
using Whistler.SDK;

namespace Whistler.Phone.Messenger.Chat
{
    internal class RequestChatHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(RequestChatHandler));

        [RemoteEvent("phone::msg::requestChat")]
        public async Task HandleRequestChat(ExtPlayer player, int chatId)
        {
            try
            {
                var character = player.Character;
                var accountId = character.PhoneTemporary.Phone?.AccountId;

                if (accountId == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var chat = await context.Chats.FindAsync(chatId);

                    if (chat == null)
                    {
                        _logger.WriteError($"Player try to load message of non existing chat. ({chatId})");
                        return;
                    }

                    IEnumerable<Message> messages = (chat.Type == Domain.Phone.Messenger.ChatType.Channel) ?
                        await GetChannelPosts(context, chatId) : await GetChatMessages(context, chatId);

                    var mapper = MapperManager.Get();

                    var subscribersCount = await context.AccountsToChats
                        .Where(a => a.ChatId == chatId)
                        .Where(a => !a.IsLeaved && !a.IsBlocked)
                        .CountAsync();

                    var accountToChat = await context.AccountsToChats
                        .FindAsync(accountId, chatId);

                    var messagesDtos = mapper.Map<IEnumerable<MessageDto>>(messages);
                    player.TriggerCefAction("smartphone/messagePage/msg_loadInchatInfo",
                        JsonConvert.SerializeObject(new { ChatId = chatId, SubscribersCount = subscribersCount, 
                            Messages = messagesDtos, accountToChat.Permissions, 
                            accountToChat.AdminLvl, chat.InviteCode }));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::requestChat ({chatId}) - " + e.ToString()); }
        }

        public async Task<IEnumerable<Message>> GetChatMessages(ServerContext context, int chatId)
        {
            return await context.Messages
                .Where(m => m.ChatId == chatId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(50)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetChannelPosts(ServerContext context, int chatId)
        {
            return await context.Posts
                .Where(m => m.ChatId == chatId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(25)
                .ToListAsync();
        }

        private class AccountComparer : IEqualityComparer<Account>
        {
            public bool Equals(Account x, Account y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(Account account)
            {
                return account.GetHashCode();
            }
        }
    }
}
