using GTANetworkAPI;
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
    internal class SwitchNumberHidedHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(SwitchNumberHidedHandler));

        [RemoteEvent("phone::msg::switchNumberHided")]
        public async Task HandleSwitchNumberHidedAsync(ExtPlayer player, bool isNumberHided)
        {
            try
            {
                using (var context = DbManager.TemporaryContext)
                {
                    var accountId = player.Character.PhoneTemporary.Phone.Account.Id;
                    var account = await context.Accounts.FindAsync(accountId);
                    account.IsNumberHided = isNumberHided;

                    await context.SaveChangesAsync();

                    player.TriggerCefEvent($"smartphone/messagePage/msg_setAccountIsNumberHided", isNumberHided);
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::switchNumberHided ({isNumberHided}) - " + e.ToString()); }
        }
    }
}
