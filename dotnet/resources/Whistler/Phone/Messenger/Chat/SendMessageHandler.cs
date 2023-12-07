using GTANetworkAPI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Chat.Updates;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat
{
    internal class SendMessageHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(SendMessageHandler));

        [RemoteEvent("phone::msg::sendMessage")]
        public async Task SendMessage(ExtPlayer player, int chatId, string messageJson)
        {
            try
            {
                var character = player.Character;
                var accountId = character.PhoneTemporary.Phone?.AccountId;

                if (accountId == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var chat = await context.Chats.FindAsync(chatId);
                    if (chat == null)
                    {
                        _logger.WriteError($"Player try send message to non-existing chat. ({chatId})");
                        return;
                    }

                    var accountToChat = await context.AccountsToChats.FindAsync(accountId, chatId);
                    if (accountToChat == null || accountToChat.IsLeaved)
                    {
                        _logger.WriteError($"Player try send message to chat where he is not in. ({chatId})");
                        return;
                    }

                    if (accountToChat.IsBlocked)
                    {
                        _logger.WriteError($"Player try send message to chat where he is blocked in. ({chatId})");
                        return;
                    }

                    var messageObj = JObject.Parse(messageJson);

                    Message message = new Message
                    {
                        Text = messageObj["text"]?.ToString(),
                        SenderId = (int)accountId,
                        ChatId = chatId,
                        Attachments = GetAttachments(player, messageObj)
                    };

                    context.Messages.Add(message);
                    await context.SaveChangesAsync();

                    await UpdatR.SendUpdate(chat, new NewMessageUpdate(chat, message));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::sendMessage ({chatId}) - " + e.ToString()); }
        }

        private List<Attachment> GetAttachments(ExtPlayer player, JObject messageObj)
        {
            List<Attachment> attachments = new List<Attachment>();
            if (messageObj["attachment"] == null) return attachments;

            JToken attachmentObj = messageObj["attachment"];
            AttachmentType type = attachmentObj["type"].ToObject<AttachmentType>();

            switch (type)
            {
                /*
                case AttachmentType.GeoPosition:
                    attachments.Add(new GeoPosition(player.Position.X, player.Position.Y, player.Position.Z));
                    break;
                */
                case AttachmentType.Sound:
                    attachments.Add(new Sound(attachmentObj["soundUrl"].ToString()));
                    break;
                case AttachmentType.Photo:
                    attachments.Add(new Photo(attachmentObj["photoUrl"].ToString()));
                    break;
            }

            return attachments;
        }
    }
}
