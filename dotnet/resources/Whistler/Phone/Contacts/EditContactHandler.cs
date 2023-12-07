using GTANetworkAPI;
using System;
using Newtonsoft.Json;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.SDK;
using Whistler.Phone.Contacts.Dtos;
using System.Threading.Tasks;
using Whistler.Entities;

namespace Whistler.Phone.Contacts
{
    internal class EditContactHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(EditContactHandler));

        [RemoteEvent("phone::contacts::edit")]
        public async Task EditContact(ExtPlayer player, string contactJson)
        {
            try
            {
                if (!player.IsLogged())
                    return;

                var character = player.Character;

                var contactDto = JsonConvert.DeserializeObject<ContactDto>(contactJson);
                using (var context = DbManager.TemporaryContext)
                {
                    var contact = await context.Contacts.FindAsync(contactDto.Id);

                    if (contact == null)
                    {
                        _logger.WriteError($"Player try to edit not exists contact ({contactJson}).");
                        return;
                    }

                    contact.Name = contactDto.Name;
                    contact.TargetNumber = contactDto.Number;

                    await context.SaveChangesAsync();

                    player.TriggerEventSafe("phone:contacts:applyEdit", contactJson);
                }
            }
            catch (Exception e) { _logger.WriteError($"Unhandled exception catched on phone::contacts::edit (contactJson: {contactJson}) - " + e.ToString()); }
        }
    }
}
