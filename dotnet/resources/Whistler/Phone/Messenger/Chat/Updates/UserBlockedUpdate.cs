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
    internal class UserBlockedUpdate : IUpdate<Domain.Phone.Messenger.Chat>
    {
        public Domain.Phone.Messenger.Chat UpdateTarget { get; }
        public int AccountId { get; }
        public string DisplayedName { get; }
        public string BlockedBy { get; }

        public UserBlockedUpdate(Domain.Phone.Messenger.Chat chat, int accountId, string displayedName, string blockedBy)
        {
            UpdateTarget = chat;
            AccountId = accountId;
            DisplayedName = displayedName;
            BlockedBy = blockedBy;
        }
    }

    internal class UserBlockedUpdateHandler : IUpdateHandler<UserBlockedUpdate, Domain.Phone.Messenger.Chat>
    {
        private string _json;

        public async Task Handle(Player subscriber, UserBlockedUpdate update)
        {
            if (!(subscriber is ExtPlayer player)) return;

            var json = _json ?? (_json = GetJson(update));
            player.TriggerCefEvent("smartphone/messagePage/msg_addBlocklistItems", json);
        }

        private string GetJson(UserBlockedUpdate update) 
        {
            var blockItem = new BlockDto
            {
                AccountId = update.AccountId,
                DisplayedName = update.DisplayedName,
                BlockedBy = update.BlockedBy
            };

            var dto = new { ChatId = update.UpdateTarget.Id, BlockItems = new List<BlockDto> { blockItem } };
            return JsonConvert.SerializeObject(dto);
        }
    }
}
