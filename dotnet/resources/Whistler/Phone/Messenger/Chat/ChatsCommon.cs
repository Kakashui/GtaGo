using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Domain.Phone.Messenger;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Accounts;
using Whistler.Phone.Messenger.Chat.Updates;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat
{
    internal class ChatsCommon
    {
        public const string InviteUrlBase = "messenger.go/join?code=";
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ChatsCommon));

        public static async Task<Domain.Phone.Messenger.Chat> CreatePrivateChat(ServerContext context, Account account, Account companionAccount)
        {
            try
            {
                var privateChat = new Domain.Phone.Messenger.Chat
                {
                    Type = Domain.Phone.Messenger.ChatType.Private
                };

                privateChat.AccountToChats.Add(CreateAccountToChat(account.Id));
                privateChat.AccountToChats.Add(CreateAccountToChat(companionAccount.Id));
                context.Chats.Add(privateChat);

                await context.SaveChangesAsync();

                await UpdatR.SendUpdate(account, new AccountJoinPrivateChatUpdate(account, privateChat, companionAccount));
                await UpdatR.SendUpdate(companionAccount, new AccountJoinPrivateChatUpdate(companionAccount, privateChat, account));

                return privateChat;
            }
            catch (Exception e)
            {
                _logger.WriteError($"Unhandled exception catched on {nameof(CreatePrivateChat)} - " + e.ToString());
                return null;
            }
        }

        public static async Task<Domain.Phone.Messenger.Chat> GetPrivateChat(ServerContext context, int accountId, int companionAccountId)
        {
            return await context.Chats
                .Where(c => c.Type == Domain.Phone.Messenger.ChatType.Private)
                .Include(c => c.AccountToChats)
                .Where(c => c.AccountToChats.Any(ac => ac.AccountId == accountId) &&
                    c.AccountToChats.Any(ac => ac.AccountId == companionAccountId))
                .FirstOrDefaultAsync();
        }

        public static async Task InviteContactsToChat(int inviterId, string contactsToInviteJson, Domain.Phone.Messenger.Chat chat)
        {
            if (chat == null) return;

            var accountsIdsToInvite = JsonConvert.DeserializeObject<int[]>(contactsToInviteJson);
            var messageTxt = InviteUrlBase + chat.InviteCode;
            foreach (var contactAccountId in accountsIdsToInvite)
            {
                await SendPrivateMessage(inviterId, contactAccountId, messageTxt);
            }
        }

        public static async Task SendPrivateMessage(int senderId, int receiverId, string messageText)
        {
            using (var context = DbManager.TemporaryContext)
            {
                var chat = await GetPrivateChat(context, senderId, receiverId);
                if (chat == null)
                {
                    var senderAccount = await context.Accounts.FindAsync(senderId);
                    if (senderAccount == null)
                    {
                        throw new ArgumentException($"There is no account with ID = {senderId}.");
                    }

                    var receiverAccount = await context.Accounts.FindAsync(receiverId);
                    if (receiverAccount == null)
                    {
                        throw new ArgumentException($"There is no account with ID = {receiverId}.");
                    }

                    chat = await CreatePrivateChat(context, senderAccount, receiverAccount);
                }

                var isSenderBlockedByReceiver = chat.AccountToChats.First(a => a.AccountId == receiverId)
                    .IsBlocked;
                if (isSenderBlockedByReceiver)
                {
                    return;
                }

                var message = new Message
                {
                    Text = messageText,
                    SenderId = senderId,
                    ChatId = chat.Id,
                    Attachments = null
                };

                context.Messages.Add(message);
                await context.SaveChangesAsync();

                await UpdatR.SendUpdate(chat, new NewMessageUpdate(chat, message));
            }
        }

        private static AccountToChat CreateAccountToChat(int accId)
        {
            return new AccountToChat
            {
                AccountId = accId,
                AdminLvl = AdminLvl.None,
                IsMuted = false,
                IsBlocked = false
            };
        }
    }
}
