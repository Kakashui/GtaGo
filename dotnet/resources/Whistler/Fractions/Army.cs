using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using Whistler.GUI;
using System;
using Whistler.ClothesCustom;
using Whistler.VehicleSystem;
using Whistler.Helpers;
using Whistler.Entities;

namespace Whistler.Fractions
{
    class Army : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Army));
        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            try
            {
                NAPI.Blip.CreateBlip(404, new Vector3(2832.932, -663.3395, 0), 1, 69, "Лодки (Армия)", shortRange: true);
                NAPI.Blip.CreateBlip(455, new Vector3(3098.926, -4795.587, 2.184744), 1, 69, "Авианосец", shortRange: true);

                //NAPI.TextLabel.CreateTextLabel("~g~Benson Pain", new Vector3(-2347.958, 3268.936, 33.81076), 5f, 0.3f, 0, new Color(255, 255, 255), true, NAPI.GlobalDimension);

                InteractShape.Create(ArmyCheckpoints[1], 1, 2)
                    .AddDefaultMarker()
                    .AddInteraction(OpenArmyclothes, "interact_3");

                InteractShape.Create(ArmyCheckpoints[3], 1, 2)
                    .AddDefaultMarker()
                    .AddInteraction(UseLiftByPlayer, "interact_5");

                InteractShape.Create(ArmyCheckpoints[4], 1, 2)
                    .AddDefaultMarker()
                    .AddInteraction(UseLiftByPlayer, "interact_5");
            }
            catch (Exception e) { _logger.WriteError("ResourceStart: " + e.ToString()); }
        }


        private static Dictionary<int, ColShape> Cols = new Dictionary<int, ColShape>();
        public static List<Vector3> ArmyCheckpoints = new List<Vector3>()
        {
            new Vector3(-2345.839, 3268.359, 31.81075), // guns     0
            new Vector3(-2357.993, 3255.133, 31.81075), // dressing room    1
            new Vector3(-108.0619, -2414.873, 5.000005), // army docks mats     2
            new Vector3(-2360.946, 3249.595, 31.81075), // army lift 1 floor     3
            new Vector3(-2360.66, 3249.115, 91.90369), // army lift 9 floor     4
            new Vector3(-2349.892, 3266.55, 31.81075), // army stock    5
        };

        private static void OpenArmyclothes(ExtPlayer player)
        {
            if (player.Character.FractionID != 14)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_6", 3000);
                return;
            }
            SkinManager.OpenSkinMenu(player);
        }

        private static void UseLiftByPlayer(ExtPlayer player)
        {
            if (player.IsInVehicle) return;
            if (player.Character.Following != null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_14", 3000);
                return;
            }
            if (player.Position.Z > 50)
            {
                player.ChangePosition(new Vector3(ArmyCheckpoints[3].X, ArmyCheckpoints[3].Y, ArmyCheckpoints[3].Z + 1));
                Main.PlayerEnterInterior(player, new Vector3(ArmyCheckpoints[3].X, ArmyCheckpoints[3].Y, ArmyCheckpoints[3].Z + 1));
            }
            else
            {
                player.ChangePosition(new Vector3(ArmyCheckpoints[4].X, ArmyCheckpoints[4].Y, ArmyCheckpoints[4].Z + 1));
                Main.PlayerEnterInterior(player, new Vector3(ArmyCheckpoints[4].X, ArmyCheckpoints[4].Y, ArmyCheckpoints[4].Z + 1));
            }
        }
    }
}
