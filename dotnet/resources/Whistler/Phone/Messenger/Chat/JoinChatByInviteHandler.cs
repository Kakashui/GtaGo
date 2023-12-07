using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
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

namespace Whistler.Phone.Messenger.Chat
{
    internal class JoinChatByInviteHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(JoinChatByInviteHandler));

        [RemoteEvent("phone::msg::joinChatByInvite")]
        public async Task HandleJoinChatByInvite(ExtPlayer player, string inviteCode)
        {
            try
            {
                using (var context = DbManager.TemporaryContext)
                {
                    var chat = await context.Chats.FirstOrDefaultAsync(c => c.InviteCode == inviteCode);
                    if (chat == null)
                    {
                        Notify.SendError(player, "phone:msg:j:1");
                        return;
                    }

                    var playerAccountId = player.Character.PhoneTemporary.Phone.Account.Id;
                    var accountToChat = await context.AccountsToChats.FindAsync(playerAccountId, chat.Id);
                    if (accountToChat != null && accountToChat.IsBlocked)
                    {
                        Notify.SendError(player, "phone:msg:j:2");
                        return;
                    }

                    if (accountToChat != null && !accountToChat.IsLeaved)
                    {
                        Notify.SendError(player, "phone:msg:j:3");
                        return;
                    }

                    if (accountToChat != null)
                    {
                        accountToChat.IsLeaved = false;
                    }
                    else
                    {
                        chat.AccountToChats.Add(new AccountToChat
                        {
                            AccountId = playerAccountId,
                            AdminLvl = AdminLvl.None,
                            IsMuted = false,
                            IsBlocked = false
                        });
                    }

                    await context.SaveChangesAsync();

                    var account = player.Character.PhoneTemporary.Phone.Account;
                    await UpdatR.SendUpdate(account, new AccountJoinChatUpdate(account, chat));
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError($"Unhandled exception catched on phone::msg::joinChatByInvite ({inviteCode}) - " + e.ToString());
                });
            }
        }
    }
}
