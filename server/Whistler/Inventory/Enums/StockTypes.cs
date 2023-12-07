namespace Whistler.Inventory.Enums
{
    public enum StockTypes
    {
        Default = 0,
        VehicleTrunk,
        OtherSock,
        /// <summary>
        /// Тип инвентаря, который хранит только одежду, экипировку, аксессуары
        /// (ошибка при попытке переложить в инвентарь с таким типом)
        /// </summary>
        ClothesStock
    }
}
