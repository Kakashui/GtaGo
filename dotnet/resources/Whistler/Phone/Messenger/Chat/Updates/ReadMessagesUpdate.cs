using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Chat.Updates
{
    internal class ReadMessagesUpdate : IUpdate<Domain.Phone.Messenger.Chat>
    {
        public Domain.Phone.Messenger.Chat UpdateTarget { get; }
        public int InitorAccountId { get; }

        public ReadMessagesUpdate(Domain.Phone.Messenger.Chat updateTarget, int initorAccountId)
        {
            UpdateTarget = updateTarget;
            InitorAccountId = initorAccountId;
        }
    }

    internal class ReadMessagesUpdateHandler : IUpdateHandler<ReadMessagesUpdate, Domain.Phone.Messenger.Chat>
    {
        public async Task Handle(Player subscriber, ReadMessagesUpdate update)
        {
            if (!(subscriber is ExtPlayer player)) return;

            var accountId = player.Character.PhoneTemporary?.Phone?.Account?.Id;
            if (accountId == null || accountId == update.InitorAccountId)
                return;

            player.TriggerCefEvent("smartphone/messagePage/msg_setOwnMessagesReadInChat", update.UpdateTarget.Id);
        }
    }
}
