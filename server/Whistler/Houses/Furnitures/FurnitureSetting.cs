namespace Whistler.Houses.Furnitures
{
    public class FurnitureSetting
    {
        public FurnitureShopType FurnitureType { get; }

        public int Cost { get; }

        public string Name { get; }

        public FurnitureSetting(FurnitureShopType furnitureType, int cost, string name)
        {
            FurnitureType = furnitureType;
            Cost = cost;
            Name = name;
        }
    }
}