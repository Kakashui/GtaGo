using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
            var accountId = subscriber?.GetCharacter().PhoneTemporary?.Account?.Id;
            if (accountId == null || accountId == update.InitorAccountId)
                return;

            subscriber.TriggerCefEvent("smartphone/messagePage/msg_setOwnMessagesReadInChat", update.UpdateTarget.Id);
        }
    }
}
