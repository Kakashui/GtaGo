using GTANetworkAPI;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Models;

namespace Whistler.Inventory
{
    public static class Extends
    {

        public static InventoryModel GetInventory(this ExtPlayer player)
        {
            return player.Character.TempInventory ?? player.Character.Inventory;
        }

        public static Equip GetEquip(this ExtPlayer player)
        {
            return player.Character?.TempEquip ?? player.Character?.Equip;
        }

        public static void ClearInventoryCache(this ExtPlayer player)
        {
            var inventory = player.GetInventory();
            if (inventory != null)
                InventoryService.ClearInventoryCache(inventory.Id);
        }
    }
}
