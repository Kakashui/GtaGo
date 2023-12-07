using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using Whistler.GUI;
using Whistler.ClothesCustom;
using Whistler.Fractions.GOV.Models;
using Whistler.Helpers;
using Newtonsoft.Json;
using System.Linq;
using Whistler.Fractions.GOV.Config;
using Whistler.Entities;

namespace Whistler.Fractions.GOV
{
    class Cityhall : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Cityhall));
        public static int lastHourTax = 0;
        public static int canGetMoney = 999999;

        public static List<Vector3> CityhallChecksCoords = new List<Vector3>
        {
            new Vector3(-571.8821, -198.0799, 46.38985), // cloth gov
            new Vector3(231.0668, 214.6083, 104.8125), // bank
        };


        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStartHandler()
        {
            try
            {

                InteractShape.Create(CityhallChecksCoords[0], 1, 2)
                    .AddDefaultMarker()
                    .AddInteraction(beginWorkDay, "interact_3");


                NAPI.Object.CreateObject(0x4f97336b, new Vector3(260.651764, 203.230209, 106.432785), new Vector3(0, 0, 160.003571), 255, 0);
                NAPI.Object.CreateObject(0x4f97336b, new Vector3(258.209259, 204.120041, 106.432785), new Vector3(0, 0, -20.0684872), 255, 0);

                NAPI.Object.CreateObject(0x4f97336b, new Vector3(259.09613, 212.803894, 106.432793), new Vector3(0, 0, 70.0000153), 255, 0);
                NAPI.Object.CreateObject(0x4f97336b, new Vector3(259.985962, 215.246399, 106.432793), new Vector3(0, 0, -109.999962), 255, 0);
            } catch(Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"FRACTIONS_CITYHALL\":\n" + e.ToString());
            }
        }


        public static void beginWorkDay(ExtPlayer player)
        {
            if (player.Character.FractionID == 6)
                SkinManager.OpenSkinMenu(player);
            else 
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_41", 3000);
        }

    }
}
