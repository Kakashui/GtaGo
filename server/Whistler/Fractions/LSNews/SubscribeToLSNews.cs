using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Helpers;

namespace Whistler.Fractions.LSNews
{
    class SubscribeToLSNews
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

        public static void TriggerCefEventSubscribers(string storeFunction, object data)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.TriggerCefEvent(storeFunction, data);
            }
        }
    }
}
