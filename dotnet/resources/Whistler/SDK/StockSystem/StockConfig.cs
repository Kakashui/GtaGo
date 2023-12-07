using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Enums;

namespace Whistler.SDK.StockSystem
{
    class StockConfig
    {
        public StockConfig(int maxWeight, int size, InventoryTypes type)
        {
            MaxWeight = maxWeight;
            Size = size;
            Type = type;
        }
        public int MaxWeight { get; }
        public int Size { get; }
        public InventoryTypes Type { get; }

    }
}
