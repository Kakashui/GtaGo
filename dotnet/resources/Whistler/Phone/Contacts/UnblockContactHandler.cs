using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Whistler.Domain.Phone.Contacts;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Contacts.Dtos;
using Whistler.SDK;

namespace Whistler.Phone.Contacts
{
    internal class UnblockContactHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(UnblockContactHandler));

        [RemoteEvent("phone::contacts::unblock")]
        public async Task UnblockContact(ExtPlayer player, int number)
        {
            try
            {
                if (!player.IsLogged())
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var character = player.Character;

                    var phone = await context.Phones
                        .Include(p => p.SimCard)
                        .FirstOrDefaultAsync(p => p.CharacterUuid == character.UUID);

                    if (phone == null || phone.SimCardId == null)
                    {
                        _logger.WriteError($"Player try to unblock number without phone or simcard ({number}).");
                        return;
                    }

                    var blockItem = await context.BlockedContacts
                        .FindAsync(phone.SimCardId, number);

                    if (blockItem == null)
                    {
                        _logger.WriteError($"Player try to unblock non blocked number ({number}).");
                        return;
                    }

                    context.BlockedContacts.Remove(blockItem);
                    await context.SaveChangesAsync();

                    player.TriggerCefEvent("smartphone/removeBlockItem", number);
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::contacts::unblock (number: {number}) - " + e); }
        }
    }
}
