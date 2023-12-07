using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Whistler.Core.Character;
using Whistler.Domain.Phone.Contacts;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Infrastructure.DataAccess;
using Whistler.Phone.Calls.Dtos;
using Whistler.SDK;

namespace Whistler.Phone.Calls
{
    internal class CallHistoryManager : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(CallHistoryManager));

        public CallHistoryManager()
        {
            PhoneLoader.PhoneReadyAsync += LoadPlayerCallhistory;
        }

        private async Task LoadPlayerCallhistory(ExtPlayer player, Character character)
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

                    var calls = await context.CallHistory
                        .Include(c => c.FromSimCard)
                        .Where(c => c.FromSimCardId == phone.SimCardId || c.TargetNumber == phone.SimCard.Number)
                        .OrderBy(c => c.CreatedAt)
                        .Take(15)
                        .ToListAsync();

                    var callDtos = calls.Select(c => GetCallDtoForSim(c, phone.SimCard));

                    player.TriggerCefEvent("smartphone/loadCallhistory",
                        JsonConvert.SerializeObject(callDtos));
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError($"Unhandled exception catched on LoadPlayerContacts ({player?.Name}) - " + e.ToString());
                });
            }
        }

        public static void SendPlayerCallDto(ExtPlayer player, Call call)
        {
            var simcard = player.Character.PhoneTemporary.Phone.SimCard;
            var calldto = GetCallDtoForSim(call, simcard);

            player.TriggerCefEvent("smartphone/loadCallhistory",
                JsonConvert.SerializeObject(new List<CallHistoryItemDto> { calldto }));
        }

        public static async Task AddCallHistoryItem(Call callHistoryItem)
        {
            try
            {
                using (var context = DbManager.TemporaryContext)
                {
                    context.SimCards.Attach(callHistoryItem.FromSimCard);

                    context.CallHistory.Add(callHistoryItem);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception catched on AddCallHistoryItem: " + e.ToString()); }
        }

        private static CallHistoryItemDto GetCallDtoForSim(Call call, SimCard simCard)
        {
            return new CallHistoryItemDto
            {
                Id = call.Id,
                Number = call.FromSimCardId == simCard.Id ? call.TargetNumber : call.FromSimCard.Number,
                IsSuccessful = call.CallStatus == CallStatus.Accepted,
                // 1 - outgoing, 0 - income
                Type = call.FromSimCardId == simCard.Id ? 1 : 0,
                Duration = call.Duration,
                Date = call.CreatedAt.GetTotalSeconds()
            };
        }
    }
}
