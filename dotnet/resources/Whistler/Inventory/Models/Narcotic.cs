using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using Whistler.PlayerEffects;
using Whistler.SDK;

namespace Whistler.Inventory.Models
{
    public class Narcotic : BaseItem
    {
        public Narcotic() : base() { }
        public Narcotic(ItemNames name, int count, bool promo, bool temporary) : base(name, count, promo, temporary)
        {

        }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Narcotic));
        private static NarcoticConfig DefaultConfig(ItemNames name)
        {
            _logger.WriteWarning($"No config for: {name}");
            return new NarcoticConfig
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
                Type = ItemTypes.Narcotic
            };

        }

        [JsonIgnore]
        public NarcoticConfig Config { get { return (!NarcoticConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : NarcoticConfigs.Config[Name]; } }

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
            if (Name == ItemNames.Marijuana)
            {
                var inventory = player.GetInventory();
                if (!inventory.HasItem(ItemNames.Bong) || !inventory.HasItem(ItemNames.Lighter))
                {
                    Notify.Send(player, NotifyType.Info, NotifyPosition.Center, "scene:action:bong:no", 3000);
                    return false;
                }
            }
           
            if (!Config.CanUse)return false;
            InventoryService.OnUseNarcoticsItem?.Invoke(player, this);
            return true;
        }
        public override bool CanUse(ExtPlayer player)
        {
            if (!player.HasData("USE_NARC") || DateTime.Now > player.GetData<DateTime>("USE_NARC"))
            {
                SafeTrigger.SetData(player, "USE_NARC", DateTime.Now.AddSeconds(5));
                return true;
            }

            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_98", 3000);
            return false;
        }
        public override string GetItemLogData()
        {
            return Promo ? "prm" : "";
        }
    }
}
