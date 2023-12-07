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
    internal class UserUnblockedUpdate : IUpdate<Domain.Phone.Messenger.Chat>
    {
        public Domain.Phone.Messenger.Chat UpdateTarget { get; }
        public int AccountId { get; }

        public UserUnblockedUpdate(Domain.Phone.Messenger.Chat chat, int accountId)
        {
            UpdateTarget = chat;
            AccountId = accountId;
        }
    }

    internal class UserUnblockedUpdateHandler : IUpdateHandler<UserUnblockedUpdate, Domain.Phone.Messenger.Chat>
    {
        private string _json;

        public async Task Handle(Player subscriber, UserUnblockedUpdate update)
        {
            if (!(subscriber is ExtPlayer player)) return;

            var json = _json ?? (_json = GetJson(update));
            player.TriggerCefEvent("smartphone/messagePage/msg_removeFromBlocklist", json);
        }

        private string GetJson(UserUnblockedUpdate update)
        {
            var dto = new { ChatId = update.UpdateTarget.Id, update.AccountId };
            return JsonConvert.SerializeObject(dto);
        }
    }
}
