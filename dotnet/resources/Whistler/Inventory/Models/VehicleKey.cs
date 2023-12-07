using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using Whistler.VehicleSystem;

namespace Whistler.Inventory.Models
{
    public class VehicleKey : BaseItem
    {
        public VehicleKey() : base() { }
        public VehicleKey(ItemNames name, int vehicleId, int keyNumber, bool promo, bool temporary) : base(name, 1, promo, temporary)
        {
            VehicleId = vehicleId;
            KeyNumber = keyNumber;
        }

        public int VehicleId { get; set; }
        public int KeyNumber { get; set; }
        [JsonIgnore]
        private string _vehicleNumber 
        { 
            get 
            {
                if (VehicleManager.Vehicles.ContainsKey(VehicleId))
                    return VehicleManager.Vehicles[VehicleId].Number.PadRight(8);
                else
                    return "Unknown ";
            }
        }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(VehicleKey));
        private static VehicleKeyConfig DefaultConfig(ItemNames name)
        {
            _logger.WriteWarning($"No config for: {name}");
            return new VehicleKeyConfig
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
                Type = ItemTypes.CarKey
            };

        }

        [JsonIgnore]
        public VehicleKeyConfig Config { get { return (!VehicleKeyConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : VehicleKeyConfigs.Config[Name]; } }

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
            var list = new List<int> { (int)Name, Count, Index, Promo ? 1 : 0 };
            list.AddRange(_vehicleNumber.ToList().Select(item => (int)item));
            return list;
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
            if (Config.CanUse)
                InventoryService.OnUseCarKey?.Invoke(player, this);
            else 
                return false;
            return true;
        }

        public override bool CanUse(ExtPlayer player)
        {
            return true;
        }

        public bool CheckTrueVehicle(int vehId, int keyNum)
        {
            return vehId == VehicleId && keyNum == KeyNumber;
        }
        public override string GetItemLogData()
        {
            return $"{VehicleId},{KeyNumber}" + (Promo ? ",prm" : "");
        }
    }
}
