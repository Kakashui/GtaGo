using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Helpers;

namespace Whistler.Fractions.TransportAlcohol.Models
{
    class TakePoint
    {
        public Vector3 Position { get; set; }
        public TakePoint(Vector3 position)
        {
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
            var character = player.GetCharacter();
            if (character == null)
                return false;
            if (character.FractionID == 16 || (character.FractionID == 0 && character.FamilyID <= 0))
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
            return vehData.InAlcoholBox;
        }
        public void TakeAlcoholBox(Player player)
        {
            var character = player.GetCharacter();
            if (character == null)
                return;
            if (character.FractionID == 16 || (character.FractionID == 0 && character.FamilyID <= 0))
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
            if (!vehData.InAlcoholBox)
            {
                return;
            }
            vehData.InAlcoholBox = false;
            vehData.targetId = 0;
            int money = (vehData.ModelName.ToLower() == "gburrito" ? TransportManager.MoneyForAuto : TransportManager.MoneyForMoto) / 2;
            MoneySystem.Wallet.MoneyAdd(player.GetCharacter(), money, "Money_TakeAlco");
            if (character.FractionID > 0)
                MoneySystem.Wallet.MoneyAdd(player.GetFraction(), money, "Money_TakeAlco");
            else
                MoneySystem.Wallet.MoneyAdd(player.GetFamily(), money, "Money_TakeAlco");
            player.DeleteClientMarker(1699);
        }
        public void Destroy()
        {
            Blip?.Delete();
            Shape?.Destroy();
        }
    }
}
