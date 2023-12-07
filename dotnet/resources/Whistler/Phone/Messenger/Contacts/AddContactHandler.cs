using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Domain.Phone.Messenger;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Messenger.Dtos;
using Whistler.SDK;

namespace Whistler.Phone.Messenger.Contacts
{
    internal class AddContactHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(AddContactHandler));

        [RemoteEvent("phone::msg::addcontact")]
        public async Task AddContact(ExtPlayer player, string username, string contactName)
        {
            try
            {
                var character = player.Character;
                var accountId = character.PhoneTemporary.Phone?.AccountId;

                if (accountId == null)
                    return;

                using (var context = DbManager.TemporaryContext)
                {
                    var targetAccount = await context.Accounts
                        .FirstOrDefaultAsync(a => a.Username == username);

                    if (targetAccount == null)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom,
                            "phone:cont:add:noname", 3000);
                        return;
                    }

                    var contact = await context.MsgContacts
                        .FirstOrDefaultAsync(c =>
                            c.HolderAccountId == accountId && c.TargetAccountId == targetAccount.Id);

                    if (contact != null)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom,
                            "phone:cont:add:already".Translate(contact.Name), 3000);
                        return;
                    }

                    contact = new MsgContact
                    {
                        HolderAccountId = (int) accountId,
                        TargetAccountId = targetAccount.Id,
                        Name = contactName
                    };

                    context.MsgContacts.Add(contact);
                    await context.SaveChangesAsync();

                    var mapper = MapperManager.Get();
                    var contactDto = mapper.Map<MsgContactDto>(contact);
                    player.TriggerCefAction("smartphone/messagePage/msg_addContact", JsonConvert.SerializeObject(contactDto));
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError(
                        $"Unhandled exception catched on phone::msg::addcontact ({player?.Name}, {username}, {contactName}) - " +
                        e.ToString());
                });
            }
        }
    }
}
