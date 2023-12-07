using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Fractions.LSNews
{
    class SubscribeToLSNews
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

        public static void TriggerCefEventSubscribers(string storeFunction, object data)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.TriggerCefEvent(storeFunction, data);
            }
        }
    }
}
