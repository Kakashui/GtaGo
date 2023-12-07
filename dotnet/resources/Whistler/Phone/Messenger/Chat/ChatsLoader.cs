using AutoMapper;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whistler.Core.Character;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Chat.Updates;
using Whistler.Phone.Messenger.Dtos;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat
{
    internal class ChatsLoader : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ChatsLoader));

        public ChatsLoader()
        {
            PhoneLoader.PhoneReadyAsync += LoadChats;
        }

        private async Task LoadChats(ExtPlayer player, Character character)
        {
            try
            {
                var accountId = character.PhoneTemporary.Phone?.AccountId;

                if (accountId == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var accountChats = await context.AccountsToChats
                        .AsNoTracking()
                        .Where(ac => ac.AccountId == accountId)
                        .Where(ac => !ac.IsLeaved && !ac.IsBlocked)
                        .Include(ac => ac.LastReadMessage)
                        .Include(ac => ac.Chat)
                        .ToListAsync();

                    var chatsIds = accountChats.Select(a => a.ChatId).ToList();
                    var lastReadedMessagesByChatId = accountChats.ToDictionary(a => a.ChatId, a => a.LastReadMessage);

                    var privateChatsName = await GetPrivateChatsNames(context,
                        accountChats.Select(ac => ac.Chat)
                            .Where(c => c.Type == Domain.Phone.Messenger.ChatType.Private)
                            .ToList(), (int) accountId
                    );

                    var accountChatsMessages = await context.Messages
                        .AsNoTracking()
                        .Where(m => chatsIds.Contains(m.ChatId))
                        .ToListAsync();

                    var chatMessagesInfo = accountChatsMessages
                        .GroupBy(m => m.ChatId)
                        .Select(g => new
                        {
                            NonReadMessagesCount = g
                                .Where(m => m.SenderId != accountId)
                                .Where(m => lastReadedMessagesByChatId[g.Key] == null ||
                                            m.CreatedAt > lastReadedMessagesByChatId[m.ChatId]?.CreatedAt)
                                .Count(),
                            LastMessage = g.OrderByDescending(m => m.CreatedAt).FirstOrDefault()
                        })
                        .ToList();

                    var mapper = MapperManager.Get();
                    var dtos = accountChats.Select(ac =>
                    {
                        var chatMessageInfo = chatMessagesInfo.Find(i => i.LastMessage?.ChatId == ac.ChatId);
                        return GetChatDto(ac.Chat, ac.IsMuted, chatMessageInfo?.NonReadMessagesCount ?? 0, chatMessageInfo?.LastMessage, mapper, privateChatsName);
                    });

                    player.TriggerCefEventWithLargeList(100, "smartphone/messagePage/msg_loadChats", dtos);

                    foreach (var chat in accountChats.Select(ac => ac.Chat))
                    {
                        UpdatR.Subscribe(player, chat);
                    }
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError($"Unhandled exception catched on LoadChats ({player?.Name}) - " + e.ToString());
                });
            }
        }

        private async Task<Dictionary<int, (string name, int accountId, Account Account)>> GetPrivateChatsNames(ServerContext context, List<Domain.Phone.Messenger.Chat> personalChats, int requesterAccountId)
        {
            var contactNameByAccount = await context.MsgContacts
                .AsNoTracking()
                .Where(c => c.HolderAccountId == requesterAccountId)
                .Select(c => new { c.TargetAccountId, c.Name })
                .ToDictionaryAsync(c => c.TargetAccountId, c => c.Name);

            var chatsIds = personalChats.Select(p => p.Id);

            var displayedNames = await context.AccountsToChats
                .Where(ac => ac.AccountId != requesterAccountId)
                .Where(ac => chatsIds.Contains(ac.ChatId))
                .Include(ac => ac.Account)
                .Select(ac => new { ac.ChatId, ac.Account.DisplayedName, ac.AccountId, ac.Account })
                .ToListAsync();

            return displayedNames.ToDictionary(d => d.ChatId,
                d => contactNameByAccount.ContainsKey(d.AccountId) ? (contactNameByAccount[d.AccountId], d.AccountId, d.Account) : (d.DisplayedName, d.AccountId, d.Account));
        }

        private ChatDto GetChatDto(Domain.Phone.Messenger.Chat chat,
            bool isMuted,
            int nonReadMessages,
            Message lastMessage,
            Mapper mapper,
            Dictionary<int, (string name, int accountId, Account Account)> personalChatsInfo)
        {
            MessageDto lastMessageDto = null;

            if (lastMessage != null)
            {
                lastMessageDto = mapper.Map<MessageDto>(lastMessage);
            }

            string chatName = chat.Name;
            int accountId = -1;
            bool isOnline = true;
            if (chat.Type == Domain.Phone.Messenger.ChatType.Private)
            {
                chatName = personalChatsInfo[chat.Id].name;
                accountId = personalChatsInfo[chat.Id].accountId;
                var account = personalChatsInfo[chat.Id].Account;
                isOnline = Main.GetExtPlayerByPredicate(item => item.Character.PhoneTemporary?.Phone?.Account?.Id == accountId) != null;
                UpdatR.SendUpdate(account, new UserOnlineUpdate(account, chat, true));
            }

            return new ChatDto
            {
                Id = chat.Id,
                Description = chat.Description,
                Name = chatName,
                Type = chat.Type,
                NonReadMessagesCount = nonReadMessages,
                LastMessage = lastMessageDto,
                CreatedTime = chat.CreatedAt.GetTotalSeconds(),
                AccountId = accountId,
                IconColors = chat.Avatar,
                IsMuted = isMuted,
                IsOnline = isOnline,
            };
        }
    }
}
