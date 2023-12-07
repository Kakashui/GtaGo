using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class GasCan : BaseItem
    {
        public GasCan() : base() { }
        public GasCan(ItemNames name, int count, bool promo, bool temporary) : base(name, count, promo, temporary)
        {

        }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(GasCan));
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
                Type = ItemTypes.GasCan
                
            };

        }

        [JsonIgnore]
        public OtherConfig Config { get { return (!GasCanConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : GasCanConfigs.Config[Name]; } }

        public override bool Equip(ExtPlayer player)
        {
            return false;
        }

        public override List<int> GetItemData()
        {
            return new List<int> { (int)Name, Count, Index, Promo ? 1 : 0 };
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

            ExtVehicle playerVehicle = player.Vehicle as ExtVehicle;
            if (playerVehicle == null) return false;
            
            return playerVehicle.FillFuel(20) > 0;
        }

        public override Vector3 GetDropRotation()
        {
            return Config.DropRotation;
        }
        public override Vector3 GetDropOffset()
        {
            return Config.DropOffsetPosition;
        }

        public override uint GetModelHash()
        {
            return Config.ModelHash;
        }

        public override bool CanUse(ExtPlayer player)
        {
            if (player.IsInVehicle)
            {
                return true;
            }
            else
            {
                Notify.SendError(player, "i_gascan_0");
                return false;
            }
        }
        public override string GetItemLogData()
        {
            return Promo ? "prm" : "";
        }
    }
}
