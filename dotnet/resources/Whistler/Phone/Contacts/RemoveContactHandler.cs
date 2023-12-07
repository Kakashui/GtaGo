using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.SDK;
using Newtonsoft.Json.Serialization;
using Whistler.Phone.Contacts.Dtos;
using Whistler.Domain.Phone.Contacts;
using Whistler.Entities;

namespace Whistler.Phone.Contacts
{
    internal class RemoveContactHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(RemoveContactHandler));

        [RemoteEvent("phone::contacts::remove")]
        public async Task RemoveContact(ExtPlayer player, int contactId)
        {
            try
            {
                if (!player.IsLogged())
                    return;

                var character = player.Character;

                using (var contex = DbManager.TemporaryContext)
                {
                    var phone = await contex.Phones.FindAsync(character.UUID);
                    if (phone == null || phone.SimCardId == null)
                    {
                        _logger.WriteError($"Player try to remove contact without phone ({contactId}).");
                        return;
                    }

                    var contact = await contex.Contacts.FindAsync(contactId);
                    if (contact == null)
                    {
                        _logger.WriteError($"Player try to remove not exists contact ({contactId}).");
                        return;
                    }

                    contex.Contacts.Remove(contact);
                    await contex.SaveChangesAsync();

                    player.TriggerCefEvent("smartphone/removeContact", contactId);
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::contacts::remove ({contactId}) - " + e.ToString()); }
        }
    }
}
