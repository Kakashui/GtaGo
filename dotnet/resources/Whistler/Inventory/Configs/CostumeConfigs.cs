using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    public class CostumeConfigs
    {
        public static Dictionary<ItemNames, CostumeConfig> Config;
        static CostumeConfigs()
        {
            Config = new Dictionary<ItemNames, CostumeConfig>();

            Config.Add(ItemNames.StandartCostume, new CostumeConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_cs_tshirt_ball_01"),
                Type = ItemTypes.Costume,
                Weight = 3000,
                DisplayName = "item_standartcostume",
                Image = "standartcostume",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.97),
                Slots = new List<ClothesSlots> { ClothesSlots.Costume },
                Stackable = false,
                Disposable = false,
            }); 


        }
    }
}
