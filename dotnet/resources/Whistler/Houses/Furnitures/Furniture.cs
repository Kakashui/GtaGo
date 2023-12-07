using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Linq;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.SDK;

namespace Whistler.Houses.Furnitures
{
    internal class Furniture
    {
        [JsonProperty("position")]
        public Vector3 Position { get; set; }
        
        [JsonProperty("rotation")]
        public Vector3 Rotation { get; set; }
        
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        
        public bool Installed { get; set; }

        private InteractShape _interactShape;
        
        [JsonProperty("inventoryId")]
        public int InventoryId { get; set; } = -1;

        public Furniture(string gtaModelName)
        {
            ModelName = gtaModelName;
        }

        public Furniture() { }
        
        public FurnitureType GetFurnitureType()
        {
            if (FurnitureSettings.SafeModelHashes.Any(m => m == ModelName))
                return FurnitureType.Safe;
            if (FurnitureSettings.WardrobeModelHashes.Any(m => m == ModelName))
                return FurnitureType.Wardrobe;

            return FurnitureType.Default;
        }

        public void IfWardrobeOrSafeThanCreateInteraction(House house)
        {
            _interactShape?.Destroy();
            var type = GetFurnitureType();
            switch (type)
            {
                case FurnitureType.Safe:
                case FurnitureType.Wardrobe:
                    InventoryTypes invType = type == FurnitureType.Wardrobe ? InventoryTypes.ClothesStock : InventoryTypes.General;
                    if (InventoryId == -1)
                        InventoryId = new InventoryModel(FurnitureSettings.HouseWardrobeWeight, FurnitureSettings.HouseWardrobeSize, invType).Id;
                    if (Installed)
                        _interactShape = InteractShape.Create(Position, 1.2f, 2, house.Dimension)
                            .AddInteraction(p => PlayerInteractedFurniture(p, house));
                    break;
            }
        }

        public void DeleteAllInteractions()
        {
            _interactShape?.Destroy();
        }


        private void PlayerInteractedFurniture(ExtPlayer player, House house)
        {
            if (house.GetAccessFurniture(player, Families.FamilyFurnitureAccess.OpenFurniture))
            {
                InventoryService.OpenStock(player, InventoryId, StockTypes.Default);
                return;
            }

            var roomate = house.GetRoommate(player.Character.UUID);

            if (roomate == null)
            {
                Notify.SendError(player, "House_143");
                return;
            }
            var type = GetFurnitureType();
            if (type == FurnitureType.Safe && !roomate.HasSafeAccess)
            {
                Notify.SendError(player, "House_144");
                return;
            }
            if (type == FurnitureType.Wardrobe && !roomate.HasWardrobeAccess)
            {
                Notify.SendError(player, "House_145");
                return;
            }

            InventoryService.OpenStock(player, InventoryId, StockTypes.Default);
        }
    }
}