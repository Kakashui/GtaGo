using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs.Models
{
    public class ClothesConfig: BaseConfig
    {
        public int ComponentId { get; set; }
        public List<ClothesSlots> Slots { get; set; }
    }
}
