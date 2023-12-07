using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Whistler.ClothesCustom;
using Whistler.Core;
using Whistler.Customization.Models;
using Whistler.Helpers;
using Whistler.Inventory.Configs.ClothesDetails;
using Whistler.Inventory.Enums;
using Whistler.SDK;
using Whistler.Entities;

namespace Whistler.Inventory.Models
{
    public class Equip
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(Equip), true);
        public int Id { get; set; }
        public Dictionary<ClothesSlots, ClothesBase> Clothes { get; set; }
        public Dictionary<WeaponSlots, Weapon> Weapons { get; set; }
        public WeaponSlots CurrentWeapon { get; set; } = WeaponSlots.Invalid;
        public WeaponSlots LastWeapon { get; set; } = WeaponSlots.Invalid;
        public DateTime LastSwap { get; set; } = DateTime.Now;
        private ExtPlayer _owner;
        private bool _changed = false;
        public bool IsTemp;

        public Equip(int id, string clothes, string weapons)
        {
            Id = id;
            Clothes = JsonConvert.DeserializeObject<Dictionary<ClothesSlots, ClothesBase>>(clothes, InventoryService.JsonOption);
            if (Clothes.Count == 0)
                Clothes = GetDefaultClothes();
            else if (!Clothes.ContainsKey(ClothesSlots.Costume))
                Clothes.Add(ClothesSlots.Costume, null);
            Weapons = JsonConvert.DeserializeObject<Dictionary<WeaponSlots, Weapon>>(weapons, InventoryService.JsonOption);
            if (Weapons.Count == 0)
                Weapons = GetDefaultWeapons();
        }
        public Equip(ClothesDTO clothes, bool gender)
        {
            Clothes = GetDefaultClothes();
            Weapons = GetDefaultWeapons();
            Clothes[ClothesSlots.Leg] = ItemsFabric.CreateClothes(ItemNames.Leg, gender, clothes.Pants, 0, true);
            Clothes[ClothesSlots.Top] = ItemsFabric.CreateClothes(ItemNames.Top, gender, clothes.Shirt, 0, true);
            Clothes[ClothesSlots.Feet] = ItemsFabric.CreateClothes(ItemNames.Feet, gender, clothes.Shoes, 0, true);
            Id = EquipService.AddNew(this);
        }

        public Equip(bool temp = false)
        {
            Clothes = GetDefaultClothes();            
            Weapons = GetDefaultWeapons();
            IsTemp = temp;
            if (IsTemp)
                Id = EquipService.AddTemp(this);
            else
                Id = EquipService.AddNew(this);
        }

        private static Dictionary<ClothesSlots, ClothesBase> GetDefaultClothes()
        {
            return new Dictionary<ClothesSlots, ClothesBase>
            {
                { ClothesSlots.Mask, null },
                { ClothesSlots.Hat, null },
                { ClothesSlots.Glasses, null },
                { ClothesSlots.Ear, null },
                { ClothesSlots.Accessories, null },
                { ClothesSlots.Top, null },
                { ClothesSlots.Shirt, null },
                { ClothesSlots.BodyArmor, null },
                { ClothesSlots.Gloves, null },
                { ClothesSlots.Bracelets, null },
                { ClothesSlots.Watches, null },
                { ClothesSlots.Leg, null },
                { ClothesSlots.Feet, null },
                { ClothesSlots.Bag, null },
                { ClothesSlots.Costume, null },
            };
        }
        private static Dictionary<WeaponSlots, Weapon> GetDefaultWeapons()
        {
            return new Dictionary<WeaponSlots, Weapon>
            {
                { WeaponSlots.Weapon1, null },
                { WeaponSlots.Weapon2, null },
                { WeaponSlots.Weapon3, null },
                { WeaponSlots.Weapon4, null },
            };
        }

        public void CorrectClothes(ExtPlayer player)
        {
            var gender = player.GetGender();
            var top = Clothes[ClothesSlots.Top];
            var shirt = Clothes[ClothesSlots.Shirt];

            if (top != null)
            {
                int under, torso;
                if (shirt != null)
                    under = Shirts.GetFixedDrawable(gender, shirt.Drawable, Tops.GetType(gender, top.Drawable));
                else
                    under = InventoryService.DefaultClothes[ClothesSlots.Shirt].GetDrawable(gender);
                torso = UnderTorsos.GetTorso(player, under);
                if (torso < 0) torso = Torsos.GetTorso(player, top.Drawable);
                var gloveClolor = Clothes[ClothesSlots.Gloves] == null ? 0 : Clothes[ClothesSlots.Gloves].Texture;
                player.SetWhistlerClothes(3, torso, gloveClolor);
                player.SetWhistlerClothes(8, under, shirt == null ? 0 : shirt.Texture);
                player.SetWhistlerClothes(11, top.Drawable, top.Texture);
            }   
            else {
                int torso; 
                var topD = shirt == null ? InventoryService.DefaultClothes[ClothesSlots.Top].GetDrawable(gender) : shirt.Drawable;
                var topT = shirt == null ? InventoryService.DefaultClothes[ClothesSlots.Top].GetTexture(gender) : shirt.Texture;
                torso = UnderTorsos.GetTorso(player, topD); 
                if (torso < 0) torso = Torsos.GetTorso(player, topD);
               
                var gloveClolor = Clothes[ClothesSlots.Gloves] == null ? 0 : Clothes[ClothesSlots.Gloves].Texture;
                player.SetWhistlerClothes(3, torso, gloveClolor);
                player.SetWhistlerClothes(8, InventoryService.DefaultClothes[ClothesSlots.Shirt].GetDrawable(gender), InventoryService.DefaultClothes[ClothesSlots.Shirt].GetTexture(gender));
                player.SetWhistlerClothes(11, topD, topT);
            }
        }

        public void CorrectArmor(ExtPlayer player)
        {
            if (Clothes[ClothesSlots.BodyArmor] != null)
            {
                var gender = player.GetGender();
                int variationCycle = (gender) ? 16 : 18;
                if (Clothes[ClothesSlots.Costume] != null)
                {
                    (Clothes[ClothesSlots.Costume] as Costume)?.SetArmor(player, Clothes[ClothesSlots.BodyArmor], variationCycle);
                }
                else
                {
                    /*if (Clothes[ClothesSlots.Top] != null)
                    {
                        var top = OldCustomization.Tops[gender].Find(t => t.Variation == Clothes[ClothesSlots.Top].Drawable);

                        int type = 0;
                        if (top != null)
                        {
                            type = top.BodyArmor;
                        }
                        player.SetWhistlerClothes(9, (Clothes[ClothesSlots.BodyArmor].Drawable * variationCycle) + type, Clothes[ClothesSlots.BodyArmor].Texture);

                    }
                    else if (Clothes[ClothesSlots.Shirt] != null)
                    {
                        int type = 0;
                        var shirt = OldCustomization.Underwears[gender].Values.FirstOrDefault(pair => pair.Top == Clothes[ClothesSlots.Shirt].Drawable);
                        if (shirt != null)
                        {
                            type = shirt.BodyArmor;
                        }
                        player.SetWhistlerClothes(9, (Clothes[ClothesSlots.BodyArmor].Drawable * variationCycle) + type, Clothes[ClothesSlots.BodyArmor].Texture);
                    }
                    else*/
                    player.SetWhistlerClothes(9, (Clothes[ClothesSlots.BodyArmor].Drawable * variationCycle) + 1, Clothes[ClothesSlots.BodyArmor].Texture);
                }
            }
            else
            {
                player.SetWhistlerClothes(9, 0, 0);
            }
        }

        public List<List<List<int>>> GetEquipData()
        {
            var data = new List<List<List<int>>>();
            var clothes = new List<List<int>>();
            foreach (var item in Clothes)
            {
                var cl = new List<int>();
                cl.Add((int)item.Key);
                cl.Add(item.Value == null ? -1 : (int)item.Value.Name);
                clothes.Add(cl);
            }
            data.Add(clothes);
            var weapons = new List<List<int>>();
            foreach (var weapon in Weapons)
            {
                var wep = new List<int>();
                wep.Add((int)weapon.Key);
                wep.Add(weapon.Value == null ? -1 : (int)weapon.Value.Name);
                weapons.Add(wep);
            }
            data.Add(weapons);  
            return data;
        }

        public bool IsCurrent(Weapon weapon)
        {
            if (CurrentWeapon == WeaponSlots.Invalid) return false;
            return Weapons[CurrentWeapon] == weapon;
        }

        public bool IsCurrent(ItemNames weapon)
        {
            if (CurrentWeapon == WeaponSlots.Invalid) return false;
            return Weapons[CurrentWeapon].Name == weapon;
        }

        public void Update(bool updateClient)
        {
            if (_owner == null) return;
            for (int i = 0; i <= 8; i++) _owner.ClearAccessory(i);            
            foreach (var cloth in Clothes)
            {
                ClothesSlots slot = cloth.Key;
                ClothesBase item = cloth.Value;
                if (item == null)
                {
                    InventoryService.DefaultClothes[slot].Set(_owner);
                    continue;
                }
                item.Equip(_owner);
            }
            _owner.RemoveAllWeapons();
            if (_owner.HasSharedData("weapon:current"))
                _owner.ResetSharedData("weapon:current");
            CurrentWeapon = WeaponSlots.Invalid;
            foreach (var weapon in Weapons)
            {
                if (weapon.Value == null)
                {
                    if (false && _owner.HasSharedData($"attach:weapon:{(int)weapon.Key}") && weapon.Key != WeaponSlots.Weapon3 && weapon.Key != WeaponSlots.Weapon4) 
                        _owner.ResetSharedData($"attach:weapon:{(int)weapon.Key}");
                }else if (false && weapon.Key != WeaponSlots.Weapon3 && weapon.Key != WeaponSlots.Weapon4) 
                    _owner.SetSharedData($"attach:weapon:{(int)weapon.Key}", weapon.Value.GetWeaponData());
            }
            if(updateClient) Sync();
        }

        public void Sync()
        {
            if (_owner == null) return;
            NAPI.Task.Run(() =>
            {
                SafeTrigger.ClientEvent(_owner, "inv:clear:equip");
                foreach (var cloth in Clothes)
                {
                    if (cloth.Value != null)
                    {
                        SafeTrigger.ClientEvent(_owner, "inv:update:equip", (int)ItemTypes.Clothes, (int)cloth.Key, cloth.Value.GetItemData());
                    }
                }

                foreach (var weapon in Weapons)
                {
                    if (weapon.Value != null)
                    {
                        SafeTrigger.ClientEvent(_owner, "inv:update:equip", (int)ItemTypes.Weapon, (int)weapon.Key, weapon.Value.GetItemData());
                    }
                }
            }, 1000);
        }

        public void Update(ClothesSlots slot)
        {
            if (slot == ClothesSlots.Invalid) return;

            ClothesBase clothes = Clothes[slot];
            NAPI.Task.Run(() =>
            {              
                if (clothes == null) InventoryService.DefaultClothes[slot].Set(_owner);
                else clothes.Equip(_owner);
                SafeTrigger.ClientEvent(_owner, "inv:update:equip", (int)ItemTypes.Clothes, (int)slot, clothes?.GetItemData());
            });
        }

        public void Save()
        {
            try
            {
                if (IsTemp || !_changed) return;
                MySQL.Query("UPDATE `equips` SET `weapons`=@prop0, `clothes`=@prop1 WHERE `id`=@prop2",
                    JsonConvert.SerializeObject(Weapons, InventoryService.JsonOption),
                    JsonConvert.SerializeObject(Clothes, InventoryService.JsonOption),
                    Id
                );
            }
            catch (Exception ex)
            {
                _logger.WriteError($"Error at {nameof(Equip)} ex: {ex}");
            }
        }

        public void Update(WeaponSlots slot)
        {
            if (slot == WeaponSlots.Invalid) return;
            var weapon = Weapons[slot];
            NAPI.Task.Run(() =>
            {
                if (weapon == null)
                {
                    if (CurrentWeapon == slot)
                    {
                        CurrentWeapon = WeaponSlots.Invalid;
                        _owner.RemoveAllWeapons();
                        if (_owner.HasSharedData("weapon:current")) _owner.ResetSharedData("weapon:current");
                    }
                    else if (false && _owner.HasSharedData($"attach:weapon:{(int)slot}") && slot != WeaponSlots.Weapon3 && slot != WeaponSlots.Weapon4) _owner.ResetSharedData($"attach:weapon:{(int)slot}");
                }
                else
                {
                    if (CurrentWeapon == slot)
                    {
                        int ammo = weapon.Config.AmmoType == ItemNames.ThrowablesAmmo ? 1 : 0;
                        _owner.GiveWeapon((WeaponHash)weapon.Config.WeaponHash, ammo);
                        _owner.SetSharedData("weapon:current", weapon.GetWeaponData());
                    }
                    else if (false && slot != WeaponSlots.Weapon3 && slot != WeaponSlots.Weapon4) _owner.SetSharedData($"attach:weapon:{(int)slot}", weapon.GetWeaponData());
                }
                SafeTrigger.ClientEvent(_owner, "inv:update:equip", (int)ItemTypes.Weapon, (int)slot, weapon?.GetItemData());
            });
        }

        internal void DeleteEmptyArmor(ExtPlayer player = null)
        {
            if (Clothes[ClothesSlots.BodyArmor] != null && Clothes[ClothesSlots.BodyArmor].Armor < 1)
            {
                if (player != null) 
                    RemoveItem(player, ClothesSlots.BodyArmor);
                else
                    Clothes[ClothesSlots.BodyArmor] = null;
            }
        }

        public void ReloadWeapon(ExtPlayer player, int ammo)
        {
            if (CurrentWeapon == WeaponSlots.Invalid || !Weapons.ContainsKey(CurrentWeapon) || Weapons[CurrentWeapon] == null)
            {
                Update(true);
                return;
            }
            if (_owner == null || Weapons[CurrentWeapon].Ammo < ammo || Weapons[CurrentWeapon].MaxAmmo == ammo) return;

            InventoryModel inventory = _owner.GetInventory();
            if (inventory == null) return;

            Configs.Models.WeaponConfig config = Weapons[CurrentWeapon].Config;
            BaseItem item = inventory.SubItemByName(Weapons[CurrentWeapon].Config.AmmoType, Weapons[CurrentWeapon].MaxAmmo - ammo, LogAction.None);
            Weapons[CurrentWeapon].Ammo = item == null ? ammo : item.Count + ammo;
            _changed = true;

            item = inventory.GetItemLink(config.AmmoType);
            int count = item == null ? 0 : item.Count;
            SafeTrigger.ClientEvent(player,"weapon:reload", Weapons[CurrentWeapon].Ammo, count);
        }
                
        //"attach:weapon:1", "attach:weapon:2", "attach:weapon:3", "attach:weapon:4", "weapon:current"

        public void SetActiveItem(ExtPlayer player, WeaponSlots slotId, int ammo)
        {
            if (slotId == WeaponSlots.Invalid) return;
            if (Weapons == null || !Weapons.ContainsKey(slotId) || Weapons[slotId] == null) return;

            if (CurrentWeapon == WeaponSlots.Invalid)
            {
                Weapon weapon = Weapons[slotId];
                Configs.Models.WeaponConfig config = weapon.Config;
                int ammos = weapon.Config.AmmoType == ItemNames.ThrowablesAmmo ? 1 : 0;
                player.GiveWeapon((WeaponHash)weapon.Config.WeaponHash, ammos);
                SafeTrigger.SetSharedData(player, $"weapon:current", weapon.GetWeaponData()); 
                if(false && slotId != WeaponSlots.Weapon3 && slotId != WeaponSlots.Weapon4) SafeTrigger.ResetSharedData(player, $"attach:weapon:{(int)slotId}");
                CurrentWeapon = slotId;
                InventoryModel inventory = player.GetInventory();
                int ammoCount = ammos;
                if (inventory != null) 
                {
                    if (Weapons.ContainsKey(CurrentWeapon) && config.AmmoType != ItemNames.Invalid)
                    {
                        BaseItem item = inventory.GetItemLink(config.AmmoType);
                        if (item != null) ammoCount = item.Count;
                    }
                }
                SafeTrigger.ClientEvent(player,"weapon:setmammo", ammoCount);
            }
            else
            {
                LastWeapon = CurrentWeapon;
                LastSwap = DateTime.Now;
                if (!Weapons.ContainsKey(CurrentWeapon)) return;

                if (Weapons[CurrentWeapon] != null) Weapons[CurrentWeapon].Ammo = ammo;
                if (CurrentWeapon == slotId)
                {
                    player.RemoveAllWeapons();
                    SafeTrigger.ResetSharedData(player, "weapon:current");
                    if(false && slotId != WeaponSlots.Weapon3 && slotId != WeaponSlots.Weapon4) SafeTrigger.SetSharedData(player, $"attach:weapon:{(int)slotId}", Weapons[slotId].GetWeaponData());
                    CurrentWeapon = WeaponSlots.Invalid;
                }
                else
                {
                    if(false && slotId != WeaponSlots.Weapon3 && slotId != WeaponSlots.Weapon4) SafeTrigger.ResetSharedData(player, $"attach:weapon:{(int)slotId}");
                    int ammos = Weapons[slotId].Config.AmmoType == ItemNames.ThrowablesAmmo ? 1 : 0;
                    player.GiveWeapon((WeaponHash)Weapons[slotId].Config.WeaponHash, ammos);
                    SafeTrigger.SetSharedData(player, $"weapon:current", Weapons[slotId].GetWeaponData());
                    if(false && Weapons[CurrentWeapon] != null && CurrentWeapon != WeaponSlots.Weapon3 && CurrentWeapon != WeaponSlots.Weapon4) SafeTrigger.SetSharedData(player, $"attach:weapon:{(int)CurrentWeapon}", Weapons[CurrentWeapon].GetWeaponData());
                    CurrentWeapon = slotId;
                }
            }
        }

        public bool TryEquipItem(ExtPlayer player, BaseItem clothes, ClothesSlots slot)
        {
            if (player != _owner) return false;
            if (Clothes[slot]?.Promo ?? false && !clothes.Promo) return false;
            return true;
        }

        public bool TryEquipItem(ExtPlayer player, BaseItem weapon, WeaponSlots slot)
        {
            if (player != _owner) return false;
            if (Weapons[slot]?.Promo ?? false && !weapon.Promo) return false;
            return true;
        }

        public bool EquipItem(ExtPlayer player, ClothesBase clothes, ClothesSlots slot, ref BaseItem oldItem, LogAction log = LogAction.Create)
        {
            if (!TryEquipItem(player, clothes, slot)) return false;

            oldItem = Clothes[slot];
            if (oldItem?.Name == ItemNames.BodyArmor)
            {
                ((Clothes)oldItem).Armor = player.Armor;
            }
            Clothes[slot] = clothes;
            Update(slot);
            _changed = true;
            if (log > LogAction.None)
                GameLog.ItemsLog(clothes.Id, "-1", $"e{Id}", clothes.Name, clothes.Count, clothes.GetItemLogData(), log);
            return true;
        }

        public bool EquipItem(ExtPlayer player, Weapon weapon, WeaponSlots slot, ref BaseItem oldItem, LogAction log = LogAction.Create)
        {
            if (!TryEquipItem(player, weapon, slot)) return false;
            oldItem = Weapons[slot];
            Weapons[slot] = weapon;
            if (LastWeapon == slot)
            {
                LastWeapon = WeaponSlots.Invalid;
            }
            if(CurrentWeapon == slot)
            {
                int ammos = weapon.Config.AmmoType == ItemNames.ThrowablesAmmo ? 1 : 0;
                player.GiveWeapon((WeaponHash)weapon.Config.WeaponHash, ammos);
                SafeTrigger.SetSharedData(player, "weapon:current", weapon.GetWeaponData());
            }
            else
            {
                if(false && slot != WeaponSlots.Weapon3 && slot != WeaponSlots.Weapon4) SafeTrigger.SetSharedData(player, $"attach:weapon:{(int)slot}", weapon.GetWeaponData());
            }
            Update(slot);
            _changed = true;
            if (log > LogAction.None)
                GameLog.ItemsLog(weapon.Id, "-1", $"e{Id}", weapon.Name, weapon.Count, weapon.GetItemLogData(), log);
            return true;
        }
        public void MarkAsChanged()
        {
            _changed = true;
        }

        public BaseItem RemoveItem(ExtPlayer player, ClothesSlots slot, LogAction log = LogAction.Delete)
        {
            if (player != _owner) return null;
            var oldItem = Clothes[slot];
            if(oldItem?.Name == ItemNames.BodyArmor)
            {
                oldItem.Armor = player.Armor;
            }
            Clothes[slot] = null;
            //InventoryService.DefaultClothes[slot].Set(player);
            Update(slot);
            _changed = true;
            if (oldItem != null && log > LogAction.None)
                GameLog.ItemsLog(oldItem.Id, $"e{Id}", "-1", oldItem.Name, oldItem.Count, oldItem.GetItemLogData(), log);
            return oldItem;
        }

        public BaseItem RemoveItem(ExtPlayer player, WeaponSlots slot, LogAction log = LogAction.Delete)
        {
            if (player != _owner) return null;
            var oldItem = Weapons[slot];
            Weapons[slot] = null;
            Update(slot);
            _changed = true;
            if (oldItem != null && log > LogAction.None)
                GameLog.ItemsLog(oldItem.Id, $"e{Id}", "-1", oldItem.Name, oldItem.Count, oldItem.GetItemLogData(), log);
            return oldItem;
        }

        public void MoveItem(ExtPlayer player, WeaponSlots from, WeaponSlots to)
        {
            if (player != _owner) return;
            var temp = Weapons[from];
            if (temp == null) return;
            Weapons[from] = Weapons[to];
            Weapons[to] = temp;
            _changed = true;
            Update(from);
            Update(to);
        }

        public void Subscribe(ExtPlayer player)
        {
            _owner = player;
            Sync();
        }
        public bool RemoveWeapons(Func<KeyValuePair<WeaponSlots,Weapon>, bool> predicate, ExtPlayer player = null)
        {
            var items = Weapons.Where(predicate).ToList();
            if(items.Count > 0) _changed = true;
            foreach (var item in items)
            {
                Weapons[item.Key] = null;
                if (player != null)
                    DropSystem.DropItem(item.Value, player.Position, player.Dimension);
            }
            return items.Count > 0;
        }
        public bool RemoveClothes(Func<KeyValuePair<ClothesSlots, ClothesBase>, bool> predicate, LogAction log = LogAction.Delete, ExtPlayer player = null)
        {
            var items = Clothes.Where(item => item.Value != null).Where(predicate).ToList();
            if (items.Count > 0) _changed = true;
            foreach (var item in items)
            {
                Clothes[item.Key] = null;
                if (_owner.IsLogged())
                    Update(item.Key);
                if (player != null)
                    DropSystem.DropItem(item.Value, player.Position, player.Dimension);
                if (log > LogAction.None)
                    GameLog.ItemsLog(item.Value.Id, $"e{Id}", player != null ? "0" : "-1", item.Value.Name, item.Value.Count, item.Value.GetItemLogData(), log);
            }
            return items.Count > 0;
        }

        public void Reset()
        {
            Clothes = GetDefaultClothes();
            Weapons = GetDefaultWeapons();
            _changed = true;
        }
    }
}
