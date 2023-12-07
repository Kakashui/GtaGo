using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.GUI.Tips
{
    internal class Tip : Script
    {
        public Tip()
        {
            Main.OnPlayerReady += SendTipsInfo;
        }

        private static void SendTipsInfo(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,JsonConvert.SerializeObject(player.Character.UsedTips));
        }

        public static void SendTip(ExtPlayer player, string tip)
        {
            if (player.Character.UsedTips.Contains(tip)) return;
            
            player.Character.UsedTips.Add(tip);
            player.TriggerEventSafe("showTip", tip);
        }

        /// <summary>
        /// Отправляет подсказку над картой, не сохраняя при этом в базу данных.
        /// </summary>
        /// <param name="player"></param>
        public static void SendTipNotification(ExtPlayer player, string tip)
        {
            player.TriggerEventSafe("tips:showTipNotification", tip);
        }

        [RemoteEvent("tipUsed")]
        public static void OnTipUsed(ExtPlayer player, string tipName)
        {
            if (!player.Character.UsedTips.Contains(tipName))
                player.Character.UsedTips.Add(tipName);
        }
    }
}