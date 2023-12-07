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
    internal class DeleteChatUpdate : IUpdate<Domain.Phone.Messenger.Chat>
    {
        public Domain.Phone.Messenger.Chat UpdateTarget { get; }

        public DeleteChatUpdate(Domain.Phone.Messenger.Chat updateTarget)
        {
            UpdateTarget = updateTarget;
        }
    }

    internal class DeleteChatHandler : IUpdateHandler<DeleteChatUpdate, Domain.Phone.Messenger.Chat>
    {
        public async Task Handle(Player subscriber, DeleteChatUpdate update)
        {
            if (!(subscriber is ExtPlayer player)) return;

            player.TriggerCefAction("smartphone/messagePage/msg_deleteChat", update.UpdateTarget.Id);
        }
    }
}
