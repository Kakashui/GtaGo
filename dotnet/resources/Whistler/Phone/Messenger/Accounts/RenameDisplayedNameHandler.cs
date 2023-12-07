using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.SDK;

namespace Whistler.Phone.Messenger.Accounts
{
    internal class RenameDisplayedNameHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(RenameDisplayedNameHandler));

        [RemoteEvent("phone::msg::renameDisplayedName")]
        public async Task HandleRenameDisplayedName(ExtPlayer player, string newDisplayedName)
        {
            try
            {
                using (var context = DbManager.TemporaryContext)
                {
                    var accountId = player.Character.PhoneTemporary.Phone.Account.Id;
                    var account = await context.Accounts.FindAsync(accountId);
                    account.DisplayedName = newDisplayedName;

                    await context.SaveChangesAsync();

                    player.TriggerCefEvent($"smartphone/messagePage/msg_setAccountDisplayedName", JsonConvert.SerializeObject(new { newDisplayedName }));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::renameDisplayedName ({newDisplayedName}) - " + e.ToString()); }
        }
    }
}
