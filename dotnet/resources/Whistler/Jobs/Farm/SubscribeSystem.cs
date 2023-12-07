using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.Jobs.Farm
{
    class SubscribeSystem
    {
        private static Dictionary<int, List<ExtPlayer>> _subscribers = new Dictionary<int, List<ExtPlayer>>();

        public static void Subscribe(ExtPlayer player, int farmId)
        {
            if (!_subscribers.ContainsKey(farmId))
                _subscribers.Add(farmId, new List<ExtPlayer>());
            if (!_subscribers[farmId].Contains(player))
                _subscribers[farmId].Add(player);
        }

        public static void UnSubscribe(ExtPlayer player, int id)
        {
            if (!_subscribers.ContainsKey(id))
                return;
            if (_subscribers[id].Contains(player))
                _subscribers[id].Remove(player);
        }

        public static bool IsSubscribe(ExtPlayer player, int farmId)
        {
            if (!_subscribers.ContainsKey(farmId))
                return false;
            return _subscribers[farmId].Contains(player);
        }

        public static int GetSubscribe(ExtPlayer player)
        {
            foreach (var farmSubscribers in _subscribers)
            {
                if (farmSubscribers.Value.Contains(player))
                    return farmSubscribers.Key;
            }
            return -1;
        }

        public static void TriggerEventToSubscribers(int farmId, string eventName, params object[] args)
        {
            if (_subscribers.ContainsKey(farmId))
                SafeTrigger.ClientEventToPlayers(_subscribers[farmId].ToArray(), eventName, args);
        }
    }
}
