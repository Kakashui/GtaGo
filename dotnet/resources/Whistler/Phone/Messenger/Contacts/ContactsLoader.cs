using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Core.Character;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Contacts.Updates;
using Whistler.Phone.Messenger.Dtos;
using Whistler.SDK;

namespace Whistler.Phone.Messenger.Contacts
{
    internal class ContactsLoader : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ContactsLoader));

        public ContactsLoader()
        {
            PhoneLoader.PhoneReadyAsync += LoadPlayerMessengerContacts;
        }

        private async Task LoadPlayerMessengerContacts(ExtPlayer player, Character character)
        {
            try
            {
                var accountId = character.PhoneTemporary.Phone?.AccountId;

                if (accountId == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var contacts = await context.MsgContacts
                        .Where(c => c.HolderAccountId == accountId)
                        .Include(c => c.TargetAccount)
                        .ToListAsync();
                    var accountWithMyContact = await context.MsgContacts
                        .Where(c => c.TargetAccountId == accountId)
                        .Include(c => c.HolderAccount)
                        .ToListAsync();
                    accountWithMyContact.ForEach(item => {
                        UpdateR.UpdatR.SendUpdate(item.HolderAccount, new ContactOnlineUpdate(item.HolderAccount, item.ContactId, true));
                    });
                    var mapper = MapperManager.Get();

                    player.TriggerCefEventWithLargeList(300, "smartphone/messagePage/msg_loadContacts",
                        contacts.Select(c => mapper.Map<MsgContactDto>(c)));
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError($"Unhandled exception catched on LoadPlayerMessengerContacts ({player?.Name}) - " + e.ToString());
                });
            }
        }
    }
}
