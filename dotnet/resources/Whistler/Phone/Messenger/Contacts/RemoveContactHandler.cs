using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.SDK;

namespace Whistler.Phone.Messenger.Contacts
{
    internal class RemoveContactHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(RemoveContactHandler));

        [RemoteEvent("phone::msg::removeContact")]
        public async Task RemoveContact(ExtPlayer player, int targetAccountId)
        {
            try
            {
                var character = player.Character;
                var accountId = character.PhoneTemporary.Phone?.AccountId;

                if (accountId == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var contact = await context.MsgContacts
                        .Include(c => c.TargetAccount)
                        .FirstOrDefaultAsync(c => c.TargetAccountId == targetAccountId);

                    if (contact == null)
                    {
                        NAPI.Task.Run(() =>
                        {
                            _logger.WriteError($"Player try remove non existing contact ({player.Name}, {targetAccountId})");
                        });
                        return;
                    }

                    var displayedName = contact.TargetAccount.DisplayedName;

                    context.MsgContacts.Remove(contact);
                    await context.SaveChangesAsync();

                    player.TriggerCefAction("smartphone/messagePage/msg_removeContact",
                        JsonConvert.SerializeObject(new {contact.ContactId, AccountDisplayedName = displayedName}));
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError($"Unhandled exception catched on phone::msg::addcontact ({player?.Name}, {targetAccountId}) - " + e.ToString());
                });
            }
        }
    }
}
