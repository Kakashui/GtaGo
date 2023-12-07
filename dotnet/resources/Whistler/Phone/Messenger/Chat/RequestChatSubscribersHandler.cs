using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Dtos;
using Whistler.SDK;

namespace Whistler.Phone.Messenger.Chat
{
    internal class RequestChatSubscribersHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(RequestChatSubscribersHandler));

        [RemoteEvent("phone::msg::requestSubscribers")]
        public async Task RequestSubscribers(ExtPlayer player, int chatId, string name)
        {
            try
            {
                var character = player.Character;
                var accountId = character.PhoneTemporary.Phone?.AccountId;

                if (accountId == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var subscribers = await context.AccountsToChats
                        .Where(ac => ac.ChatId == chatId)
                        .Where(ac => !ac.IsLeaved && !ac.IsBlocked)
                        .Include(ac => ac.Account)
                        .Where(ac => ac.Account.DisplayedName.Contains(name) || ac.Account.Username.Contains(name))
                        .ToListAsync();

                    var mapper = MapperManager.Get();
                    var subscribersDtos = subscribers.Select(ac => mapper.Map<SubscriberDto>(ac.Account));
                    player.TriggerCefEvent("smartphone/messagePage/msg_setSubscribersForSearch", JsonConvert.SerializeObject(subscribersDtos));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::requestSubscribers ({chatId}) - " + e.ToString()); }
        }
    }
}
