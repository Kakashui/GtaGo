using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.CustomSync.Attachments;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Models
{
    public class ItemBox : BaseItem
    {
        public ItemBox() : base() { }
        public ItemBox(ItemNames name, ItemNames storedName, int countStoredItem, int serial, bool promo, bool temporary) : base(name, 1, promo, temporary)
        {
            StoredName = storedName;
            CountStoredItem = countStoredItem;
            PinKey = new Random().Next(10000, 99999);
            Serial = serial;
        }

        public ItemNames StoredName { get; set; }
        public int CountStoredItem { get; set; }
        public int PinKey { get; set; }
        public int Serial { get; set; }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(ItemBox));
        private static ItemBoxConfig DefaultConfig(ItemNames name)
        {
            _logger.WriteWarning($"No config for: {name}");
            return new ItemBoxConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_box_ammo03a"),
                Type = ItemTypes.ItemBox,
                AttachId = AttachId.SupplyBox,
                Weight = 1000,
                DisplayName = "bad_item",
                Image = "bad_item",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.83),
                AvailableItem = ItemTypes.Weapon,

                Stackable = false,
                Disposable = true
            };
        }

        [JsonIgnore]
        public ItemBoxConfig Config { get { return (!ItemBoxConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : ItemBoxConfigs.Config[Name]; } }

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
            return new List<int> { (int)Name, Count, Index, Promo ? 1 : 0, (int)StoredName, CountStoredItem, Serial };
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
            return Config.Disposable || CountStoredItem <= 0;
        }
        public override bool Use(ExtPlayer player)
        {
            if (Config.CanUse)
                InventoryService.OnUseItemBox?.Invoke(player, this);
            else return false;
            return true;
        }

        public override bool CanUse(ExtPlayer player)
        {
            return true;
        }
        public override string GetItemLogData()
        {
            return $"{Serial},{(int)StoredName}" + (Promo ? ",prm" : "");
        }
    }
}
