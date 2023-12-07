using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Jobs.Farm;

namespace Whistler.Inventory.Configs.Models
{
    public class FertilizerConfig : BaseConfig
    {
        public PlantType PlantType { get; set; }
        public FertilizerType FertilizerType { get; set; }
    }
}
