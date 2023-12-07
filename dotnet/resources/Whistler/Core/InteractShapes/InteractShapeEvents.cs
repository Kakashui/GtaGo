using GTANetworkAPI;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.Core.InteractShapes
{
    internal class InteractShapeEvents : Script
    {
        public static event Action<ExtPlayer> PlayerConnected;

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(InteractShapeEvents));

        [ServerEvent(Event.PlayerConnected)]
        public void HandlePlayerConnect(ExtPlayer player)
        {
            try
            {
                PlayerConnected?.Invoke(player);
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception on interact:interactKeyPressed - " + e.ToString()); }
        }

        [RemoteEvent("interact:interactKeyPressed")]
        public void InteractKeyPressed(ExtPlayer player, int keyNum)
        {
            try
            {
                var key = (Key)keyNum;
                InteractShape.PlayerPressInteractKeyHandlerStatic(player, key);
            }
            catch (Exception e) { _logger.WriteError("Unhandled exception on interact:interactKeyPressed - " + e.ToString()); }
        }
    }
}
