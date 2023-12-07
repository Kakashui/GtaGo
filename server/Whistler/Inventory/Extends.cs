using GTANetworkAPI;
using Whistler.Helpers;
using Whistler.Inventory.Models;

namespace Whistler.Inventory
{
    public static class Extends
    {

        public static InventoryModel GetInventory(this Player player)
        {
            return player.GetCharacter().TempInventory ?? player.GetCharacter().Inventory;
        }

        public static Equip GetEquip(this Player player)
        {
            return player.GetCharacter()?.TempEquip ?? player.GetCharacter()?.Equip;
        }

        public static void ClearInventoryCache(this Player player)
        {
            var inventory = player.GetInventory();
            if (inventory != null)
                InventoryService.ClearInventoryCache(inventory.Id);
        }
    }
}
