using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Helpers;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Models;

namespace Whistler.Inventory
{

    public static class DropSystem
    {
        private static Dictionary<uint, List<WorldObject>> _items;
        private static Random _rand;
        private static int _range = 2;
        private static double _searchRange = _range * Math.Sqrt(2);
        private static List<Player> _subscribes = new List<Player>();
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(DropSystem));
        static DropSystem()
        {
            _rand = new Random();
            _items = new Dictionary<uint, List<WorldObject>>();
        }
        
        public static List<WorldObject> GetNearItems(this Player player)
        {
            if (!_items.ContainsKey(player.Dimension)) return new List<WorldObject>();            
            return _items[player.Dimension].Where(o => o.Position.DistanceTo(player.Position) < _searchRange).ToList();
        }

        public static void Subscribe(Player player)
        {
            if (!_subscribes.Contains(player)) _subscribes.Add(player);
            Update(player);
        }

       public static void Update(Player player)
       {
            player.TriggerEvent("inv:update:near", GetNearItemsDTO(player));
       }

        public static void Unsubscribe(Player player)
        {
            if (_subscribes.Contains(player)) _subscribes.Remove(player);
        }

        private static void UpdateItem(WorldObject obj)
        {
            foreach (var player in _subscribes)
            {
                if (player.Dimension != obj.Dimension) continue;
                if(player.Position.DistanceTo(obj.Position) < _range)
                {
                    WhistlerTask.Run(() => {
                        var item = obj.Item.GetItemData();
                        player.TriggerEvent("inv:update:item", 0, obj.Item.GetItemData()); 
                    });
                }
            }
        }

        private static void UpdateItem(int objId, uint dimension, Vector3 position)
        {
            foreach (var player in _subscribes)
            {
                if (player.Dimension != dimension) continue;
                if (player.Position.DistanceTo(position) < _range * 2)
                {
                    WhistlerTask.Run(() => { 
                        player.TriggerEvent("inv:update:item", 0, new List<int> { 0, 0, objId }); 
                    });
                }
            }
        }

        public static List<List<int>> GetNearItemsDTO(this Player player)
        {
            var nearItems = player.GetNearItems();
            var data = new List<List<int>>();
            if (player.IsInVehicle) return data;
            foreach (var obj in nearItems)
            {
                data.Add(obj.Item.GetItemData());
            }
            return data;
        }
        
        public static void DropItem(BaseItem item, Vector3 position, uint dimension, bool inRandomPosition = true)
        {
            if (item == null || position == null) return;
            if (item.Promo)
            {
                if (item is Backpack) InventoryService.DestroyInventory((item as Backpack).InventoryId);
            }
            else
            {
                if (!_items.ContainsKey(dimension)) _items.Add(dimension, new List<WorldObject>());
                Vector3 pos;
                if (inRandomPosition)
                    pos = new Vector3(position.X + _rand.NextDouble() * _range, position.Y + _rand.NextDouble() * _range, position.Z) - item.GetDropOffset();
                else
                    pos = position - item.GetDropOffset();
                var obj = new WorldObject(item, pos, item.GetDropRotation(), dimension); 
                _items[dimension].Add(obj);
                UpdateItem(obj);
            }
        }

        public static void CollectGarbage()
        {
            try
            {
                var _forDelete = new Dictionary<uint, List<WorldObject>>();
                lock (_items)
                {
                    foreach (var items in _items)
                    {
                        var list = new List<WorldObject>();
                        foreach (var obj in items.Value)
                        {
                            if (obj.CreationDate.AddMinutes(15) < DateTime.Now)
                            {
                                list.Add(obj);
                            }
                        }         
                        if(list.Count > 0) _forDelete.Add(items.Key, list);
                    }
                    foreach (var items in _forDelete)
                    {
                        foreach (var obj in items.Value)
                        {
                            UpdateItem(obj.Id, obj.Dimension, obj.Position);
                            obj.Destroy();
                            _items[items.Key].Remove(obj);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.WriteError($"CollectGarbage:\n{e}");
            }
        }

        public static BaseItem PickupItem(Player player, int itemId, int count = 1)
        {
            if (!_items.ContainsKey(player.Dimension)) return null;
            var obj = _items[player.Dimension].FirstOrDefault(i => i.Id == itemId);
            if (obj == null) return null;
            BaseItem item;
            if(obj.Item.Count <= count)
            {
                item = obj.Item;
                _items[player.Dimension].Remove(obj);
                UpdateItem(obj.Id, obj.Dimension, obj.Position);
                obj.Destroy();
            }
            else
            {
                item = obj.Item.SplitItem(count);
                UpdateItem(obj);
            }
            return item;
        }

        public static BaseItem GetItemLink(Player player, int itemId)
        {
            if (!_items.ContainsKey(player.Dimension)) return null;
            var obj = _items[player.Dimension].FirstOrDefault(i => i.Id == itemId);
            if (obj == null) return null;
            return obj.Item;
        }

        public static List<BaseItem> ClearItemsInDimension(uint dimension)
        {
            var result = new List<BaseItem>();
            if (!_items.ContainsKey(dimension)) return result;
            foreach (var obj in _items[dimension].ToList())
            {
                result.Add(obj.Item);
                _items[dimension].Remove(obj);
                UpdateItem(obj.Id, obj.Dimension, obj.Position);
                obj.Destroy();
            }
            return result;
        }
    }
}
