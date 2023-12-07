using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.SDK;

namespace Whistler.Phone.Contacts
{
    internal class ForwardToMessengerHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ForwardToMessengerHandler));

        [RemoteEvent("phone::contacts::goToMsg")]
        public async Task HandleForwardToMessenger(Player player, int number)
        {
            try
            {
                var character = player.GetCharacter();
                var account = character.PhoneTemporary.Phone?.Account;

                if (account == null)
                {
                    Notify.SendError(player, "phone:msgr:1");
                    return;
                }

                using (var context = DbManager.TemporaryContext)
                {
                    var targetAccount = await context.Accounts
                        .Include(a => a.SimCard)
                        .FirstOrDefaultAsync(a => a.SimCard.Number == number);

                    if (targetAccount == null || targetAccount.IsNumberHided)
                    {
                        Notify.SendError(player, "phone:msgr:2");
                        return;
                    }

                    player.TriggerCefAction("smartphone/messagePage/msg_openPrivateChat", targetAccount.Id);
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::contacts::goToMsg (number: {number}) - " + e.ToString()); }
        }
    }
}
