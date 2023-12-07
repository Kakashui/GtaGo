using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Common;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models;

namespace Whistler.Inventory.Models
{
    public class Other : BaseItem
    {
        public Other() : base() { }
        public Other(ItemNames name, int count, bool promo, bool temporary) : base(name, count, promo, temporary)
        {

        }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Other));
        private static OtherConfig DefaultConfig(ItemNames name)
        {
            _logger.WriteWarning($"No config for: {name}");
            return new OtherConfig
            {
                Weight = 5000,
                CanUse = true,
                Stackable = true,
                DisplayName = "bad_item",
                Disposable = true,
                DropOffsetPosition = new Vector3(),
                DropRotation = new Vector3(),
                Image = "bad_item",
                ModelHash = NAPI.Util.GetHashKey("prop_box_ammo07a"),
                Type = ItemTypes.Other
            };
        }

        [JsonIgnore]
        public OtherConfig Config { get { return (!OtherConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : OtherConfigs.Config[Name]; } }

        public override bool Equip(ExtPlayer player)
        {
            return false;
        }

        public override Vector3 GetDropRotation()
        {
            return Config.DropRotation;
        }
        public override Vector3 GetDropOffset()
        {
            return Config.DropOffsetPosition;
        }

        public override List<int> GetItemData()
        {
            return new List<int> { (int)Name, Count, Index, Promo ? 1 : 0 };
        }

        public override uint GetModelHash()
        {
            return Config.ModelHash;
        }

        public override int GetWeight()
        {
            return Config.Weight * Count;
        }

        public override bool IsStackable()
        {
            return Config.Stackable;
        }
        public override bool IsDisposable()
        {
            return Config.Disposable;
        }

        public override bool Use(ExtPlayer player)
        {
            if (Name == ItemNames.Cigarettes)
            {
                var inventory = player.GetInventory();
                if (!inventory.HasItem(ItemNames.Lighter))
                {
                    Notify.Send(player, NotifyType.Info, NotifyPosition.Center, "scene:action:ciga:no", 3000);
                    return false;
                }
            }
            if (Config.CanUse)
                InventoryService.OnUseOtherItem?.Invoke(player, this);
            else return false;
            return true;
        }

        public override bool CanUse(ExtPlayer player)
        {
            if (Name == ItemNames.ArmyLockpick)
            {
                if (!player.IsInVehicle || player.VehicleSeat != VehicleConstants.DriverSeat)
                    return false;

                ExtVehicle vehicle = player.Vehicle as ExtVehicle;
                if (vehicle.Data.OwnerType != OwnerType.Fraction || vehicle.Data.OwnerID != 14 || vehicle.Data.ModelName.ToLower() != "brickade" || vehicle.Engine || vehicle.Data.Fuel <= 0)
                    return false;
            }
            return true;
        }
        public override string GetItemLogData()
        {
            return Promo ? "prm" : "";
        }
    }
}
