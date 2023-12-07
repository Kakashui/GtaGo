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

namespace Whistler.Inventory.Models
{
    public class WateringCan : BaseItem
    {
        public int Water { get; set; }
        public WateringCan() : base() { }
        public WateringCan(ItemNames name, bool promo, bool temporary) : base(name, 1, promo, temporary)
        {
            Water = 0;
        }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Cage));
        private static WateringCanConfig DefaultConfig(ItemNames name)
        {
            _logger.WriteWarning($"Нет конфига для: {name}");
            return new WateringCanConfig
            {
                Weight = 5000,
                CanUse = true,
                Stackable = true,
                DisplayName = "bad_item",
                Disposable = false,
                DropOffsetPosition = new Vector3(),
                DropRotation = new Vector3(),
                Image = "bad_item",               
                ModelHash = NAPI.Util.GetHashKey("prop_box_ammo07a"),
                Type = ItemTypes.WateringCan
            };

        }

        [JsonIgnore]
        public WateringCanConfig Config { get { return (!WateringCanConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : WateringCanConfigs.Config[Name]; } }

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
            return new List<int> { (int)Name, Count, Index, Promo ? 1 : 0, Water, GetWeight() };
        }

        public override uint GetModelHash()
        {
            return Config.ModelHash;
        }

        public override int GetWeight()
        {
            return Config.Weight + Water;
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
                InventoryService.OnUseWateringCan?.Invoke(player, this);
            else return false;
            return true;
        }

        public override bool CanUse(ExtPlayer player)
        {
            return true;
        }

        public override string GetItemLogData()
        {
            return Promo ? "prm" : "";
        }

        public bool WateringSeed()
        {
            if (Water - 1000 >= 0)
            {
                Water -= 1000;
                return true;
            }
            return false;
        }

        public void PourWater()
        {
            Water = Config.MaxWater;
        }
    }
}