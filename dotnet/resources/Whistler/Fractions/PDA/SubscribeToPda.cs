using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.Fractions.PDA
{
    class SubscribeToPda
    {
        private static List<ExtPlayer> _subscribers = new List<ExtPlayer>();

        public static void Subscribe(ExtPlayer player)
        {
            if (!_subscribers.Contains(player))
                _subscribers.Add(player);
        }

        public static void UnSubscribe(ExtPlayer player)
        {
            if (_subscribers.Contains(player))
                _subscribers.Remove(player);
        }

        public static bool IsSubscribe(ExtPlayer player)
        {
            return _subscribers.Contains(player);
        }

        public static void TriggerEventToSubscribers(string eventName, params object[] args)
        {
            SafeTrigger.ClientEventToPlayers(_subscribers.ToArray(), eventName, args);
        }
    }
}
