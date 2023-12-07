using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using Whistler.Jobs.Farm;
using Whistler.Scenes.Configs;

namespace Whistler.Inventory.Configs
{
    public class FertilizerConfigs
    {
        public static Dictionary<ItemNames, FertilizerConfig> Config;
        static FertilizerConfigs()
        {
            Config = new Dictionary<ItemNames, FertilizerConfig>();

            Config.Add(ItemNames.FertilizerBigVegetable, new FertilizerConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Fertilizer,
                Weight = 400,
                DisplayName = "item_fertilizerbigvegetable",
                Image = "fertilizerbigvegetable",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                SceneName = SceneNames.Fertilizing,
                FertilizerType = FertilizerType.Big,
                PlantType = PlantType.Vegetable,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.FertilizerBigBerry, new FertilizerConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Fertilizer,
                Weight = 400,
                DisplayName = "item_fertilizerbigberry",
                Image = "fertilizerbigberry",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                SceneName = SceneNames.Fertilizing,
                FertilizerType = FertilizerType.Big,
                PlantType = PlantType.Berry,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.FertilizerBigFruit, new FertilizerConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Fertilizer,
                Weight = 400,
                DisplayName = "item_fertilizerbigfruit",
                Image = "fertilizerbigfruit",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                SceneName = SceneNames.Fertilizing,
                FertilizerType = FertilizerType.Big,
                PlantType = PlantType.Fruit,

                Stackable = true,
                Disposable = false
            });
            Config.Add(ItemNames.FertilizerStandVegetable, new FertilizerConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Fertilizer,
                Weight = 200,
                DisplayName = "item_fertilizerstandvegetable",
                Image = "fertilizerstandvegetable",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                SceneName = SceneNames.Fertilizing,
                FertilizerType = FertilizerType.Standart,
                PlantType = PlantType.Vegetable,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.FertilizerStandBerry, new FertilizerConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Fertilizer,
                Weight = 200,
                DisplayName = "item_fertilizerstandberry",
                Image = "fertilizerstandberry",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                SceneName = SceneNames.Fertilizing,
                FertilizerType = FertilizerType.Standart,
                PlantType = PlantType.Berry,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.FertilizerStandFruit, new FertilizerConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Fertilizer,
                Weight = 200,
                DisplayName = "item_fertilizerstandfruit",
                Image = "fertilizerstandfruit",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                SceneName = SceneNames.Fertilizing,
                FertilizerType = FertilizerType.Standart,
                PlantType = PlantType.Fruit,

                Stackable = true,
                Disposable = false
            });


        }
    }
}
