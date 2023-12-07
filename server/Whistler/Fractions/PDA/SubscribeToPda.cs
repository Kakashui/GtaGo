using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Fractions.PDA
{
    class SubscribeToPda
    {
        private static List<Player> _subscribers = new List<Player>();

        public static void Subscribe(Player player)
        {
            if (!_subscribers.Contains(player))
                _subscribers.Add(player);
        }

        public static void UnSubscribe(Player player)
        {
            if (_subscribers.Contains(player))
                _subscribers.Remove(player);
        }

        public static bool IsSubscribe(Player player)
        {
            return _subscribers.Contains(player);
        }

        public static void TriggerEventToSubscribers(string eventName, params object[] args)
        {
            Trigger.ClientEventToPlayers(_subscribers.ToArray(), eventName, args);
        }
    }
}
