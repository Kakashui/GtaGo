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
using Whistler.SDK;

namespace Whistler.Inventory.Models
{
    public class Medicaments : BaseItem
    {
        public Medicaments() : base() { }
        public Medicaments(ItemNames name, int count, bool promo, bool temporary) : base(name, count, promo, temporary)
        {

        }

        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Medicaments));
        private static MedicamentsConfig DefaultConfig(ItemNames name)
        {
            _logger.WriteWarning($"No config for: {name}");
            return new MedicamentsConfig
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
                Type = ItemTypes.Medicaments
            };

        }

        [JsonIgnore]
        public MedicamentsConfig Config { get { return (!MedicamentsConfigs.Config.ContainsKey(Name)) ? DefaultConfig(Name) : MedicamentsConfigs.Config[Name]; } }

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
            if (Config.CanUse)
            {
                InventoryService.OnUseMedicaments?.Invoke(player, this);
            }
            else return false;
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
            if (Name == ItemNames.Bandage)
            {
                DateTime now = DateTime.Now;
                if (player.Session.NextBandage <= now)
                {
                    player.Session.NextBandage = now.AddSeconds(30);
                    return true;
                }

                TimeSpan difference = player.Session.NextBandage - now;
                Notify.Send(player, NotifyType.Warning, NotifyPosition.Center, $"Вы сможете использовать бинт через {difference.Seconds} секунд.", 3000);
                return false;
            }
            if (Name != ItemNames.HealthKit) return true;
           
            if (!player.HasData("USE_MEDKIT") || DateTime.Now > player.GetData<DateTime>("USE_MEDKIT"))
            {
                SafeTrigger.SetData(player, "USE_MEDKIT", DateTime.Now.AddMinutes(5));
                return true;
            }
            else
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Core_98", 3000);
                return false;
            }
        }
        public override string GetItemLogData()
        {
            return Promo ? "prm" : "";
        }
    }
}
