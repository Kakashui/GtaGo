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
using Whistler.Phone.Messenger.Chat.Updates;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat.Channels
{
    internal class DeletePostHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(DeletePostHandler));

        [RemoteEvent("phone::msg::deletePost")]
        public async Task DeletePost(ExtPlayer player, int postId)
        {
            try
            {
                var character = player.Character;
                var account = character.PhoneTemporary.Phone.Account;

                if (account == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var post = await context.Posts
                        .Include(p => p.Chat)
                        .FirstOrDefaultAsync(p => p.Id == postId);

                    if (post == null)
                    {
                        _logger.WriteError($"Player try to delete non-existing post. ({postId})");
                        return;
                    }

                    var accountToChat = await context.AccountsToChats.FindAsync(account.Id, post.ChatId);
                    var isHavePermission = accountToChat.Permissions.Contains(Permission.DeletingMessages);

                    if (!isHavePermission)
                    {
                        _logger.WriteError($"Player try to delete post without permissions. ({postId})");
                        return;
                    }

                    context.Posts.Remove(post);
                    await context.SaveChangesAsync();

                    await UpdatR.SendUpdate(post.Chat, new DeleteMessageUpdate(post.Chat, postId));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::deletePost ({postId}) - " + e.ToString()); }
        }
    }
}
