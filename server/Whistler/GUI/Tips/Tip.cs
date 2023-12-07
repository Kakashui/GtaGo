using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Helpers;

namespace Whistler.GUI.Tips
{
    internal class Tip : Script
    {
        public Tip()
        {
            Main.OnPlayerReady += SendTipsInfo;
        }

        private static void SendTipsInfo(Player player)
        {
            player.TriggerEvent(JsonConvert.SerializeObject(player.GetCharacter().UsedTips));
        }

        public static void SendTip(Player player, string tip)
        {
            if (player.GetCharacter().UsedTips.Contains(tip)) return;
            
            player.GetCharacter().UsedTips.Add(tip);
            player.TriggerEventSafe("showTip", tip);
        }

        /// <summary>
        /// Отправляет подсказку над картой, не сохраняя при этом в базу данных.
        /// </summary>
        /// <param name="player"></param>
        public static void SendTipNotification(Player player, string tip)
        {
            player.TriggerEventSafe("tips:showTipNotification", tip);
        }

        [RemoteEvent("tipUsed")]
        public static void OnTipUsed(Player player, string tipName)
        {
            if (!player.GetCharacter().UsedTips.Contains(tipName))
                player.GetCharacter().UsedTips.Add(tipName);
        }
    }
}