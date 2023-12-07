using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.GUI
{
    public static class QuestInformation
    {
        public static void Show(ExtPlayer player, string title, string subtitle)
        {
            SafeTrigger.ClientEvent(player,"questmsg:show", title, subtitle);
        }
        public static void Hide(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"questmsg:hide");
        }

    }
}
