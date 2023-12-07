using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.Inventory.Configs.Models
{
    public class BackpackConfig: ClothesConfig
    {
        public int MaxWeight { get; set; }
        public int Size { get; set; }
    }
}
