using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Dtos;
using Whistler.SDK;

namespace Whistler.Phone.Messenger.Chat
{
    internal class RequestAdminsHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(RequestAdminsHandler));

        [RemoteEvent("phone::msg::requestAdmins")]
        public async Task RequestAdmins(ExtPlayer player, int chatId)
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
                        .Where(ac => ac.AdminLvl == AdminLvl.Administrator)
                        .Include(ac => ac.Account)
                        .ToListAsync();

                    var mapper = MapperManager.Get();
                    var adminsDto = subscribers.Select(ac => mapper.Map<AdminDto>(ac));
                    player.TriggerCefAction("smartphone/messagePage/msg_setAdminsForCurrentChat", JsonConvert.SerializeObject(adminsDto));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::requestAdmins ({chatId}) - " + e.ToString()); }
        }
    }
}
