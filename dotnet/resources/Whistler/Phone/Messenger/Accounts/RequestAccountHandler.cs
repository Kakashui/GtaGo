using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

namespace Whistler.Phone.Messenger.Accounts
{
    internal class RequestAccountHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(RequestAccountHandler));

        [RemoteEvent("phone::msg::requestAccount")]
        public async Task HandleRequestAccount(ExtPlayer player, int reqAccountId)
        {
            try
            {
                var character = player.Character;
                var accountId = character.PhoneTemporary.Phone?.AccountId;

                if (accountId == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var targetAccount = await context.Accounts
                        .Include(a => a.SimCard)
                        .FirstOrDefaultAsync(a => a.Id == reqAccountId);

                    if (targetAccount == null)
                    {
                        _logger.WriteError($"Player requested not existing full account ({reqAccountId})");
                        return;
                    }

                    var mapper = MapperManager.Get();
                    var dto = mapper.Map<FullAccountDto>(targetAccount);

                    var privateChat = await context.Chats
                        .Where(c => c.Type == Domain.Phone.Messenger.ChatType.Private)
                        .Include(c => c.AccountToChats)
                        .Where(c => c.AccountToChats.Any(ac => ac.AccountId == accountId) &&
                            c.AccountToChats.Any(ac => ac.AccountId == targetAccount.Id))
                        .FirstOrDefaultAsync();

                    if (privateChat != null)
                    {
                        dto.IsMuted = privateChat.AccountToChats.First(a => a.AccountId == accountId).IsMuted;
                        dto.IsBlocked = privateChat.AccountToChats.First(a => a.AccountId == accountId).IsBlocked;

                    }

                    player.TriggerCefEvent("smartphone/messagePage/msg_setViewedProfileInfo", JsonConvert.SerializeObject(dto));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::requestAccount ({reqAccountId}) - " + e.ToString()); }
        }
    }
}
