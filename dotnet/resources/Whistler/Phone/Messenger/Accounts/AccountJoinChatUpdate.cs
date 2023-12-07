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
using Whistler.Phone.Messenger.Chat.Updates;
using Whistler.Phone.Messenger.Dtos;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Accounts
{
    internal class AccountJoinChatUpdate : IUpdate<Account>
    {
        public Account UpdateTarget { get; }
        public Domain.Phone.Messenger.Chat Chat { get; }

        public AccountJoinChatUpdate(Account updateTarget, Domain.Phone.Messenger.Chat chat)
        {
            UpdateTarget = updateTarget;
            Chat = chat;
        }
    }

    internal class AccountJoinChatHandler : IUpdateHandler<AccountJoinChatUpdate, Account>
    {
        public async Task Handle(Player subscriber, AccountJoinChatUpdate update)
        {
            if (!(subscriber is ExtPlayer player)) return;

            var lastMessage = await GetChatLastMessage(update.Chat.Id, update.Chat.Type);

            var chatDto = new ChatDto()
            {
                Id = update.Chat.Id,
                Description = update.Chat.Description,
                Name = update.Chat.Name,
                Type = update.Chat.Type,
                NonReadMessagesCount = 0,
                LastMessage = lastMessage,
                CreatedTime = update.Chat.CreatedAt.GetTotalSeconds(),
                IconColors = update.Chat.Avatar,
                IsMuted = false,
                IsOnline = true,
            };

            player.TriggerCefAction("smartphone/messagePage/msg_loadChat", JsonConvert.SerializeObject(chatDto));
            UpdatR.Subscribe(subscriber, update.Chat);

            await UpdatR.SendUpdate(update.Chat, new NewSubscriberUpdate(update.Chat));
        }

        private async Task<MessageDto> GetChatLastMessage(int chatId, Domain.Phone.Messenger.ChatType chatType)
        {
            using (var context = DbManager.TemporaryContext)
            {
                var mapper = MapperManager.Get();

                var query = (chatType == Domain.Phone.Messenger.ChatType.Channel)
                    ? context.Posts.AsNoTracking() : context.Messages.AsNoTracking();

                var lastMessage = await query
                    .Where(p => p.ChatId == chatId)
                    .OrderByDescending(p => p.CreatedAt)
                    .FirstOrDefaultAsync();

                return mapper.Map<MessageDto>(lastMessage);
            }
        }
    }

    internal class AccountJoinPrivateChatUpdate : IUpdate<Account>
    {
        public Account UpdateTarget { get; }
        public Domain.Phone.Messenger.Chat Chat { get; }
        public Account Companion { get; }

        public AccountJoinPrivateChatUpdate(Account updateTarget, Domain.Phone.Messenger.Chat chat, Account companion)
        {
            UpdateTarget = updateTarget;
            Chat = chat;
            Companion = companion;
        }
    }

    internal class AccountJoinPrivateChatHandler : IUpdateHandler<AccountJoinPrivateChatUpdate, Account>
    {
        public async Task Handle(Player subscriber, AccountJoinPrivateChatUpdate update)
        {
            if (!(subscriber is ExtPlayer player)) return;

            var isOnline = Main.GetExtPlayerByPredicate(item => item.Character.PhoneTemporary?.Phone.Account?.Id == update.Companion.Id) != null;
            var chatDto = new ChatDto()
            {
                Id = update.Chat.Id,
                Description = update.Chat.Description,
                Name = update.Companion.DisplayedName,
                Type = update.Chat.Type,
                NonReadMessagesCount = 0,
                LastMessage = null,
                CreatedTime = update.Chat.CreatedAt.GetTotalSeconds(),
                AccountId = update.Companion.Id,
                IsMuted = false,
                IsOnline = isOnline,
            };

            player.TriggerCefAction("smartphone/messagePage/msg_loadChat", JsonConvert.SerializeObject(chatDto));
            UpdatR.Subscribe(subscriber, update.Chat);
        }
    }
}
