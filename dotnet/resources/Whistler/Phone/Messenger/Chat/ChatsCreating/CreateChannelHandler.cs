using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Accounts;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat.ChatsCreating
{
    internal class CreateChannelHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(CreateChannelHandler));

        [RemoteEvent("phone::msg::createChannel")]
        public async Task CreateChannel(ExtPlayer player, string name, string description, string avatar)
        {
            try
            {
                var character = player.Character;
                var account = character.PhoneTemporary.Phone?.Account;

                if (account == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var channel = new Domain.Phone.Messenger.Chat
                    {
                        Name = name,
                        Type = Domain.Phone.Messenger.ChatType.Channel,
                        Description = description,
                        Avatar = avatar,
                        InviteCode = await new InvitesGenerator().Generate()
                    };

                    var accountToChat = new AccountToChat
                    {
                        AccountId = account.Id,
                        Chat = channel,
                        IsLeaved = false,
                        IsMuted = false,
                        AdminLvl = AdminLvl.Owner,
                        Permissions = new List<Permission> { Permission.ChangingGroupProfile, Permission.CreatingPosts, Permission.DeletingMessages, Permission.BlockingAccounts, Permission.PurposingAdmins },
                        IsBlocked = false
                    };

                    context.Chats.Add(channel);
                    context.AccountsToChats.Add(accountToChat);
                    await context.SaveChangesAsync();

                    await UpdatR.SendUpdate(account, new AccountJoinChatUpdate(account, channel));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::createChannel ({name}, {description}, {avatar}) - " + e.ToString()); }
        }
    }
}
