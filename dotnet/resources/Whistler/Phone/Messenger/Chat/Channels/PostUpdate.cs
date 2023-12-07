using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Phone.Messenger.Dtos;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat.Channels
{
    internal class PostUpdate : IUpdate<Domain.Phone.Messenger.Chat>
    {
        public Domain.Phone.Messenger.Chat UpdateTarget { get; }
        public Post Post { get; }

        public PostUpdate(Domain.Phone.Messenger.Chat chat, Post post)
        {
            UpdateTarget = chat;
            Post = post;
        }
    }

    internal class PostUpdateHandler : IUpdateHandler<PostUpdate, Domain.Phone.Messenger.Chat>
    {
        private string _jsonPostDto;

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(PostUpdateHandler));

        public async Task Handle(Player subscriber, PostUpdate update)
        {
            try
            {
                if (!(subscriber is ExtPlayer player)) return;

                var dto = _jsonPostDto ?? ConvertMessageToJson(update.Post);
                player.TriggerCefAction("smartphone/messagePage/msg_editMessage", dto);
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on postupdate Handle () - " + e.ToString()); }
        }

        private string ConvertMessageToJson(Post post)
        {
            var mapper = MapperManager.Get();
            var dto = mapper.Map<PostDto>(post);

            _jsonPostDto = JsonConvert.SerializeObject(new { post.ChatId, Message = dto });
            return _jsonPostDto;
        }
    }
}
