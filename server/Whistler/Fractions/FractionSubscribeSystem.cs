using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Helpers;

namespace Whistler.Fractions
{
    class FractionSubscribeSystem
    {
        private static Dictionary<int, List<Player>> _subscribers = new Dictionary<int, List<Player>>();

        public static void Subscribe(Player player, int fractionId)
        {
            lock (_subscribers)
            {
                if (!_subscribers.ContainsKey(fractionId))
                    _subscribers.Add(fractionId, new List<Player>());
                if (!_subscribers[fractionId].Contains(player))
                    _subscribers[fractionId].Add(player);
            }
        }

        public static void UnSubscribe(Player player)
        {
            lock (_subscribers)
            {
                foreach (var subscribers in _subscribers.Values)
                {
                    if (subscribers.Contains(player))
                        subscribers.Remove(player);
                }
            }
        }

        public static void TriggerEventToSubscribers(int fractionId, string eventName, params object[] args)
        {
            if (_subscribers.ContainsKey(fractionId))
                Trigger.ClientEventToPlayers(_subscribers[fractionId].ToArray(), eventName, args);
        }
        public static void TriggerEventToSubscribers(List<int> fractionsId, string eventName, params object[] args)
        {
            foreach (var fractionId in fractionsId)
            {
                if (_subscribers.ContainsKey(fractionId))
                    Trigger.ClientEventToPlayers(_subscribers[fractionId].ToArray(), eventName, args);
            }
        }

        public static void TriggerCefEventSubscribers(int fractionId, string storeFunction, object data)
        {
            if (_subscribers.ContainsKey(fractionId))
                foreach (var subscriber in _subscribers[fractionId])
                    subscriber.TriggerCefEvent(storeFunction, data);
        }

        public static void TriggerCefEventSubscribers(List<int> fractionsId, string storeFunction, object data)
        {
            foreach (var fractionId in fractionsId)
            {
                if (_subscribers.ContainsKey(fractionId))
                    foreach (var subscriber in _subscribers[fractionId])
                        subscriber.TriggerCefEvent(storeFunction, data);
            }
        }
    }
}
