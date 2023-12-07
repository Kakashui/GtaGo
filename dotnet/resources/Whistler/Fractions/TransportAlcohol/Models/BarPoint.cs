using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.VehicleSystem.Models;

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
        public bool EnterPredicate(ColShape shape, ExtPlayer player)
        {
            if (!player.IsLogged())
                return false;
            if (player.Character.FractionID != 16)
            {
                return false;
            }
            if (player.Vehicle == null)
            {
                return false;
            }
            ExtVehicle extVehicle = player.Vehicle as ExtVehicle;
            if (!(extVehicle.Data is VehicleSystem.Models.VehiclesData.FractionVehicle))
            {
                return false;
            }
            var vehData = extVehicle.Data as VehicleSystem.Models.VehiclesData.FractionVehicle;
            return vehData.InAlcoholBox && vehData.targetId == Id;
        }
        public void TakeAlcoholBox(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            if (player.Character.FractionID != 16)
            {
                return;
            }
            if (player.Vehicle == null)
            {
                return;
            }
            ExtVehicle extVehicle = player.Vehicle as ExtVehicle;
            if (!(extVehicle.Data is VehicleSystem.Models.VehiclesData.FractionVehicle))
            {
                return;
            }
            var vehData = extVehicle.Data as VehicleSystem.Models.VehiclesData.FractionVehicle;
            if (!vehData.InAlcoholBox || vehData.targetId != Id)
            {
                return;
            }
            vehData.InAlcoholBox = false;
            vehData.targetId = 0;
            int money = vehData.ModelName.ToLower() == "gburrito" ? TransportManager.MoneyForAuto : TransportManager.MoneyForMoto;
            MoneySystem.Wallet.MoneyAdd(player.Character, money, "Доставка алкоголя");
            MoneySystem.Wallet.MoneyAdd(Manager.GetFraction(16), money, "Доставка алкоголя");
            player.DeleteClientMarker(1699);
        }
        public void Destroy()
        {
            Blip?.Delete();
            Shape?.Destroy();
        }
    }
}
