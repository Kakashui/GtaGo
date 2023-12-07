using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Phone.Messenger.Dtos;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat.Updates
{
    internal class ChatUpdate : IUpdate<Domain.Phone.Messenger.Chat>
    {
        public Domain.Phone.Messenger.Chat UpdateTarget { get; }

        public ChatUpdate(Domain.Phone.Messenger.Chat chat)
        {
            UpdateTarget = chat;
        }
    }

    internal class ChatUpdateHandler : IUpdateHandler<ChatUpdate, Domain.Phone.Messenger.Chat>
    {
        private string _jsonChatDto;

        public async Task Handle(Player subscriber, ChatUpdate update)
        {
            if (!(subscriber is ExtPlayer player)) return;

            var dto = _jsonChatDto ?? ConvertMessageToJson(update.UpdateTarget);
            player.TriggerCefAction("smartphone/messagePage/msg_loadChat", dto);
        }

        private string ConvertMessageToJson(Domain.Phone.Messenger.Chat chat)
        {
            var chatDto = new ChatDto()
            {
                Id = chat.Id,
                Description = chat.Description,
                Name = chat.Name,
                Type = chat.Type,
                NonReadMessagesCount = 0,
                LastMessage = null,
                CreatedTime = chat.CreatedAt.GetTotalSeconds(),
                IconColors = chat.Avatar,
                IsMuted = false,
                IsOnline = true,
            };

            _jsonChatDto = JsonConvert.SerializeObject(chatDto);
            return _jsonChatDto;
        }
    }
}
