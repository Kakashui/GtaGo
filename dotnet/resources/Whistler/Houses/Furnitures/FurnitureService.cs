using GTANetworkAPI;
using Newtonsoft.Json;
using System.Linq;
using Whistler.Entities;
using Whistler.Inventory;
using Whistler.SDK;

namespace Whistler.Houses.Furnitures
{
    internal class FurnitureService : Script
    {
        public FurnitureService()
        {
            HouseManager.PlayerEntered += LoadHouseFurnitureForPlayer;
            HouseManager.PlayerLeaved += DestroyHouseFurnitureForPlayer;
        }

        public static void InitializeForHouse(House house)
        {
            house.Furnitures.ForEach(f =>
            {
                if (f.Installed)
                    f.IfWardrobeOrSafeThanCreateInteraction(house);
            });
        }

        private static void LoadHouseFurnitureForPlayer(ExtPlayer player, House house)
        {
            SafeTrigger.ClientEvent(player, "house::playerEntered", JsonConvert.SerializeObject(house.Furnitures.Where(f => f.Installed)), house.Dimension);
        }

        private static void DestroyHouseFurnitureForPlayer(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"house::playerLeaved");
        }

        public static void AddFurniture(Furniture furniture, House house)
        {
            house.Furnitures.Add(furniture);
            house.UpdateFurnitures();
        }

        public static void RemoveFurniture(Furniture furniture, House house)
        {
            DeinstallFurniture(furniture, house);
            house.Furnitures.Remove(furniture);
            if (furniture.InventoryId != -1) 
                InventoryService.DestroyInventory(furniture.InventoryId);
        }

        public static void DeinstallFurniture(Furniture furniture, House house)
        {
            furniture.Installed = false;
            furniture.DeleteAllInteractions();
            SafeTrigger.ClientEventToPlayers(house.PlayersInside.ToArray(), "house::updateFurniture", JsonConvert.SerializeObject(house.Furnitures.Where(f => f.Installed)), house.Dimension);
            house.UpdateFurnitures();
        }

        public static void InstallFurniture(House house, Furniture furniture, Vector3 position, Vector3 rotation)
        {
            furniture.Installed = true;
            furniture.Position = position;
            furniture.Rotation = rotation;
            furniture.IfWardrobeOrSafeThanCreateInteraction(house);

            SafeTrigger.ClientEventToPlayers(house.PlayersInside.ToArray(), "house::updateFurniture", JsonConvert.SerializeObject(house.Furnitures.Where(f => f.Installed)), house.Dimension);
            house.UpdateFurnitures();
        }
    }
}