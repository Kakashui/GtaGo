using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat.Updates
{
    internal class DeleteMessageUpdate : IUpdate<Domain.Phone.Messenger.Chat>
    {
        public Domain.Phone.Messenger.Chat UpdateTarget { get; }
        public int MessageId { get; }

        public DeleteMessageUpdate(Domain.Phone.Messenger.Chat chat, int messageId)
        {
            UpdateTarget = chat;
            MessageId = messageId;
        }
    }

    internal class DeleteMessageHandler : IUpdateHandler<DeleteMessageUpdate, Domain.Phone.Messenger.Chat>
    {
        public async Task Handle(Player subscriber, DeleteMessageUpdate update)
        {
            if (!(subscriber is ExtPlayer player)) return;

            player.TriggerCefAction("smartphone/messagePage/msg_deleteMessage", update.MessageId);
        }
    }
}
