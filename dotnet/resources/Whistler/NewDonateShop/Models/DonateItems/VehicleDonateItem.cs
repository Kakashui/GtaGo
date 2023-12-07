using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Common;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Houses;
using Whistler.VehicleSystem;

namespace Whistler.NewDonateShop.Models
{
    class VehicleDonateItem:BaseDonateItem
    {
        private static Color _defaultColor1 = new Color(255, 255, 255);
        private static Color _defaultColor2 = new Color(255, 255, 255);
        public VehicleDonateItem(string model)
        {
            Model = model;
            Color1 = _defaultColor1;
            Color2 = _defaultColor2;
        }
        public VehicleDonateItem(string model, Color color1, Color color2)
        {
            Model = model;
            Color1 = color1;
            Color2 = color2;
        }
        public string Model { get; set; }
        public Color Color1 { get; set; }
        public Color Color2 { get; set; }

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var character = player.Character;
            var vehData = VehicleManager.Create(character.UUID, Model, Color1, Color2, typeOwner: OwnerType.Personal, status: sell ? PropBuyStatus.Roulette : PropBuyStatus.Donate);
            GarageManager.SendVehicleIntoGarage(vehData);
            MainMenu.SendProperty(player);
            return true;
        }
    }
}
