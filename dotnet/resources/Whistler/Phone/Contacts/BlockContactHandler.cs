using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
    internal class BlockContactHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(BlockContactHandler));

        [RemoteEvent("phone::contacts::block")]
        public async Task BlockContact(ExtPlayer player, int number)
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
                        _logger.WriteError($"Player try to block number without phone or simcard ({number}).");
                        return;
                    }

                    if (await context.BlockedContacts.AnyAsync(b =>
                        b.SimCardId == phone.SimCardId && b.TargetNumber == number))
                    {
                        _logger.WriteError($"Player try to block already blocked number ({number}).");
                        return;
                    }

                    var blockItem = new Block
                    {
                        SimCardId = (int) phone.SimCardId,
                        TargetNumber = number
                    };

                    context.BlockedContacts.Add(blockItem);
                    await context.SaveChangesAsync();

                    var mapper = MapperManager.Get();
                    player.TriggerCefEvent("smartphone/addBlockItem",
                        JsonConvert.SerializeObject(mapper.Map<BlockDto>(blockItem)));
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError($"Unhandled exception catched on phone::contacts::block ({player?.Name}, number: {number}) - " + e.ToString());
                });
            }
        }
    }
}
