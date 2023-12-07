using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Whistler.SDK;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Configs.InventoryConfig;
using Whistler.Helpers;
using Whistler.Core;
using Whistler.Entities;

namespace Whistler.Inventory.Models
{
    public class InventoryModel
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(InventoryModel));
        private int _maxWeight = 40000;
        private int _size = 0;
        private bool _changed = false;
        private List<ExtPlayer> _subscribes;
        public int CurrentWeight
        {
            get
            {
                return Items.Sum(i => i == null ? 0 : i.GetWeight());
            }
        }
        public bool IsTemp;
        private InventoryTypes _type = InventoryTypes.General;

        public List<BaseItem> Items { get; private set; }
        public int MaxWeight { get { return _maxWeight; } }
        public int Size { get { return _size; } }
        public InventoryTypes Type { get { return _type; } }

        internal bool IsSubscribed(ExtPlayer player)
        {
            return _subscribes.Contains(player);
        }

        public int Id { get; set; }

        internal InventoryClass Class { get { return InventoryClasses.GetClass(_type); } }
        
        public InventoryModel(int id, int maxWeight, int size, string stringDataItems, InventoryTypes type)
        {
            Id = id;
            _maxWeight = maxWeight;
            _size = size;
            Items = JsonConvert.DeserializeObject<List<BaseItem>>(stringDataItems, InventoryService.JsonOption);            
            _subscribes = new List<ExtPlayer>();
            IsTemp = false;
            _type = type;
        }

        public InventoryModel(int maxWeight, int size, InventoryTypes type, bool temp = false)
        {
            _maxWeight = maxWeight;
            _size = size;
            Items = new List<BaseItem>();
            _subscribes = new List<ExtPlayer>();
            _type = type;
            if (temp) 
                Id = InventoryService.AddTemp(this);
            else 
                Id = InventoryService.AddNew(this);
            IsTemp = temp;
        }

        public void Subscribe(ExtPlayer player)
        {
            lock (_subscribes)
            {
                if (!_subscribes.Contains(player))
                {
                    _subscribes.Add(player);
                    player.TriggerEventSafe("inv:update", Id, GetInventoryData(), MaxWeight, Size);
                }
            }           
        }

        public void Unsubscribe(ExtPlayer player)
        {
            lock (_subscribes)
            {
                if (_subscribes.Contains(player)) _subscribes.Remove(player);
            }
        }

        public void Update(ExtPlayer player)
        {
            lock (_subscribes)
            {
                if (!_subscribes.Contains(player))
                    Subscribe(player);
                else
                    NAPI.Task.Run(() => SafeTrigger.ClientEvent(player,"inv:update", Id, GetInventoryData(), MaxWeight, Size));
            }           
        }
        public void Update()
        {
            NAPI.Task.Run(() =>
            {
                foreach (var player in _subscribes)
                {
                    Update(player);
                }
            });
        }

        private void UpdateItem(BaseItem item)
        {
            NAPI.Task.Run(() =>
            {
                foreach (var player in _subscribes)
                {
                    if (item == null) continue;

                    SafeTrigger.ClientEvent(player,"inv:update:item", Id, item.GetItemData());
                }
            });
           
        }

        private void UpdateItem(int index)
        {
            NAPI.Task.Run(() =>
            {
                foreach (var player in _subscribes)
                {
                    SafeTrigger.ClientEvent(player,"inv:update:item", Id, new List<int> { 0, 0, index });
                }
            });           
        }  

        public void UpdateItemData(int index)
        {
            var item = Items.FirstOrDefault(item => item.Index == index);
            if (item != null)
                UpdateItem(item);
        }   

        public void ChangeMaxWeight(int maxWeigth)
        {
            _maxWeight = maxWeigth;
            if (IsTemp) return;
            MySQL.Query("UPDATE `inventories` SET `maxWeight`=@prop0 WHERE `id`=@prop1", _maxWeight, Id);
        }

        public void ChangeSlotsCount(int slots)
        {
            _size = slots;
            if (IsTemp) return;
            MySQL.Query("UPDATE `inventories` SET `size`=@prop0 WHERE `id`=@prop1", _size, Id);
        }

        public void Save()
        {
            if (IsTemp || !_changed) return;
            MySQL.Query("UPDATE `inventories` SET `items`=@prop0 WHERE `id`=@prop1", JsonConvert.SerializeObject(Items, InventoryService.JsonOption), Id);
            _changed = false;
        }

        public bool AddItem(BaseItem item, int index = -1, LogAction log = LogAction.Create)
        {
            try
            {
                if (!Class.CanAddedItem(item)) return false;

                var weight = item.GetWeight();
                if (_maxWeight < weight + CurrentWeight) return false;

                if (index == -1)
                {
                    if (item.IsStackable())
                    {
                        var exists = Items.FirstOrDefault(i => i.Name == item.Name && i.Promo == item.Promo);
                        if (exists == null)
                        {
                            if (_size > Items.Count)
                            {
                                item.Index = GetNextFreeIndex();
                                if (item.Index == -1) return false;
                                Items.Add(item);
                                UpdateItem(item);
                            }
                            else
                                return false;
                        }
                        else
                        {
                            exists.Count += item.Count;
                            UpdateItem(exists);
                        }
                    }
                    else
                    {
                        if (Size > Items.Count)
                        {
                            item.Index = GetNextFreeIndex();
                            if (item.Index == -1) return false;
                            Items.Add(item);
                            UpdateItem(item);
                        }
                        else return false;
                    }
                }
                else
                {
                    if (index >= Size) return false;
                    var exists = Items.FirstOrDefault(i => i.Index == index && i.Promo == item.Promo);
                    if (exists == null)
                    {
                        if (Size > Items.Count)
                        {
                            item.Index = index;
                            Items.Add(item);
                            UpdateItem(item);
                        }
                        else return false;
                    }
                    else
                    {
                        if (exists.Name != item.Name || !exists.IsStackable()) return false;
                        exists.Count += item.Count;
                        UpdateItem(exists);
                    }
                }
                _changed = true;
                if (log > LogAction.None)
                    GameLog.ItemsLog(item.Id, "-1", $"i{Id}", item.Name, item.Count, item.GetItemLogData(), log);
                return true;
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
                return false;
            }
            
        }

        public bool CanAddItem(BaseItem item, int index = -1)
        {
            try
            {
                if (!Class.CanAddedItem(item))
                    return false;
                var weight = item.GetWeight();
                if (_maxWeight < weight + CurrentWeight) return false;

                if (index == -1)
                {
                    if (item.IsStackable())
                    {
                        var exists = Items.FirstOrDefault(i => i.Name == item.Name && item.Promo == i.Promo);
                        if (exists == null)
                        {
                            if (_size > Items.Count)
                            {
                                item.Index = GetNextFreeIndex();
                                if (item.Index == -1) return false;
                            }
                            else
                                return false;
                        }
                    }
                    else
                    {
                        if (Size > Items.Count)
                        {
                            item.Index = GetNextFreeIndex();
                            if (item.Index == -1) return false;
                        }
                        else return false;
                    }
                }
                else
                {
                    if (index >= Size) return false;
                    var exists = Items.FirstOrDefault(i => i.Index == index && item.Promo == i.Promo);
                    if (exists == null)
                    {
                        if (Size > Items.Count)
                        {
                            item.Index = index;
                        }
                        else return false;
                    }
                    else
                    {
                        if (exists.Name != item.Name || !exists.IsStackable()) return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
                return false;
            }
        }
        
        public void MarkAsChanged()
        {
            _changed = true;
        }

        private int GetNextFreeIndex()
        {
            for (int i = 0; i < Size; i++)
            {
                if (Items.Any(item => item.Index == i)) continue;
                else return i;
            }
            return -1;
        }

        public BaseItem SubItemByName(ItemNames name, int count, LogAction log = LogAction.Delete)
        {
            BaseItem newItem = null;
            var items = Items.Where(i => i.Name == name).ToList();
            _changed = true;
            foreach (var item in items)
            {
                if (item.IsStackable())
                {
                    if (item.Count > count)
                    {
                        if(newItem == null)
                        {
                            newItem = item.SplitItem(count);
                        }
                        else
                        {
                            newItem.Count += item.SplitItem(count).Count;
                        }
                        UpdateItem(item);
                        break;
                    }
                    else
                    {
                        Items.Remove(item);
                        UpdateItem(item.Index);
                        count -= item.Count;
                        item.Index = -1;
                        if (newItem == null)
                        {
                            newItem = item;
                        }
                        else
                        {
                            newItem.Count += item.Count;
                        }
                        if (count == 0) break;
                    }
                }
                else
                {
                    Items.Remove(item);
                    UpdateItem(item.Index);
                    item.Index = -1;
                    _changed = true;
                    newItem = item;
                    break;
                }
            }
            if (newItem != null && log > LogAction.None)
                GameLog.ItemsLog(newItem.Id, $"i{Id}", "-1", newItem.Name, newItem.Count, newItem.GetItemLogData(), log);
            return newItem;
        }

        public bool HasItem(ItemNames name)
        {
            return Items.Any(i => i.Name == name);
        }

        public void DeleteEmptyArmor()
        {
            foreach (var item in Items.Where(i => i.Name == ItemNames.BodyArmor && (i as Clothes).Armor < 1).ToList())
            {
                Items.Remove(item);
                _changed = true;
            }
        }

        public BaseItem SubItem(int index, int count, LogAction log = LogAction.Delete)
        {
            if (index < 0 || index >= Size) return null;
            var item = Items.FirstOrDefault(i => i.Index == index);
            if (item == null) return null;
            if (item.Count < count) return null;
            _changed = true;
            if (item.Count > count) {
                var newItem = item.SplitItem(count);
                UpdateItem(item);
                _changed = true;
                if (newItem != null && log > LogAction.None)
                    GameLog.ItemsLog(newItem.Id, $"i{Id}", "-1", newItem.Name, newItem.Count, newItem.GetItemLogData(), log);
                return newItem;
            }
            else
            {
                Items.Remove(item);
                UpdateItem(index);
                _changed = true;
                if (log > LogAction.None)
                    GameLog.ItemsLog(item.Id, $"i{Id}", "-1", item.Name, item.Count, item.GetItemLogData(), log);
                return item;
            }
        }

        public BaseItem SubItem(int index, LogAction log = LogAction.Delete)
        {
            if (index < 0 || index >= Size) return null;
            var item = Items.FirstOrDefault(i => i.Index == index);
            if (item == null) return null;
            _changed = true;
            Items.Remove(item);
            UpdateItem(index);
            _changed = true;
            if (log > LogAction.None)
                GameLog.ItemsLog(item.Id, $"i{Id}", "-1", item.Name, item.Count, item.GetItemLogData(), log);
            return item;
        }

        public BaseItem GetItemLink(int index)
        {
            if (index < 0 || index >= Size) return null;
            return Items.FirstOrDefault(i => i.Index == index);
        }
        public BaseItem GetItemLink(ItemNames name)
        {
            return Items.FirstOrDefault(i => i.Name == name);
        }
        public List<BaseItem> GetItemLinksByCondition(Func<BaseItem, bool> predicate)
        {
            return Items.Where(predicate).ToList();
        }
        public List<List<int>> GetInventoryData()
        {
            var data = new List<List<int>>();
            foreach (var item in Items)
                data.Add(item.GetItemData());
            return data;
        }

        public bool RemoveItems(Func<BaseItem, bool> predicate, LogAction log = LogAction.Delete, ExtPlayer player = null)
        {
            var items = Items.Where(predicate).ToList();
            if(items.Count > 0) _changed = true;
            foreach (var item in items)
            {
                Items.Remove(item);
                UpdateItem(item.Index);
                if (player != null)
                    DropSystem.DropItem(item, player.Position, player.Dimension);
                if (log > LogAction.None)
                    GameLog.ItemsLog(item.Id, $"i{Id}", player != null ? "0" : "-1", item.Name, item.Count, item.GetItemLogData(), log);
            }
            return items.Count > 0;
        }

        public void Reset()
        {
            Items = new List<BaseItem>();
            _changed = true;
        }
    }
}
