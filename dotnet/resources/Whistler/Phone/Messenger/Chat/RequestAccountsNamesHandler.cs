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

namespace Whistler.Phone.Messenger.Chat
{
    internal class RequestAccountsNamesHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(RequestAccountsNamesHandler));

        [RemoteEvent("phone::msg::requestAccountsNames")]
        public async Task HandleRequestAccountNames(ExtPlayer player, string accountsIdsJson)
        {
            try
            {
                using (var context = DbManager.TemporaryContext)
                {
                    var accountsIds = JsonConvert.DeserializeObject<int[]>(accountsIdsJson);
                    var accounts = await context.Accounts
                        .Where(a => accountsIds.Contains(a.Id))
                        .Select(a => new AccountDto
                        {
                            Id = a.Id,
                            DisplayedName = a.DisplayedName
                        })
                        .ToListAsync();

                    player.TriggerCefEvent("smartphone/messagePage/msg_loadAccounts", JsonConvert.SerializeObject(accounts));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::requestAccountsNames ({accountsIdsJson}) - " + e.ToString()); }
        }
    }
}
