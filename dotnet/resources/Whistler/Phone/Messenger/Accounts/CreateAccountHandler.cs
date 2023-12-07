using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Core.nAccount;
using Whistler.Domain.Phone.Contacts;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.SDK;
using Whistler.UpdateR;

namespace Whistler.Phone.Messenger.Accounts
{
    internal class CreateAccountHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(CreateAccountHandler));

        [RemoteEvent("phone::msg::createAccount")]
        public async Task HandleCreateAccount(ExtPlayer player, string username, string displayname)
        {
            try
            {
                var character = player.Character;
                using (var context = DbManager.TemporaryContext)
                {
                    if (character.PhoneTemporary.Phone == null)
                        return;

                    if (await context.Accounts.AnyAsync(a => a.Username == username))
                    {
                        Notify.SendError(player, "phone:acc:exists");
                        return;
                    }

                    var simcard = character.PhoneTemporary.Phone.SimCard;
                    var account = new Domain.Phone.Messenger.Account
                    {
                        Username = username,
                        SimCardId = simcard.Id,
                        DisplayedName = displayname,
                        IsNumberHided = false
                    };

                    context.Accounts.Add(account);
                    await context.SaveChangesAsync();

                    Domain.Phone.Phone phone = DbManager.GlobalContext.Phones.Find(character.UUID);
                    phone.AccountId = account.Id;
                    await DbManager.GlobalContext.SaveChangesAsync();

                    UpdatR.Subscribe(player, account);
                    character.PhoneTemporary.Phone.Account = account;

                    player.TriggerCefEvent("smartphone/messagePage/msg_setAccountSettings", JsonConvert.SerializeObject(new
                    {
                        account.Id,
                        account.Username,
                        account.DisplayedName,
                        account.IsNumberHided
                    }));
                    player.TriggerCefAction("smartphone/msg_goToChats", true);
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::msg::createAccount ({username}, {displayname}) - " + e.ToString()); }
        }
    }
}
