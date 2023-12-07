using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.PersonalEvents.Contracts.Models
{
    class AbstractItemConfig
    {
        public int Weight { get; set; }
        public AbstractItemConfig(int weight)
        {
            Weight = weight;
        }
        public AbstractItemConfig()
        {
            Weight = 9999000;
        }
    }
}
