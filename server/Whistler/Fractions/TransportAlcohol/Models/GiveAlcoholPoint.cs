using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Helpers;

namespace Whistler.Fractions.TransportAlcohol.Models
{
    class GiveAlcoholPoint
    {
        public Vector3 Position { get; set; }
        public GiveAlcoholPoint(Vector3 position)
        {
            Position = position;
            CreateInteract();
        }
        private InteractShape Shape;
        private Blip Blip;

        public void CreateInteract()
        {
            Shape = InteractShape.Create(Position, 3, 2, 0)
                .AddMarker(27, Position, 3, InteractShape.DefaultMarkerColor)
                .AddInteraction(GetAlcoholBox, "frac:transp:alco_2");
        }
        public void GetAlcoholBox(Player player)
        {
            if (!player.IsLogged())
                return;
            if (player.GetCharacter().FractionID != 16)
            {
                SDK.Notify.SendError(player, "frac:transp:alco_3");
                return;
            }
            if (player.Vehicle == null)
            {
                SDK.Notify.SendError(player, "frac:transp:alco_4");
                return;
            }
            var vehGo = player.Vehicle.GetVehicleGo();
            if (!(vehGo.Data is Whistler.VehicleSystem.Models.VehiclesData.FractionVehicle))
            {
                SDK.Notify.SendError(player, "frac:transp:alco_5");
                return;
            }
            var vehData = vehGo.Data as VehicleSystem.Models.VehiclesData.FractionVehicle;
            if (vehData.InAlcoholBox)
            {
                SDK.Notify.SendError(player, "frac:transp:alco_6");
                return;
            }
            vehData.InAlcoholBox = true;
            vehData.targetId = TransportManager._bars.Keys.GetRandomElement();
            TransportManager.GetWaipoint(player, vehData.targetId);
        }

        public void Destroy()
        {
            Blip?.Delete();
            Shape?.Destroy();
        }
    }
}
