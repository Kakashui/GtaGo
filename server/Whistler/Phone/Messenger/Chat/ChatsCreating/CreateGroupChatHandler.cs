using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Domain.Phone.Messenger;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Accounts;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat.ChatsCreating
{
    internal class CreateGroupChatHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(CreateGroupChatHandler));

        [RemoteEvent("phone::msg::createGroupChat")]
        public async Task HandleCreateGroupChat(Player player, string name, string description, string avatar, string contactsToInviteJson)
        {
            try
            {
                var character = player.GetCharacter();
                var account = character.PhoneTemporary.Phone?.Account;

                if (account == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var chat = await CreateGroupChat(name, description, avatar, account, context);
                    await ChatsCommon.InviteContactsToChat(account.Id, contactsToInviteJson, chat);
                }
            }
            catch (Exception e)
            {
                WhistlerTask.Run(() =>
                    _logger.WriteError($"Unhandled exception catched on phone::msg::createGroupChat ({player?.Name}, {name}, {description}, {avatar}) - " + e));
            }
        }

        private static async Task<Domain.Phone.Messenger.Chat> CreateGroupChat(string name, string description, string avatar, Account account, ServerContext context)
        {
            var chat = new Domain.Phone.Messenger.Chat
            {
                Name = name,
                Type = Domain.Phone.Messenger.ChatType.Group,
                Description = description,
                Avatar = avatar,
                InviteCode = await new InvitesGenerator().Generate()
            };

            var accountToChat = new AccountToChat
            {
                AccountId = account.Id,
                Chat = chat,
                IsLeaved = false,
                IsMuted = false,
                AdminLvl = AdminLvl.Owner,
                Permissions = new List<Permission> { Permission.ChangingGroupProfile, Permission.DeletingMessages, Permission.BlockingAccounts, Permission.PurposingAdmins },
                IsBlocked = false
            };

            context.Chats.Add(chat);
            context.AccountsToChats.Add(accountToChat);
            await context.SaveChangesAsync();

            await UpdatR.SendUpdate(account, new AccountJoinChatUpdate(account, chat));

            return chat;
        }
    }
}
