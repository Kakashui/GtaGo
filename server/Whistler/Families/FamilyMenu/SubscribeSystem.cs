using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Helpers;

namespace Whistler.Families.FamilyMenu
{
    class SubscribeSystem 
    {
        private static Dictionary<int, List<Player>> _subscrabers = new Dictionary<int, List<Player>>();

        public static void SubscribeMember(Player player, int famId)
        {
            if (!_subscrabers.ContainsKey(famId))
                _subscrabers.Add(famId, new List<Player>());
            if (!_subscrabers[famId].Contains(player))
                _subscrabers[famId].Add(player);
        }
        public static void UnsubscribeMember(Player player, int famId)
        {
            if (!_subscrabers.ContainsKey(famId))
                return;
            if (_subscrabers[famId].Contains(player))
                _subscrabers[famId].Remove(player);
        }

        public static void TriggerEventToSubscribe(int famId, string eventName, params object[] args)
        {
            if (!_subscrabers.ContainsKey(famId))
                return;
            Trigger.ClientEventToPlayers(_subscrabers[famId].ToArray(), eventName, args);
        }

        public static void TriggerEventToSubscribeAllFamily(string eventName, params object[] args)
        {
            foreach (var _subscrabersFamily in _subscrabers)
            {
                Trigger.ClientEventToPlayers(_subscrabersFamily.Value.ToArray(), eventName, args);
            }
        }

        public static void TriggerCefEventToSubscribeAllFamily(string eventName, object data)
        {
            foreach (var _subscrabersFamily in _subscrabers)
            {
                foreach (var player in _subscrabersFamily.Value)
                {
                    player.TriggerCefEvent(eventName, data);
                }
            }
        }

        public static void TriggerCefEventToSubscribe(int famId, string eventName, object data)
        {
            if (!_subscrabers.ContainsKey(famId))
                return;
            foreach (var player in _subscrabers[famId])
            {
                player.TriggerCefEvent(eventName, data);
            }
        }
    }
}
