using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Phone.Calls
{
    internal class TakeCallHandler : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(TakeCallHandler));

        [RemoteEvent("phone::calls::take")]
        public void TakeCall(Player player, int number)
        {
            try
            {
                if (!player.IsLogged())
                    return;

                CallsManager.TakeCall(player);
            }
            catch (Exception e)
            {
                WhistlerTask.Run(() => _logger.WriteError($"Unhandled exception catched on phone::calls::take ({player?.Name}, {number}) - " + e.ToString()));
            }
        }
    }
}
