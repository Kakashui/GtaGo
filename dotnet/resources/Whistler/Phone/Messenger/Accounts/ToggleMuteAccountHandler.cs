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
using Whistler.Phone.Messenger.Chat;
using Whistler.SDK;

namespace Whistler.Phone.Messenger.Accounts
{
    internal class ToggleMuteAccountHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ToggleMuteAccountHandler));

        [RemoteEvent("phone::msg::muteAccount")]
        public async Task MuteAccountAsync(ExtPlayer player, int targetAccountId, bool toggle)
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
                        _logger.WriteError($"Player try mute non existing account ({targetAccountId})");
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

                    player.TriggerCefAction("smartphone/messagePage/msg_setAccountAndChatMuted",
                        JsonConvert.SerializeObject(new { ChatId = privateChat.Id, AccountId = targetAccountId, IsMuted = toggle }));
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::muteAccount ({targetAccountId}, {toggle}) - " + e.ToString()); }
        }
    }
}
