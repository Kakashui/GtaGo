using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Phone.Calls
{
    internal class TakeCallHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(TakeCallHandler));

        [RemoteEvent("phone::calls::take")]
        public void TakeCall(ExtPlayer player, int number)
        {
            try
            {
                if (!player.IsLogged())
                    return;

                CallsManager.TakeCall(player);
            }
            catch (Exception e)
            {
                NAPI.Task.Run(() => _logger.WriteError($"Unhandled exception catched on phone::calls::take ({player?.Name}, {number}) - " + e.ToString()));
            }
        }
    }
}
