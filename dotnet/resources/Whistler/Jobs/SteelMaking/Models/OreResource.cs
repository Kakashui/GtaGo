using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Enums;

namespace Whistler.Jobs.SteelMaking.Models
{
    class OreResource
    {
        public OreResource(ItemNames commonResource, ItemNames perfectResource)
        {
            CommonResource = commonResource;
            PerfectResource = perfectResource;
        }

        public ItemNames CommonResource { get; set; }
        public ItemNames PerfectResource { get; set; }
    }
}
