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
using Whistler.SDK;
using Whistler.Core;

namespace Whistler.Phone.Calls
{
    internal class MakeCallHandler : Script
    {
        public static Action<ExtPlayer, int> CallShortNumber;
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(MakeCallHandler));

        [RemoteEvent("phone::calls::call")]
        public async Task MakeCallAsync(ExtPlayer player, int targetNumber)
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
                        NAPI.Task.Run(() =>
                        {
                            _logger.WriteWarning(
                                $"Player try to call without phone or simcard ({player.Name}, {targetNumber}).");
                        });
                        return;
                    }

                    if (character.PhoneTemporary.Phone.SimCard.Number == targetNumber)
                    {
                        await SendPlayerFailedCall(player, targetNumber, "phone:call:1");
                        return;
                    }

                    if (targetNumber < 1000)
                    {
                        CallShortNumber?.Invoke(player, targetNumber);
                        await SendPlayerFailedCall(player, targetNumber, "phone:call:2");
                        return;
                    }

                    var targetSimcard = await context.SimCards
                        .FirstOrDefaultAsync(s => s.Number == targetNumber);
                    if (targetSimcard == null)
                    {
                        await SendPlayerFailedCall(player, targetNumber, "phone:call:3");
                        return;
                    }

                    var block = await context.BlockedContacts
                        .FindAsync(targetSimcard.Id, phone.SimCard.Number);
                    if (block != null)
                    {
                        await SendPlayerFailedCall(player, targetNumber, "phone:call:4");
                        return;
                    }

                    if (!CallsManager.SendCall(targetNumber, player, phone.SimCard.Number))
                    {
                        await SendPlayerFailedCall(player, targetNumber, "phone:call:5");
                    }
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError(
                        $"Unhandled exception catched on phone::calls:call ({player?.Name}, number: {targetNumber}) - " +
                        e.ToString());
                });
            }
        }

        private async Task SendPlayerFailedCall(ExtPlayer player, int targetNumber, string reason)
        {
            try
            {
                var simcard = player.Character.PhoneTemporary.Phone.SimCard;

                var call = new Call
                {
                    FromSimCardId = simcard.Id,
                    FromSimCard = simcard,
                    TargetNumber = targetNumber,
                    CreatedAt = DateTime.Now,
                    CallStatus = CallStatus.Failed,
                    Duration = 0
                };

                await CallHistoryManager.AddCallHistoryItem(call);

                CallHistoryManager.SendPlayerCallDto(player, call);
                player.TriggerCefEvent("smartphone/dropCall", JsonConvert.SerializeObject(new { reason }));
            }
            catch (Exception e)
            {
                _logger.WriteError($"SendPlayerFailedCall {e}");
            }
        }
    }
}
