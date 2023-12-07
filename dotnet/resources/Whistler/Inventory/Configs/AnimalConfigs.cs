using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    public class AnimalConfigs
    {
        public static Dictionary<ItemNames, AnimalConfig> Config;
        static AnimalConfigs()
        {
            Config = new Dictionary<ItemNames, AnimalConfig>();

            Config.Add(ItemNames.Pet, new AnimalConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_box_02"),
                Type = ItemTypes.Animal,
                Weight = 10000,
                DisplayName = "item_animal",
                Image = "animal_dog",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.83), 
                
                Stackable = false,
                Disposable = false
            });

        }
    }
}
