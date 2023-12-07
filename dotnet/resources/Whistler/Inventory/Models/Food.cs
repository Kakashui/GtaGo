using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Core.CustomSync;
using Whistler.Core.CustomSync.Attachments;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using Whistler.Scenes;
using Whistler.SDK;

namespace Whistler.Inventory.Models
{
    public class Food : BaseItem
    {
        public Food() : base() { }
        public Food(ItemNames name, int count, bool promo, bool temporary) : base(name, count, promo, temporary)
        {

        }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Food));
        private static FoodConfig DefaultConfig(ItemNames name)
        {
            _logger.WriteWarning($"No config for: {name}");
            return new FoodConfig
            {
                Weight = 5000,
                CanUse = true,
                Stackable = true,
                DisplayName = "bad_item",
                Disposable = true,
                DropOffsetPosition = new Vector3(),
                DropRotation = new Vector3(),
                Image = "bad_item",
                LifeActivity = new LifeActivityData
                {
                    Hp = 0,
                    HungerIncrease = 0,
                    ThirstIncrease = 0
                },
                ModelHash = NAPI.Util.GetHashKey("prop_box_ammo07a"),
                Type = ItemTypes.Food
            };
        }

        [JsonIgnore]
        public FoodConfig Config { get { return (!FoodConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : FoodConfigs.Config[Name]; } }

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
            if (!Config.CanUse) return false;           
            InventoryService.OnUseFoodItem?.Invoke(player, this);            
            return true;
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
            return Config.CanUse;
        }
        public override string GetItemLogData()
        {
            return Promo ? "prm" : "";
        }
    }
}
