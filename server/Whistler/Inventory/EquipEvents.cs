using GTANetworkAPI;
using System;
using Whistler.AntiCheatServices;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.MP.Arena.Helpers;
using Whistler.SDK;

namespace Whistler.Inventory
{
    class EquipEvents : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(EquipEvents));

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart() 
        {
            try
            {
                EquipService.InitDataBase();
            }
            catch (Exception ex)
            {
                _logger.WriteError($"OnResourceStart:\n{ex}");
            }
            
        }

        //[ServerEvent(Event.PlayerDamage)]
        //public void OnPlayerDamage(Player player, float healthLoss, float armorLoss)
        //{
        //    Console.WriteLine($"OnPlayerDamage {player.Name} h:{healthLoss} h:{armorLoss}");
        //}

        [RemoteEvent("equip:equip")]
        public void EquipItem(Player player, int fromId, int fromIndex, int toIndex)
        {
            try
            {
                if (!player.IsLogged()) return;
                var inventory = InventoryService.GetById(fromId);
                if (inventory == null) return;
                if (inventory.IsSubscribed(player))
                {
                    var linkItem = inventory.GetItemLink(fromIndex);
                    if (linkItem == null)
                    {
                        player.SyncInventory(fromId);
                        return;
                    }
                    var equip = player.GetEquip();
                    if (equip == null) return;
                    if (equip.IsTemp != (linkItem.Id < 0)) return;
                    if (linkItem is ClothesBase)
                    {
                        if (player.IsInVehicle)
                        {
                            Notify.SendError(player, "equip:event:vehicle");
                            return;
                        }
                        if(!(linkItem as ClothesBase).AvailableForGender(player.GetGender()))
                        {
                            Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "gender:bad", 3000);
                            return;
                        }
                        var slotTo = (ClothesSlots)toIndex;
                        if (!equip.TryEquipItem(player, inventory.GetItemLink(fromIndex), slotTo))
                            return;
                        var clothes = (ClothesBase)inventory.SubItem(fromIndex, log: LogAction.None);
                        BaseItem oldItem = null;
                        if (equip.EquipItem(player, clothes, slotTo, ref oldItem, LogAction.None))
                        {
                            Core.GameLog.ItemsLog(clothes.Id, $"i{inventory.Id}", $"e{equip.Id}", clothes.Name, clothes.Count, clothes.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                            if (oldItem != null)
                            {
                                if (!inventory.AddItem(oldItem, log: LogAction.None))
                                {
                                    DropSystem.DropItem(oldItem, player.Position, player.Dimension);
                                    Core.GameLog.ItemsLog(oldItem.Id, $"e{equip.Id}", "0", oldItem.Name, oldItem.Count, oldItem.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                                    Core.Animations.Animations.DropItem(player);
                                }
                                else
                                    Core.GameLog.ItemsLog(oldItem.Id, $"e{equip.Id}", $"i{inventory.Id}", oldItem.Name, oldItem.Count, oldItem.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                            }
                        }
                    }
                    else if (linkItem is Weapon)
                    {
                        var slotTo = (WeaponSlots)toIndex;
                        if (!equip.TryEquipItem(player, inventory.GetItemLink(fromIndex), slotTo))
                            return;
                        var weapon = (Weapon)inventory.SubItem(fromIndex, log: LogAction.None);
                        BaseItem oldItem = null;
                        if (equip.EquipItem(player, weapon, slotTo, ref oldItem, LogAction.None))
                        {
                            Core.GameLog.ItemsLog(weapon.Id, $"i{inventory.Id}", $"e{equip.Id}", weapon.Name, weapon.Count, weapon.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                            if (oldItem != null)
                            {
                                if (!inventory.AddItem(oldItem, log: LogAction.None))
                                {
                                    DropSystem.DropItem(oldItem, player.Position, player.Dimension);
                                    Core.GameLog.ItemsLog(oldItem.Id, $"e{equip.Id}", "0", oldItem.Name, oldItem.Count, oldItem.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                                    Core.Animations.Animations.DropItem(player);
                                }
                                else
                                    Core.GameLog.ItemsLog(oldItem.Id, $"e{equip.Id}", $"i{inventory.Id}", oldItem.Name, oldItem.Count, oldItem.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError($"EquipItem:\n{ex}");
            }
            
        }

        [RemoteEvent("equip:remove")]
        public void EquipRemoveItem(Player player, int fromId, int fromIndex, int toId, int toIndex)
        {
            try
            {
                if (!player.IsLogged()) return;
                var type = (ItemTypes)fromId;
                if (type != ItemTypes.Weapon && type != ItemTypes.Clothes) return;
                var equip = player.GetEquip();
                if (equip == null) return;
                var inventory = InventoryService.GetById(toId);
                if (inventory == null) return;
                if (!inventory.IsSubscribed(player)) return;
                if (type == ItemTypes.Clothes || type == ItemTypes.Props || type == ItemTypes.Backpack || type == ItemTypes.Costume)
                {
                    if (player.IsInVehicle)
                    {
                        Notify.SendError(player, "equip:event:vehicle");
                        return;
                    }
                    var slot = (ClothesSlots)fromIndex;
                    if (slot == ClothesSlots.Invalid) return;
                    var clothes = equip.RemoveItem(player, slot, LogAction.None);
                    if (clothes == null) equip.Update(slot);
                    else
                    {
                        if (clothes.Promo && player.GetInventory().Id != toId)
                        {
                            Core.GameLog.ItemsLog(clothes.Id, $"e{equip.Id}", "-1", clothes.Name, clothes.Count, clothes.GetItemLogData(), LogAction.Delete, player.GetCharacter().UUID);
                            return;
                        }
                        if (!inventory.AddItem(clothes, toIndex, LogAction.None))
                        {
                            DropSystem.DropItem(clothes, player.Position, player.Dimension);
                            Core.GameLog.ItemsLog(clothes.Id, $"e{equip.Id}", "0", clothes.Name, clothes.Count, clothes.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                            Core.Animations.Animations.DropItem(player);
                        }
                        else
                            Core.GameLog.ItemsLog(clothes.Id, $"e{equip.Id}", $"i{inventory.Id}", clothes.Name, clothes.Count, clothes.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                    }
                }
                else if (type == ItemTypes.Weapon)
                {
                    var slot = (WeaponSlots)fromIndex;
                    if (slot == WeaponSlots.Invalid) return;
                    var weapon = equip.RemoveItem(player, slot, LogAction.None);
                    if (weapon == null) equip.Update(slot);
                    else
                    {
                        if (weapon.Promo && player.GetInventory().Id != toId)
                        {
                            Core.GameLog.ItemsLog(weapon.Id, $"e{equip.Id}", "-1", weapon.Name, weapon.Count, weapon.GetItemLogData(), LogAction.Delete, player.GetCharacter().UUID);
                            return;
                        }
                        if (!inventory.AddItem(weapon, toIndex, LogAction.None))
                        {
                            DropSystem.DropItem(weapon, player.Position, player.Dimension);
                            Core.GameLog.ItemsLog(weapon.Id, $"e{equip.Id}", "0", weapon.Name, weapon.Count, weapon.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                            Core.Animations.Animations.DropItem(player);
                        }
                        else
                            Core.GameLog.ItemsLog(weapon.Id, $"e{equip.Id}", $"i{inventory.Id}", weapon.Name, weapon.Count, weapon.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError($"EquipRemoveItem:\n{ex}");
            }
            
        }

        [RemoteEvent("equip:pickup")]
        public void EquipPickupItem(Player player, int itemId, int toIndex)
        {
            try
            {
                if (!player.IsLogged()) return;
                var linkItem = DropSystem.GetItemLink(player, itemId);
                if (linkItem == null)return;

                var equip = player.GetEquip();
                if (equip == null) return;
                if (equip.IsTemp != (linkItem.Id < 0)) return;
                if (linkItem is ClothesBase)
                {
                    if (player.IsInVehicle)
                    {
                        Notify.SendError(player, "equip:event:vehicle");
                        return;
                    }
                    if (!(linkItem as ClothesBase).AvailableForGender(player.GetGender()))
                    {
                        Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "gender:bad", 3000);
                        return;
                    }
                    var slotTo = (ClothesSlots)toIndex;
                    if (!equip.TryEquipItem(player, DropSystem.GetItemLink(player, itemId), slotTo))
                        return;
                    var clothes = (ClothesBase)DropSystem.PickupItem(player, itemId);
                    BaseItem oldItem = null;
                    if (equip.EquipItem(player, clothes, slotTo, ref oldItem, LogAction.None))
                    {
                        Core.GameLog.ItemsLog(clothes.Id, "0", $"e{equip.Id}", clothes.Name, clothes.Count, clothes.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                        Core.Animations.Animations.PickUpItem(player);
                        if (oldItem != null)
                        {
                            DropSystem.DropItem(oldItem, player.Position, player.Dimension);
                            Core.GameLog.ItemsLog(oldItem.Id, $"e{equip.Id}", "0", oldItem.Name, oldItem.Count, oldItem.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                            Core.Animations.Animations.DropItem(player);
                        }
                    }
                }
                else if (linkItem is Weapon)
                {
                    var slotTo = (WeaponSlots)toIndex;
                    if (!equip.TryEquipItem(player, DropSystem.GetItemLink(player, itemId), slotTo))
                        return;
                    var weapon = (Weapon)DropSystem.PickupItem(player, itemId);
                    BaseItem oldItem = null;
                    if (equip.EquipItem(player, weapon, slotTo, ref oldItem, LogAction.None))
                    {
                        Core.GameLog.ItemsLog(weapon.Id, "0", $"e{equip.Id}", weapon.Name, weapon.Count, weapon.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                        Core.Animations.Animations.PickUpItem(player);
                        if (oldItem != null)
                        {
                            DropSystem.DropItem(oldItem, player.Position, player.Dimension);
                            Core.GameLog.ItemsLog(oldItem.Id, $"e{equip.Id}", "0", oldItem.Name, oldItem.Count, oldItem.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                            Core.Animations.Animations.DropItem(player);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError($"EquipPickupItem:\n{ex}");
            }
            
        }

        [RemoteEvent("equip:move")]
        public void EquipMoveItem(Player player, int fromId, int fromIndex, int toIndex)
        {
            try
            {
                if (!player.IsLogged()) return;
                var type = (ItemTypes)fromId;
                if (type != ItemTypes.Weapon) return;

                if (fromIndex == toIndex) return;
                var equip = player.GetEquip();
                if (equip == null) return;
                var from = (WeaponSlots)fromIndex;
                if (from == WeaponSlots.Invalid) return;
                var to = (WeaponSlots)toIndex;
                if (to == WeaponSlots.Invalid) return;
                equip.MoveItem(player, from, to);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"EquipMoveItem:\n{ex}");
            }
            
        }

        [RemoteEvent("equip:drop")]
        public void EquipDropItem(Player player, int fromId, int fromIndex)
        {
            try
            {
                if (!player.IsLogged()) return;
                var type = (ItemTypes)fromId;
                if (type == ItemTypes.Invalid) return;

                var equip = player.GetEquip();
                if (equip == null) return;
                var from = (WeaponSlots)fromIndex;
                if (from == WeaponSlots.Invalid) return;
                if (type == ItemTypes.Clothes || type == ItemTypes.Props || type == ItemTypes.Backpack)
                {
                    if (player.IsInVehicle)
                    {
                        Notify.SendError(player, "equip:event:vehicle");
                        return;
                    }
                    var slot = (ClothesSlots)fromIndex;
                    if (slot == ClothesSlots.Invalid) return;
                    var clothes = equip.RemoveItem(player, slot, LogAction.None);
                    if (clothes == null)
                        equip.Update(slot);
                    else
                    {
                        DropSystem.DropItem(clothes, player.Position, player.Dimension);
                        Core.GameLog.ItemsLog(clothes.Id, $"e{equip.Id}", "0", clothes.Name, clothes.Count, clothes.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                        Core.Animations.Animations.DropItem(player);
                    }
                }
                else if (type == ItemTypes.Weapon)
                {
                    var slot = (WeaponSlots)fromIndex;
                    if (slot == WeaponSlots.Invalid) return;
                    var weapon = equip.RemoveItem(player, slot, LogAction.None);
                    if (weapon == null)
                        equip.Update(slot);
                    else
                    {
                        ((Weapon)weapon).Ammo = 0;
                        DropSystem.DropItem(weapon, player.Position, player.Dimension);
                        Core.GameLog.ItemsLog(weapon.Id, $"e{equip.Id}", "0", weapon.Name, weapon.Count, weapon.GetItemLogData(), LogAction.Move, player.GetCharacter().UUID);
                        Core.Animations.Animations.DropItem(player);
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.WriteError($"EquipDropItem:\n{ex}");
            }
            
        }

        [RemoteEvent("equip:armor:destroy")]
        public void DestroyArmor(PlayerGo player)
        {
            try
            {
                if (!player.IsLogged()) return;
                var equip = player.GetEquip();
                if (equip == null || equip.Clothes[ClothesSlots.BodyArmor] == null) return;
                equip.RemoveItem(player, ClothesSlots.BodyArmor);
            }
            catch (Exception ex)
            {
                _logger.WriteError($"DestroyArmor:\n{ex}");
            }
        }

        [RemoteEvent("equip:armor:check")]
        public void CheckArmor(PlayerGo player, int armour)
        {
            WhistlerTask.Run(() =>
            {
                if (!player.IsLogged()) return;
                if (player.IsInVehicle) return;
                // Console.WriteLine($"CheckArmour ${armour} / ${player.Armor}");
                var equip = player.GetEquip();
                if (equip == null) return;
                if (equip.Clothes[ClothesSlots.BodyArmor] == null)
                {
                    if (armour > 0)
                        AntiCheatService.ArmorHack_NoArmor(player, armour);
                }
                else
                {
                    Clothes lastArmour = player.GetData<Clothes>("armour:last");
                    Clothes currentArmour = (Clothes)equip.Clothes[ClothesSlots.BodyArmor];
                    if (System.Object.ReferenceEquals(lastArmour, currentArmour))
                    {
                        if (currentArmour.Armor < armour)
                            AntiCheatService.ArmorHack_BadArmor(player, equip.Clothes[ClothesSlots.BodyArmor].Armor);
                        else
                        {
                            currentArmour.Armor = Math.Min(armour, player.Armor);
                            if (!ArenaBattleHelper.IsPlayerInAnyBattle(player))
                            {
                                equip.MarkAsChanged();
                                equip.DeleteEmptyArmor(player);
                            }
                        }
                    }
                }
            }, 250);
        }
    }
}
