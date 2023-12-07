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
using Whistler.Phone.Messenger.Chat;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Accounts
{
    internal class ToggleBlockAccountHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ToggleBlockAccountHandler));

        [RemoteEvent("phone::msg::blockAccount")]
        public async Task ToggleBlockAccountAsync(ExtPlayer player, int targetAccountId, bool toggle)
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
                        .FirstOrDefaultAsync(a => a.Id == targetAccountId);

                    if (targetAccount == null)
                    {
                        _logger.WriteError($"Player try block non existing account ({targetAccountId})");
                        return;
                    }

                    var privateChat = await ChatsCommon.GetPrivateChat(context, (int)accountId, targetAccountId);
                    if (privateChat == null)
                    {
                        privateChat = await ChatsCommon.CreatePrivateChat(context, character.PhoneTemporary.Phone.Account, targetAccount);
                    }

                    privateChat.AccountToChats.First(ac => ac.AccountId == accountId)
                        .IsMuted = toggle;

                    await context.SaveChangesAsync();

                    player.TriggerCefAction("smartphone/messagePage/msg_setAccountBlocked",
                        JsonConvert.SerializeObject(new { AccountId = targetAccountId, IsBlocked = toggle }));

                    await UpdatR.SendUpdate(targetAccount, new AccountBlockInChatUpdate(targetAccount, privateChat.Id, toggle));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::blockAccount ({targetAccountId}, {toggle}) - " + e.ToString()); }
        }
    }
}
