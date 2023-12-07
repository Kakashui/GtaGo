using GTANetworkAPI;
using System;
using System.Linq;
using Whistler.Core;
using Whistler.Core.CustomSync;
using Whistler.Helpers;
using Whistler.Inventory.Enums;
using Whistler.Scenes.Configs;
using Whistler.SDK;
using Whistler.Entities;

namespace Whistler.Inventory
{
    public class InventoryEvents : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(InventoryEvents));

        public static Action<ExtPlayer, int, ItemTypes> ItemInteract;

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            try
            {
                InventoryService.InitDataBase();
                DevEvents.ParseConfigs();
                Selecting.ObjectInteract += WorldObjectInteracted;
                ItemInteract += PickupItem;
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }            
        }

        public static void WorldObjectInteracted(ExtPlayer player, GTANetworkAPI.Object entity)
        {
            if (entity.HasSharedData("data:object:id"))
            {
                if (entity.Position.DistanceTo(player.Position) > 2)
                    return;
                int itemId = entity.GetSharedData<int>("data:object:id");
                var item = DropSystem.GetItemLink(player, itemId);
                if (item != null)
                    ItemInteract?.Invoke(player, itemId, item.Type);
            }
        }
        public static void PickupItem(ExtPlayer player, int itemId, ItemTypes itemType)
        {
            if (itemType == ItemTypes.ItemBox)
                return;

            var itemFrom = DropSystem.PickupItem(player, itemId, 999999);
            if (player.GetInventory().AddItem(itemFrom, log:LogAction.None))
            {
                GameLog.ItemsLog(itemFrom.Id, "0", $"i{player.GetInventory().Id}", itemFrom.Name, itemFrom.Count, itemFrom.GetItemLogData(), LogAction.Move, player.Character.UUID);
                Core.Animations.Animations.PickUpItem(player);
            }
            else
                DropSystem.DropItem(itemFrom, player.Position, player.Dimension);
        }


        [RemoteEvent("inv:moveItem")]
        public void MoveItem(ExtPlayer player, int fromId, int fromIndex, int toId, int toIndex, int count)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (player.HasSharedData("scene:current") && player.GetSharedData<int>("scene:current") != (int)SceneNames.NoAction) return;
                var inventoryFrom = InventoryService.GetById(fromId);
                if (inventoryFrom == null) return;
                var itemFrom = inventoryFrom.GetItemLink(fromIndex);
                if (itemFrom == null)
                    player.SyncInventory(inventoryFrom.Id);
                else
                {
                    var local = fromId == toId;
                    var inventoryTo = local ? inventoryFrom : InventoryService.GetById(toId);
                    if (inventoryTo == null) return;
                    //if (!inventoryTo.Class.CanAddedItem(itemFrom)) return;
                    if (itemFrom.Promo && !local && inventoryTo.Id != player.GetInventory().Id) return;
                    if (inventoryTo.IsTemp != (itemFrom.Id < 0)) return;
                    itemFrom = inventoryFrom.SubItem(fromIndex, count, LogAction.None);
                    if (!inventoryTo.AddItem(itemFrom, toIndex, LogAction.None))
                    {
                        if (!inventoryFrom.AddItem(itemFrom, fromIndex, LogAction.None))
                        {
                            DropSystem.DropItem(itemFrom, player.Position, player.Dimension);
                            GameLog.ItemsLog(itemFrom.Id, $"i{inventoryFrom.Id}", "0", itemFrom.Name, itemFrom.Count, itemFrom.GetItemLogData(), LogAction.Move, player.Character?.UUID ?? -1);
                            Core.Animations.Animations.DropItem(player);
                        }
                    }
                    else
                        GameLog.ItemsLog(itemFrom.Id, $"i{inventoryFrom.Id}", $"i{inventoryTo.Id}", itemFrom.Name, itemFrom.Count, itemFrom.GetItemLogData(), LogAction.Move, player.Character.UUID);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }
        }

        [RemoteEvent("inv:drop")]
        public void DropItem(ExtPlayer player, int fromId, int fromIndex, int count)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (player.HasSharedData("scene:current") && player.GetSharedData<int>("scene:current") != (int)SceneNames.NoAction) return;
                var inventoryFrom = InventoryService.GetById(fromId);
                if (inventoryFrom == null) return;
                var itemFrom = inventoryFrom.SubItem(fromIndex, count, LogAction.None);
                if (itemFrom == null) player.SyncInventory(inventoryFrom.Id);
                else
                {
                    DropSystem.DropItem(itemFrom, player.Position, player.Dimension);
                    GameLog.ItemsLog(itemFrom.Id, $"i{inventoryFrom.Id}", "0", itemFrom.Name, itemFrom.Count, itemFrom.GetItemLogData(), LogAction.Move, player.Character.UUID);
                    Core.Animations.Animations.DropItem(player);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }            
        }

        [RemoteEvent("inv:use:itembox")]
        public void Useitem(ExtPlayer player, int invId, int itemIndex)
        {
            try
            {
                if (!player.IsLogged()) return;
                var inventory = InventoryService.GetById(invId);
                if (inventory == null) return;
                var link = inventory.GetItemLink(itemIndex);
                if (link == null)
                {
                    inventory.Update(player);
                    return;
                }
                if (!link.CanUse(player))
                    return;
                var item = link.IsDisposable() ? inventory.SubItem(itemIndex, 1) : link;
                if (!item.Use(player))
                    inventory.Update(player);
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }
            
        }

        [RemoteEvent("inv:use:item")]
        public void Useitem(ExtPlayer player, int itemIndex)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (player.HasSharedData("scene:current") && player.GetSharedData<int>("scene:current") != (int)SceneNames.NoAction) return;
                if (player.GetEquip().CurrentWeapon != WeaponSlots.Invalid)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "act:canc:w", 3000);
                    return;
                }
                var inventory = player.GetInventory();
                lock(inventory){
                    if (inventory == null) return;
                    var link = inventory.GetItemLink(itemIndex);
                    if (link == null)
                    {
                        inventory.Update(player);
                        return;
                    }
                    if (!link.CanUse(player))
                        return;
                    if (!link.Use(player)) return;
                    if (link.IsDisposable())
                        inventory.SubItem(itemIndex, 1, LogAction.Use);
                }                
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }
            
        }

        [RemoteEvent("inv:use:fast")]
        public void UseItemFast(ExtPlayer player, int itemId)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (player.HasSharedData("scene:current") && player.GetSharedData<int>("scene:current") != (int)SceneNames.NoAction) return;
                if (player.GetEquip().CurrentWeapon != WeaponSlots.Invalid)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "act:canc:w", 3000);
                    return;
                }
                var inventory = player.GetInventory();
                if (inventory == null) return;
                var link = inventory.GetItemLink((ItemNames)itemId);
                if (link == null)
                {
                    inventory.Update(player);
                    return;
                }
                if (!link.CanUse(player))
                    return;
                if (!link.Use(player)) return;
                if (link.IsDisposable())
                    inventory.SubItemByName((ItemNames)itemId, 1, LogAction.Use);
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }
            
        }

        [RemoteEvent("inv:get:byid")]
        public void GetInventoryById(ExtPlayer player, int id)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (id == 0)
                {
                    SafeTrigger.ClientEvent(player,"inv:update", 0, player.GetNearItemsDTO(), -1, 2);
                }
                else
                {
                    var inventory = InventoryService.GetById(id);
                    if (inventory == null) return;
                    inventory.Update(player);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }
            
        }
        [RemoteEvent("inv:get:personal")]
        public void GetPersonalId(ExtPlayer player)
        {
            InventoryService.SyncInventoryId(player);
        }

        [RemoteEvent("inv:pickup")]
        public void PickupItem(ExtPlayer player, int itemId, int toId, int toIndex, int count)
        {
            try
            {
                if (!player.IsLogged()) return;
                var inventoryFrom = InventoryService.GetById(toId);
                if (inventoryFrom == null) return;
                var itemFrom = DropSystem.GetItemLink(player, itemId);
                //if (!inventoryFrom.Class.CanAddedItem(itemFrom)) return;
                itemFrom = DropSystem.PickupItem(player, itemId, count);
                if (itemFrom == null) return;
                if (inventoryFrom.IsTemp != (itemFrom.Id < 0)) return;
                if (!inventoryFrom.AddItem(itemFrom, toIndex, LogAction.None))
                    DropSystem.DropItem(itemFrom, player.Position, player.Dimension);
                else
                {
                    GameLog.ItemsLog(itemFrom.Id, "0", $"i{inventoryFrom.Id}", itemFrom.Name, itemFrom.Count, itemFrom.GetItemLogData(), LogAction.Move, player.Character.UUID);
                    Core.Animations.Animations.PickUpItem(player);
                }

            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }
        }

        [RemoteEvent("inv:open")]
        public void OpenInventory(ExtPlayer player)
        {
            try
            {
                if (!player.IsLogged()) return;
                DropSystem.Subscribe(player);
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }
            
        }

        [RemoteEvent("inv:close")]
        public void CloseInventory(ExtPlayer player, int stockId)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (stockId > 0) InventoryService.GetById(stockId)?.Unsubscribe(player);
                DropSystem.Unsubscribe(player);
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }

        }

        [Command("spmw")]
        public void SetPlayerWeight(ExtPlayer player, int playerId, int val)
        {
            try
            {
                if (!player.IsLogged() || val < 1) return; 
                if (!Group.CanUseAdminCommand(player, "setinventoryweight")) return;

                ExtPlayer target = Trigger.GetPlayerById(playerId);
                if(target == null)
                {
                    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "spmw", 3000);
                    return;
                }
                var inventory = target.GetInventory();
                inventory.ChangeMaxWeight(val);
                inventory.Update(target);
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }

        }
    }
}
