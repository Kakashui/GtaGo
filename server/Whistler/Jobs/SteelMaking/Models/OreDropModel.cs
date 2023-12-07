using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Enums;

namespace Whistler.Jobs.SteelMaking.Models
{
    class OreDropModel
    {
        public ItemTypes ItemTypes { get; set; }
        public ItemNames Name { get; set; }
        public int Propability { get; set; }
        public OreDropModel(ItemTypes itemTypes, ItemNames name, int propability)
        {
            ItemTypes = itemTypes;
            Name = name;
            Propability = propability;
        }
    }
}
