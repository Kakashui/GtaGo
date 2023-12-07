using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
    internal class AddContactHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(AddContactHandler));

        [RemoteEvent("phone::contacts::add")]
        public async Task AddContact(ExtPlayer player, string contactJson)
        {
            try
            {
                if (!player.IsLogged())
                    return;

                var character = player.Character;

                var contactDto = JsonConvert.DeserializeObject<ContactDto>(contactJson);
                using (var context = DbManager.TemporaryContext)
                {
                    var phone = await context.Phones
                        .Include(p => p.SimCard)
                        .ThenInclude(s => s.Contacts)
                        .FirstOrDefaultAsync(p => p.CharacterUuid == character.UUID);

                    if (phone == null)
                    {
                        return;
                    }

                    if (phone.SimCard == null)
                    {
                        _logger.WriteError($"Player without simcard try to add contact ({contactJson}).");
                        return;
                    }

                    if (phone.SimCard.Contacts.Any(c => c.TargetNumber == contactDto.Number))
                    {
                        _logger.WriteError($"ExtPlayer to add already exists contact ({contactJson}).");
                        return;
                    }

                    var contact = new Contact
                    {
                        HolderSimCardId = (int) phone.SimCardId,
                        TargetNumber = contactDto.Number,
                        Name = contactDto.Name
                    };
                    context.Contacts.Add(contact);
                    await context.SaveChangesAsync();

                    var mapper = MapperManager.Get();
                    player.TriggerCefEvent("smartphone/addContact",
                        JsonConvert.SerializeObject(mapper.Map<ContactDto>(contact)));
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError(
                        $"Unhandled exception catched on phone::contacts::add ({player?.Name}, contactJson: {contactJson}) - " +
                        e.ToString());
                });
            }
        }
    }
}
