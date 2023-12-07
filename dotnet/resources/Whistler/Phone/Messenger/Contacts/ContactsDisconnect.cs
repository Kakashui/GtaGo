using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Whistler.Core.Character;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Contacts.Updates;

namespace Whistler.Phone.Messenger.Contacts
{
    class ContactsDisconnect
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ContactsDisconnect));



        public static async Task DisconnectCharacter(Character character)
        {
            try
            {
                var accountId = character.PhoneTemporary.Phone?.AccountId;

                if (accountId == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var accountWithMyContact = await context.MsgContacts
                        .Where(c => c.TargetAccountId == accountId)
                        .Include(c => c.HolderAccount)
                        .ToListAsync();
                    accountWithMyContact.ForEach(item => {
                        UpdateR.UpdatR.SendUpdate(item.HolderAccount, new ContactOnlineUpdate(item.HolderAccount, item.ContactId, false));
                    });
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError($"Unhandled exception catched on LoadPlayerMessengerContacts ({character?.FullName}) - " + e.ToString());
                });
            }
        }
    }
}
