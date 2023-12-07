using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using Whistler.Scenes.Configs;

namespace Whistler.Inventory.Configs
{
    public class SeedConfigs
    {
        public static Dictionary<ItemNames, SeedConfig> Config;
        static SeedConfigs()
        {
            Config = new Dictionary<ItemNames, SeedConfig>();

            Config.Add(ItemNames.CabbageSeed, new SeedConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Seed,
                Weight = 5,
                DisplayName = "item_cabbageseed",
                Image = "cabbageseed",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                SceneName = SceneNames.SeatSeed,


                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.PumpkinSeed, new SeedConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Seed,
                Weight = 5,
                DisplayName = "item_pumpkinseed",
                Image = "pumpkinseed",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = SceneNames.SeatSeed,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.ZucchiniSeed, new SeedConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Seed,
                Weight = 5,
                DisplayName = "item_zucchiniseed",
                Image = "zucchiniseed",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = SceneNames.SeatSeed,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.WatermelonSeed, new SeedConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Seed,
                Weight = 5,
                DisplayName = "item_watermelonseed",
                Image = "watermelonseed",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = SceneNames.SeatSeed,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.TomatoSeed, new SeedConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Seed,
                Weight = 5,
                DisplayName = "item_tomatoseed",
                Image = "tomatoseed",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = SceneNames.SeatSeed,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.StrawberrySeed, new SeedConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Seed,
                Weight = 5,
                DisplayName = "item_strawberryseed",
                Image = "strawberryseed",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = SceneNames.SeatSeed,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.RaspberriesSeed, new SeedConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Seed,
                Weight = 5,
                DisplayName = "item_raspberriesseed",
                Image = "raspberriesseed",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = SceneNames.SeatSeed,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.RadishSeed, new SeedConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Seed,
                Weight = 5,
                DisplayName = "item_radishseed",
                Image = "radishseed",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = SceneNames.SeatSeed,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.PotatoesSeed, new SeedConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Seed,
                Weight = 5,
                DisplayName = "item_potatoesseed",
                Image = "potatoesseed",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = SceneNames.SeatSeed,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.OrangeSeed, new SeedConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Seed,
                Weight = 5,
                DisplayName = "item_orangeseed",
                Image = "orangeseed",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = SceneNames.SeatSeed,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.CucumberSeed, new SeedConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Seed,
                Weight = 5,
                DisplayName = "item_cucumberseed",
                Image = "cucumberseed",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = SceneNames.SeatSeed,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.CarrotSeed, new SeedConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Seed,
                Weight = 5,
                DisplayName = "item_carrotseed",
                Image = "carrotseed",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = SceneNames.SeatSeed,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.BananaSeed, new SeedConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Seed,
                Weight = 5,
                DisplayName = "item_bananaseed",
                Image = "bananaseed",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = SceneNames.SeatSeed,

                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.AppleSeed, new SeedConfig
            {
                ModelHash = NAPI.Util.GetHashKey("bkr_prop_weed_bucket_open_01a"),
                Type = ItemTypes.Seed,
                Weight = 5,
                DisplayName = "item_appleseed",
                Image = "appleseed",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.9),
                SceneName = SceneNames.SeatSeed,

                Stackable = true,
                Disposable = false
            });

        }
    }
}
