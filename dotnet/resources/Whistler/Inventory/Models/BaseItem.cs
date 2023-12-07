using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using Whistler.PlayerEffects;

namespace Whistler.Inventory.Models
{
    public abstract class BaseItem
    {
        private static int _lastItemId = 0;
        public static int NextItemId
        {
            get
            {
                if (_lastItemId == 0)
                {
                    _lastItemId = GameLog.GetItemLastId();
                }
                return ++_lastItemId;
            }
        }
        public BaseItem(ItemNames name, int count, bool promo, bool temporary)
        {
            if (!temporary)
                Id = NextItemId;
            Name = name;
            Count = count;
            Promo = promo;
            Index = -1;
        }
        public BaseItem()
        {
            if (Id == -1)
                Id = NextItemId;
        }

        public ItemNames Name { get; set; }
        public int Count { get; set; }
        public int Index { get; set; }
        public int EffectUsedOnCount { get; set; } = -1;

        public bool Promo { get; set; }
        public int Id { get; set; } = -1;

        [JsonIgnore]
        public ItemTypes Type { 
            get { 
                return Config.GetTypeByName(Name); 
            } 
        }

        public void CheckEffects(ExtPlayer player, List<List<EffectModel>> effects)
        {
            if (effects != null && EffectUsedOnCount != Count)
            {
                effects.ForEach(es =>
                {
                    EffectModel effect = es.Count > 1 ? effect = es[new Random().Next(0, es.Count)] : es[0];
                    NAPI.Task.Run(() => {
                        PlayerEffectsManager.AddEffect(player, effect.Name, effect.Time);
                    }, effect.Delay * 1000);
                });
                EffectUsedOnCount = Count;
            }
        }

        public abstract bool Use(ExtPlayer player);
        public abstract bool CanUse(ExtPlayer player);
        public abstract bool Equip(ExtPlayer player);
        public abstract int GetWeight();
        public abstract bool IsStackable();
        public abstract bool IsDisposable();
        public abstract Vector3 GetDropRotation();
        public abstract Vector3 GetDropOffset();
        public abstract uint GetModelHash();
        public abstract List<int> GetItemData();
        public abstract string GetItemLogData();
        public BaseItem SplitItem(int count) {
            if (!IsStackable()) return null;
            Count -= count;
            var newItem = ItemsFabric.CreateByName(Name);
            if (newItem == null) return null;
            newItem.Name = Name;
            newItem.Count = count;
            newItem.Index = -1;
            newItem.Promo = Promo;
            return newItem;
        }
    }
}
