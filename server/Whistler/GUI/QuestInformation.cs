using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.GUI
{
    public static class QuestInformation
    {
        public static void Show(Player player, string title, string subtitle)
        {
            player.TriggerEvent("questmsg:show", title, subtitle);
        }
        public static void Hide(Player player)
        {
            player.TriggerEvent("questmsg:hide");
        }

    }
}
