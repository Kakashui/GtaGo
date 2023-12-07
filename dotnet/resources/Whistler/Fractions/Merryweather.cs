using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using Whistler.GUI;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using Whistler.Helpers;
using Whistler.Entities;

namespace Whistler.Fractions
{
    class Merryweather : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Merryweather));

        private static Dictionary<int, ColShape> Cols = new Dictionary<int, ColShape>();
        public static List<Vector3> Coords = new List<Vector3>
        {
            new Vector3(-442.969, 6016.779, 30.59221), // Колшэйп входа в бункер
            new Vector3(2154.641, 2921.034, -62.82243), // Колшэйп изнутри интерьера для телепорта наверх
            new Vector3(2033.842, 2942.104, -62.82434), // Колшэйп входа на другой этаж
            new Vector3(2155.425, 2921.066, -81.99551), // Колшэйп изнутри этажа, чтобы вернуться назад
        };

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStartHandler()
        {
            try
            {
                //InteractShape.Create(Coords[0], 1, 2)
                //    .AddDefaultMarker()
                //    .AddInteraction((player) =>
                //    {
                //        interactPressed(player, 82);
                //    });
                //InteractShape.Create(Coords[1], 1, 2)
                //    .AddDefaultMarker()
                //    .AddInteraction((player) =>
                //    {
                //        interactPressed(player, 83);
                //    });

                //InteractShape.Create(Coords[2], 1, 2)
                //    .AddDefaultMarker()
                //    .AddInteraction((player) =>
                //    {
                //        interactPressed(player, 84);
                //    });

                //InteractShape.Create(Coords[3], 1, 2)
                //    .AddDefaultMarker()
                //    .AddInteraction((player) =>
                //    {
                //        interactPressed(player, 85);
                //    });
            } catch(Exception e)
            {
                _logger.WriteError("EXCEPTION AT\"FRACTIONS_MERRYWEATHER\":\n" + e.ToString());
            }
        }

        private static void interactPressed(ExtPlayer player, int interact)
        {
            switch (interact)
            {
                case 82:
                case 83:
                case 84:
                case 85:
                    if (player.IsInVehicle) return;
                    if (player.Character.Following != null)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_453", 3000);
                        return;
                    }
                    if(player.Character.FractionID != 17)
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "Frac_454", 3000);
                        return;
                    }
                    if(interact == 82) player.ChangePosition(Coords[1] + new Vector3(0, 0, 1.12));
                    else if(interact == 83) player.ChangePosition(Coords[0] + new Vector3(0, 0, 1.12));
                    else if(interact == 84) player.ChangePosition(Coords[3] + new Vector3(0, 0, 1.12));
                    else if(interact == 85) player.ChangePosition(Coords[2] + new Vector3(0, 0, 1.12));
                    return;
            }
        }
    }
}
