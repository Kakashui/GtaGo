using AutoMapper;
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
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat.Updates
{
    internal class NewMessageUpdate : IUpdate<Domain.Phone.Messenger.Chat>
    {
        public Domain.Phone.Messenger.Chat UpdateTarget { get; }
        public Message Message { get; }

        public NewMessageUpdate(Domain.Phone.Messenger.Chat chat, Message message)
        {
            UpdateTarget = chat;
            Message = message;
        }
    }

    internal class NewMessageHandler : IUpdateHandler<NewMessageUpdate, Domain.Phone.Messenger.Chat>
    {
        private string _jsonMsgDto;

        public async Task Handle(Player subscriber, NewMessageUpdate update)
        {
            if (!(subscriber is ExtPlayer player)) return;

            // UpdatR создает 1 инстант хэндлера на весь апдейт (на всех подписчиков события)
            // Поэтому маппер создастся только один раз на условно 100 человек.
            var dto = _jsonMsgDto ?? ConvertMessageToJson(update.Message);
            player.TriggerCefAction("smartphone/messagePage/msg_sendMessage", dto);
        }

        private string ConvertMessageToJson(Message message)
        {
            var mapper = MapperManager.Get();
            var dto = mapper.Map<MessageDto>(message);

            _jsonMsgDto = JsonConvert.SerializeObject(new { message.ChatId, Message = dto });
            return _jsonMsgDto;
        }
    }
}
