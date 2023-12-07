using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.SDK;
using Whistler.UpdateR;
using Whistler.Entities;

namespace Whistler.Phone.Contacts
{
    internal class ForwardToMessengerHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ForwardToMessengerHandler));

        [RemoteEvent("phone::contacts::goToMsg")]
        public async Task HandleForwardToMessenger(ExtPlayer player, int number)
        {
            try
            {
                var character = player.Character;
                var account = character.PhoneTemporary.Phone?.Account;

                if (account == null)
                {
                    try
                    {
                        using (var context = DbManager.TemporaryContext)
                        {
                            if (character.PhoneTemporary.Phone == null)
                                return;

                            if (character.PhoneTemporary.Phone.SimCard == null)
                                return;

                            // if (character.PhoneTemporary.Account != null)
                            //     return;

                            // if (await context.Accounts.AnyAsync(a => a.Username == username))
                            // {
                            //     Notify.SendError(player, "phone:acc:exists");
                            //     return;
                            // }

                            var simcard = character.PhoneTemporary.Phone.SimCard;
                            account = new Domain.Phone.Messenger.Account
                            {
                                Username = character.FullName,
                                SimCardId = simcard.Id,
                                DisplayedName = character.FullName,
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
                    catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::contacts::goToMsg (create account) - " + e.ToString()); }
                }

                using (var context = DbManager.TemporaryContext)
                {
                    // var targetAccount =
                    var targetAccount = await context.Accounts
                        .Include(a => a.SimCard)
                        .FirstOrDefaultAsync(a => a.SimCard.Number == number);

                    if (targetAccount == null) // || targetAccount.IsNumberHided
                    {
                        Notify.SendError(player, "Невозможно найти пользователя");
                        return;
                    }

                    player.TriggerCefAction("smartphone/messagePage/msg_openPrivateChat", targetAccount.Id);
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::contacts::goToMsg (number: {number}) - " + e.ToString()); }
        }
    }
}
