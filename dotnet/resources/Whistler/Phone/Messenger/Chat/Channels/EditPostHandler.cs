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
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat.Channels
{
    internal class EditPostHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(EditPostHandler));

        [RemoteEvent("phone::msg::editPost")]
        public async Task EditPost(ExtPlayer player, int postId, string imgUrl, string title, string text)
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
                        _logger.WriteError($"Player try to edit non-existing post. ({postId})");
                        return;
                    }

                    var accountToChat = await context.AccountsToChats.FindAsync(account.Id, post.ChatId);
                    var isHavePermission = accountToChat.Permissions.Contains(Permission.CreatingPosts);

                    if (!isHavePermission)
                    {
                        _logger.WriteError($"Player try to edit post without permissions. ({postId})");
                        return;
                    }

                    post.Photo = imgUrl;
                    post.Title = title;
                    post.Text = text;

                    await context.SaveChangesAsync();

                    await UpdatR.SendUpdate(post.Chat, new PostUpdate(post.Chat, post));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::editPost ({imgUrl}, {title}, {text}) - " + e.ToString()); }
        }
    }
}
