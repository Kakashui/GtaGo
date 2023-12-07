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
using Whistler.Phone.Calls.Dtos;
using Whistler.SDK;

namespace Whistler.Phone.Calls
{
    internal class DropCallHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(DropCallHandler));

        [RemoteEvent("phone::calls::drop")]
        public async Task DropCall(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged())
                    return;

                var dropData = CallsManager.DropCall(player, "DropCall");
                if (dropData != null)
                {
                    await CallHistoryManager.AddCallHistoryItem(dropData.Value.call);
                    CallHistoryManager.SendPlayerCallDto(dropData.Value.from, dropData.Value.call);
                    CallHistoryManager.SendPlayerCallDto(dropData.Value.target, dropData.Value.call);
                }
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() =>
                {
                    _logger.WriteError($"Unhandled exception catched on phone::calls::drop ({player?.Name}) - " + e.ToString());
                });
            }
        }
    }
}
