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

namespace Whistler.Phone.Messenger.Chat.Manage
{
    internal class RequestChatBlockedUsersHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(RequestChatBlockedUsersHandler));

        [RemoteEvent("phone::msg::manage::requestBlockedUsers")]
        public async Task HandleRequestBlockedUsers(ExtPlayer player, int chatId)
        {
            try
            {
                var character = player.Character;
                var accountId = character.PhoneTemporary.Phone?.AccountId;

                if (accountId == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var mapper = MapperManager.Get();
                    var blocklist = await context.AccountsToChats
                        .AsNoTracking()
                        .Where(ac => ac.ChatId == chatId)
                        .Where(ac => ac.IsBlocked)
                        .Include(ac => ac.Account)
                        .Include(ac => ac.BlockedBy)
                        .Select(ac => mapper.Map<BlockDto>(ac))
                        .ToListAsync();

                    player.TriggerCefEvent("smartphone/messagePage/msg_addBlocklistItems", JsonConvert.SerializeObject(new { ChatId = chatId, BlockItems = blocklist }));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::manage::requestBlockedUsers ({chatId}) - " + e.ToString()); }
        }
    }
}
