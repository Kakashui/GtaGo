using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.SDK;
using Whistler.Phone.Contacts.Dtos;
using Whistler.Domain.Phone.Contacts;
using Whistler.Core.Character;
using Whistler.Entities;

namespace Whistler.Phone.Contacts
{
    internal class ContactsLoader : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ContactsLoader));

        public ContactsLoader()
        {
            PhoneLoader.PhoneReadyAsync += LoadPlayerContactsInfo;
        }

        private async Task LoadPlayerContactsInfo(ExtPlayer player, Character character)
        {
            try
            {
                using (var context = DbManager.TemporaryContext)
                {
                    var phone = character.PhoneTemporary.Phone;

                    if (phone == null || phone.SimCardId == null)
                    {
                        return;
                    }

                    await LoadPlayerContacts(player, context, phone);
                    await LoadPlayerBlocks(player, context, phone);
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                    _logger.WriteError($"Unhandled exception catched on LoadPlayerContacts ({player?.Name}) - " + e));
            }
        }

        private async Task LoadPlayerContacts(ExtPlayer player, ServerContext context, Domain.Phone.Phone phone)
        {
            var contacts = await context.Contacts
                .Where(c => c.HolderSimCardId == phone.SimCardId)
                .ToListAsync();

            var mapper = MapperManager.Get();
            player.TriggerEventWithLargeList(50, "phone:contacts:load",
                contacts.Select(c => mapper.Map<ContactDto>(c)));
        }

        private async Task LoadPlayerBlocks(ExtPlayer player, ServerContext context, Domain.Phone.Phone phone)
        {
            var blocks = await context.BlockedContacts
                .Where(b => b.SimCardId == phone.SimCardId)
                .ToListAsync();

            var mapper = MapperManager.Get();
            player.TriggerCefEventWithLargeList(100, "smartphone/addBlockItems", blocks.Select(source => mapper.Map<BlockDto>(source)));
        }
    }
}
