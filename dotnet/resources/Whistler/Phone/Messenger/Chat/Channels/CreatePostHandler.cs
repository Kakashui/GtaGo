using GTANetworkAPI;
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
    internal class CreatePostHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(CreatePostHandler));

        [RemoteEvent("phone::msg::createPost")]
        public async Task CreatePost(ExtPlayer player, int chatId, string imgUrl, string title, string text)
        {
            try
            {
                var character = player.Character;
                var account = character.PhoneTemporary.Phone.Account;

                if (account == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var accountToChat = await context.AccountsToChats.FindAsync(account.Id, chatId);
                    var isHavePermission = accountToChat.Permissions.Contains(Permission.CreatingPosts);

                    if (!isHavePermission)
                    {
                        _logger.WriteError($"Player try to create post without permissions. ({chatId})");
                        return;
                    }

                    var post = new Post
                    {
                        SenderId = account.Id,
                        ChatId = chatId,
                        Text = text,
                        Title = title,
                        Photo = imgUrl
                    };

                    var chat = await context.Chats.FindAsync(chatId);

                    context.Posts.Add(post);
                    await context.SaveChangesAsync();

                    await UpdatR.SendUpdate(chat, new NewMessageUpdate(chat, post));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::createPost ({imgUrl}, {title}, {text}) - " + e.ToString()); }
        }
    }
}
