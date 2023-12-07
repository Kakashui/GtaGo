using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.SDK;
using Newtonsoft.Json;
using Whistler.NewDonateShop.Configs;
using Whistler.Helpers;
using System.Linq;
using Whistler.Entities;

namespace Whistler.NewDonateShop.Models
{
    public class DonateInventoryModel
    {
        private List<DonateInventoryItemModel> _items;
        private List<ExtPlayer> _subscribes;
        private bool _changed = false;
        public int Id { get; set; }
        public DonateInventoryModel()
        {
            _items = new List<DonateInventoryItemModel>();
            _subscribes = new List<ExtPlayer>();
        }
        public DonateInventoryModel(string data)
        {
            _items = JsonConvert.DeserializeObject<List<DonateInventoryItemModel>>(data);
            _subscribes = new List<ExtPlayer>();
        }

        public void Subscribe(ExtPlayer player)
        {
            if (!_subscribes.Contains(player)) _subscribes.Add(player);
            UpdateItemDataForPlayer(player);
        }

        public void Unsubscribe(ExtPlayer player)
        {
            if (_subscribes.Contains(player)) _subscribes.Remove(player);
        }

        public void UseItem(ExtPlayer player, int id, bool sell, int count)
        {
            var i = _items.FirstOrDefault(i => i.Id == id & i.Sell == sell);
            if (i == null || i.Count < count)
            {
                UpdateItemDataForPlayer(player);
                return;
            }
            var item = DonateService.Items[id];
            if (item.Data.TryUse(player, count, sell))
            {
                RemoveItem(id, sell, count);
                DonateLog.DonateItemlog(player, item, "use");
            }
        }

        public void SellItem(ExtPlayer player, int id, bool sell)
        {
            var item = DonateService.Items[id];
            if (!RemoveItem(id, sell, item.Count))
            {
                UpdateItemDataForPlayer(player);
                return;
            }
            var price = RouletteConfig.RarityPrice[item.Rarity].Price;
            player.AddMCoins(price);
            DonateLog.DonateItemlog(player, item, price, "sell");
        }

        public void AddItem(int id, bool sell, bool update = true)
        {
            var i = _items.FirstOrDefault(i => i.Id == id && i.Sell == sell);
            var count = DonateService.Items[id].Count;
            if (i == null)
            {
                i = new DonateInventoryItemModel(id, count, sell);
                _items.Add(i);
            }
            else i.Count += count;
            _changed = true;
            if (update)
            {
                foreach (var player in _subscribes)
                {
                    SafeTrigger.ClientEvent(player,"dshop:inventory:update", i.Id, i.Count, i.Sell);
                }
            }
        }

        public bool RemoveItem(int id, bool sell, int count = 1)
        {
            var i = _items.FirstOrDefault(i => i.Id == id & i.Sell == sell);
            if (i == null || i.Count < count) return false;
            _changed = true;
            i.Count -= count;
            if (i.Count < 1)
            {
                _items.Remove(i);
                foreach (var player in _subscribes)
                {
                    SafeTrigger.ClientEvent(player,"dshop:inventory:update", i.Id, -1, i.Sell);
                }
            }
            else
            {
                foreach (var player in _subscribes)
                {
                    SafeTrigger.ClientEvent(player,"dshop:inventory:update",i.Id, i.Count, i.Sell);
                }
            }            
            return true;
        }

        public void UpdateItemDataForPlayer(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"dshop:inventory:set", GetItemData());
        }

        public void Save()
        {
            if (_changed)
            {
                MySQL.Query("UPDATE `donate_inventories` SET `items`=@prop0 WHERE `id`=@prop1", GetItemData(), Id);
                _changed = false;
            }
        }
               
        public string GetItemData()
        {
            return JsonConvert.SerializeObject(_items);
        }

        internal bool RemoveItems(Func<DonateInventoryItemModel, bool> predicate)
        {
            var items = _items.Where(predicate).ToList();
            if (items.Count > 0) _changed = true;
            foreach (var item in items)
                _items.Remove(item);
            return items.Count > 0;
        }
        public void Reset()
        {
            _items = new List<DonateInventoryItemModel>();
            _changed = true;
        }
    }
}
