using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Configs;
using Whistler.Inventory.Models;
using Whistler.SDK;

namespace Whistler.Inventory
{

    public static class DropSystem
    {
        private static Dictionary<uint, List<WorldObject>> _items;
        private static Random _rand;
        private static int _range = 2;
        private static double _searchRange = _range * Math.Sqrt(2);
        private static List<ExtPlayer> _subscribes = new List<ExtPlayer>();
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(DropSystem));
        static DropSystem()
        {
            _rand = new Random();
            _items = new Dictionary<uint, List<WorldObject>>();
        }
        
        public static List<WorldObject> GetNearItems(this ExtPlayer player)
        {
            if (!_items.ContainsKey(player.Dimension)) return new List<WorldObject>();            
            return _items[player.Dimension].Where(o => o.Position.DistanceTo(player.Position) < _searchRange).ToList();
        }

        public static void Subscribe(ExtPlayer player)
        {
            if (player == null) return;

            if (!_subscribes.Contains(player)) _subscribes.Add(player);
            Update(player);
        }

       public static void Update(ExtPlayer player)
       {
            SafeTrigger.ClientEvent(player,"inv:update:near", GetNearItemsDTO(player));
       }

        public static void Unsubscribe(ExtPlayer player)
        {
            if (player == null) return;
            if (!_subscribes.Contains(player)) return;

            _subscribes.Remove(player);
        }

        private static void UpdateItem(WorldObject obj)
        {
            if (obj == null) return;
            if (obj.Item == null) return;

            NAPI.Task.Run(() =>
            {
                if (obj == null) return;
                if (obj.Item == null) return;

                int range = _range * 2;
                List<int> itemData = obj.Item.GetItemData();
                foreach (ExtPlayer player in _subscribes)
                {
                    if (player == null) continue;
                    if (player.Dimension != obj.Dimension) continue;
                    if (player.Position.DistanceTo(obj.Position) >= range) continue;

                    SafeTrigger.ClientEvent(player, "inv:update:item", 0, itemData);
                }
            });
        }

        private static void UpdateItem(int objId, uint dimension, Vector3 position)
        {
            NAPI.Task.Run(() =>
            {
                int range = _range * 2;
                List<int> dataList = new List<int> { 0, 0, objId };
                foreach (ExtPlayer player in _subscribes)
                {
                    if (player == null) continue;
                    if (player.Dimension != dimension) continue;
                    if (player.Position.DistanceTo(position) >= range) continue;

                    SafeTrigger.ClientEvent(player, "inv:update:item", 0, dataList);
                }
            });
        }

        public static List<List<int>> GetNearItemsDTO(this ExtPlayer player)
        {
            List<List<int>> data = new List<List<int>>();
            if (player == null) return data;

            List<WorldObject> nearItems = player.GetNearItems();
            if (player.IsInVehicle) return data;

            foreach (WorldObject obj in nearItems)
            {
                if (obj == null) continue;
                if (obj.Item == null) continue;

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
                Vector3 pos = !inRandomPosition ? position - item.GetDropOffset() : new Vector3(position.X + _rand.NextDouble() * _range, position.Y + _rand.NextDouble() * _range, position.Z) - item.GetDropOffset();
                WorldObject obj = new WorldObject(item, pos, item.GetDropRotation(), dimension); 
                _items[dimension].Add(obj);
                UpdateItem(obj);
            }
        }

        public static void CollectGarbage()
        {
            try
            {
                Dictionary<uint, List<WorldObject>> _forDelete = new Dictionary<uint, List<WorldObject>>();
                DateTime now = DateTime.Now;
                lock (_items)
                {
                    foreach (KeyValuePair<uint, List<WorldObject>> items in _items)
                    {
                        List<WorldObject> list = new List<WorldObject>();
                        foreach (WorldObject obj in items.Value)
                        {
                            if (obj.DeleteDate > now) continue;

                            list.Add(obj);
                        } 
                        if(!list.Any()) continue;
                        
                        _forDelete.Add(items.Key, list);
                    }
                }
                if (!_forDelete.Any()) return;

                NAPI.Task.Run(() =>
                {
                    foreach (KeyValuePair<uint, List<WorldObject>> items in _forDelete)
                    {
                        if (!items.Value.Any()) continue;

                        foreach (WorldObject obj in items.Value)
                        {
                            UpdateItem(obj.Id, obj.Dimension, obj.Position);
                            obj.Destroy();
                            _items[items.Key].Remove(obj);
                        }
                    }
                });
                _logger.WriteDebug("Garbage got collected.");
            }
            catch (Exception e)
            {
                _logger.WriteError($"CollectGarbage:\n{e}");
            }
        }

        public static BaseItem PickupItem(ExtPlayer player, int itemId, int count = 1)
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

        public static BaseItem GetItemLink(ExtPlayer player, int itemId)
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
