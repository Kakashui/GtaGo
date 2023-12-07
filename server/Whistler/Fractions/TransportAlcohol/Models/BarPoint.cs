using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Helpers;

namespace Whistler.Fractions.TransportAlcohol.Models
{
    class BarPoint
    {
        public int Id { get; set; }
        public Vector3 Position { get; set; }
        public BarPoint(int id, Vector3 position)
        {
            Id = id;
            Position = position;
            CreateInteract();
        }


        private InteractShape Shape;
        private Blip Blip;
        public void CreateInteract()
        {
            Shape = InteractShape.Create(Position, 5, 2, 0)
                .AddEnterPredicate(EnterPredicate)
                .AddInteraction(TakeAlcoholBox, "Чтобы выгрузить алкоголь");
        }
        public bool EnterPredicate(ColShape shape, Player player)
        {
            if (!player.IsLogged())
                return false;
            if (player.GetCharacter().FractionID != 16)
            {
                return false;
            }
            if (player.Vehicle == null)
            {
                return false;
            }
            var vehGo = player.Vehicle.GetVehicleGo();
            if (!(vehGo.Data is VehicleSystem.Models.VehiclesData.FractionVehicle))
            {
                return false;
            }
            var vehData = vehGo.Data as VehicleSystem.Models.VehiclesData.FractionVehicle;
            return vehData.InAlcoholBox && vehData.targetId == Id;
        }
        public void TakeAlcoholBox(Player player)
        {
            if (!player.IsLogged())
                return;
            if (player.GetCharacter().FractionID != 16)
            {
                return;
            }
            if (player.Vehicle == null)
            {
                return;
            }
            var vehGo = player.Vehicle.GetVehicleGo();
            if (!(vehGo.Data is VehicleSystem.Models.VehiclesData.FractionVehicle))
            {
                return;
            }
            var vehData = vehGo.Data as VehicleSystem.Models.VehiclesData.FractionVehicle;
            if (!vehData.InAlcoholBox || vehData.targetId != Id)
            {
                return;
            }
            vehData.InAlcoholBox = false;
            vehData.targetId = 0;
            int money = vehData.ModelName.ToLower() == "gburrito" ? TransportManager.MoneyForAuto : TransportManager.MoneyForMoto;
            MoneySystem.Wallet.MoneyAdd(player.GetCharacter(), money, "Money_TakeAlco");
            MoneySystem.Wallet.MoneyAdd(Manager.GetFraction(16), money, "Money_TakeAlco");
            player.DeleteClientMarker(1699);
        }
        public void Destroy()
        {
            Blip?.Delete();
            Shape?.Destroy();
        }
    }
}
