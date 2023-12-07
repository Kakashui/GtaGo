using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat.Updates
{
    internal class NewSubscriberUpdate : IUpdate<Domain.Phone.Messenger.Chat>
    {
        public Domain.Phone.Messenger.Chat UpdateTarget { get; }

        public NewSubscriberUpdate(Domain.Phone.Messenger.Chat updateTarget)
        {
            UpdateTarget = updateTarget;
        }
    }

    internal class NewSubscriberUpdateHandler : IUpdateHandler<NewSubscriberUpdate, Domain.Phone.Messenger.Chat>
    {
        public async Task Handle(Player subscriber, NewSubscriberUpdate update)
        {
            if (!(subscriber is ExtPlayer player)) return;

            player.TriggerCefEvent("smartphone/messagePage/msg_increaseSubscribersCount", update.UpdateTarget.Id);
        }
    }
}
