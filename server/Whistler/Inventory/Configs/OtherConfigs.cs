using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    public class OtherConfigs
    {
        public static Dictionary<ItemNames, OtherConfig> Config;
        static OtherConfigs()
        {
            Config = new Dictionary<ItemNames, OtherConfig>();

            Config.Add(ItemNames.Lockpick, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("p_car_keys_01"),
                Type = ItemTypes.Other,
                Weight = 300,
                DisplayName = "item_lockpick",
                Image = "lockpick",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99), 
                
                Stackable = true
            });

            Config.Add(ItemNames.Material, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_security_case_01"),
                Type = ItemTypes.Other,
                Weight = 100,
                DisplayName = "item_material",
                Image = "material",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.63), 
                
                Stackable = true
            });

            Config.Add(ItemNames.ArmyLockpick, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("p_car_keys_01"),
                Type = ItemTypes.Other,
                Weight = 300,
                DisplayName = "item_armylockpick",
                Image = "armylockpick",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99), 
                
                Stackable = true
            });

            Config.Add(ItemNames.Pocket, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_michael_balaclava"),
                Type = ItemTypes.Other,
                Weight = 300,
                DisplayName = "item_pocket",
                Image = "pocket",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = true
            });

            Config.Add(ItemNames.Cuffs, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_devin_rope_01"),
                Type = ItemTypes.Other,
                Weight = 300,
                DisplayName = "item_cuffs",
                Image = "cuffs",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.96), 
                DropRotation = new Vector3(16, 0, 0), 
                Stackable = true
            });

            Config.Add(ItemNames.Present, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_box_ammo07a"),
                Type = ItemTypes.Other,
                Weight = 300,
                DisplayName = "item_present",
                Image = "present",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
                Stackable = false
            });

            Config.Add(ItemNames.TobaccoForKolyannaya, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_small"),
                Type = ItemTypes.Other,
                Weight = 200,
                DisplayName = "item_tobaccoforkolyannaya",
                Image = "tobaccoforkolyannaya",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.96), 
                DropRotation = new Vector3(0, 0, 0),
                Stackable = false
            });

            Config.Add(ItemNames.CoalsForKolyann, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_paper_bag_small"),
                Type = ItemTypes.Other,
                Weight = 500,
                DisplayName = "item_coalsforkolyann",
                Image = "coalsforkolyann",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.96), 
                DropRotation = new Vector3(0, 0, 0),
                Stackable = false
            });

            Config.Add(ItemNames.Screwdriver, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_tool_screwdvr03"),
                Type = ItemTypes.Other,
                Weight = 300,
                DisplayName = "item_screwdriver",
                Image = "screwdriver",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.98), 
                DropRotation = new Vector3(90, 0, 0), 
                Stackable = false
            });

            Config.Add(ItemNames.FishingBait, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_pot_04"),
                Type = ItemTypes.Other,
                Weight = 300,
                DisplayName = "item_fishbait",
                Image = "fishbait",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.99), 
                DropRotation = new Vector3(0, 0, 0), 
                Stackable = true
            });

            Config.Add(ItemNames.Milt, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_pot_04"),
                Type = ItemTypes.Other,
                Weight = 300,
                DisplayName = "item_milt",
                Image = "item_milt",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                DropRotation = new Vector3(0, 0, 0),
                Stackable = true
            });

            Config.Add(ItemNames.Cigarettes, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_cigar_pack_01"),
                Type = ItemTypes.Other,
                Weight = 300,
                DisplayName = "item_cigarettes",
                Image = "cigarettes",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                DropRotation = new Vector3(0, 0, 0),
                Stackable = true,
                ActionsCount = 5,
                SceneName = Scenes.Configs.SceneNames.Cigarette
            });

            Config.Add(ItemNames.Radio, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_cs_hand_radio"),
                Type = ItemTypes.Other,
                Weight = 600,
                DisplayName = "item_radio",
                Image = "radio",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                DropRotation = new Vector3(0, 0, 0),
                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.AnimalSkin, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_pot_04"),
                Type = ItemTypes.Other,
                Weight = 500,
                DisplayName = "item_animalskin",
                Image = "animalskin",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                DropRotation = new Vector3(0, 0, 0),
                Stackable = true
            });

            Config.Add(ItemNames.Tablet, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_cs_tablet"),
                Type = ItemTypes.Other,
                Weight = 400,
                DisplayName = "item_tablet",
                Image = "tablet",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                DropRotation = new Vector3(0, 0, 0),
                Stackable = false,
                Disposable = false,
                SceneName = Scenes.Configs.SceneNames.Tablet,
            });

            Config.Add(ItemNames.Guitar, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_acc_guitar_01"),
                Type = ItemTypes.Other,
                Weight = 1500,
                DisplayName = "item_guitar",
                Image = "guitar",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.94),
                DropRotation = new Vector3(90, 0, 0),
                Stackable = false,
                Disposable = false,
                SceneName = Scenes.Configs.SceneNames.Guitar,
            });

            Config.Add(ItemNames.Camera, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_v_cam_01"),
                Type = ItemTypes.Other,
                Weight = 1500,
                DisplayName = "item_camera",
                Image = "camera",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                DropRotation = new Vector3(0, 0, 0),
                Stackable = false,
                Disposable = false,
                SceneName = Scenes.Configs.SceneNames.Camera,
            });

            Config.Add(ItemNames.Microphone, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_microphone_02"),
                Type = ItemTypes.Other,
                Weight = 1500,
                DisplayName = "item_microphone",
                Image = "microphone",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                DropRotation = new Vector3(0, -90, 0),
                Stackable = false,
                Disposable = false,
                SceneName = Scenes.Configs.SceneNames.Microphone,
            });

            Config.Add(ItemNames.Binoculars, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_binoc_01"),
                Type = ItemTypes.Other,
                Weight = 1500,
                DisplayName = "item_binoculars",
                Image = "binoculars",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                DropRotation = new Vector3(0, -90, 0),
                Stackable = false,
                Disposable = false,
                SceneName = Scenes.Configs.SceneNames.Binoculars,
            });

            Config.Add(ItemNames.Clipboard, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("p_amb_clipboard_01"),
                Type = ItemTypes.Other,
                Weight = 1500,
                DisplayName = "item_clipboard",
                Image = "clipboard",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                DropRotation = new Vector3(0, -90, 0),
                Stackable = false,
                Disposable = false,
                SceneName = Scenes.Configs.SceneNames.Clipboard
            });

            Config.Add(ItemNames.Lighter, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("p_cs_lighter_01"),
                Type = ItemTypes.Other,
                Weight = 1500,
                DisplayName = "item_lighter",
                Image = "lighter",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                DropRotation = new Vector3(0, -90, 0),
                Stackable = false,
                Disposable = false
            });

            Config.Add(ItemNames.Bong, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_bong_01"),
                Type = ItemTypes.Other,
                Weight = 1500,
                DisplayName = "item_bong",
                Image = "bong",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                DropRotation = new Vector3(0, -90, 0),
                Stackable = false,
                Disposable = false
            });

            Config.Add(ItemNames.FoodBox, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("ng_proc_crate_04a"),
                Type = ItemTypes.Other,
                Weight = 1500,
                DisplayName = "item_foodbox",
                Image = "foodbox",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                DropRotation = new Vector3(0, 0, 0),
                SceneName = Scenes.Configs.SceneNames.Harvesting,
                Stackable = false,
                Disposable = false
            });

            Config.Add(ItemNames.Dynamite, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("stt_prop_c4_stack"),
                Type = ItemTypes.Other,
                Weight = 3000,
                DisplayName = "item_dynamite",
                Image = "dynamite",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                DropRotation = new Vector3(0, -90, 0),
                SceneName = Scenes.Configs.SceneNames.DynamitePlant,
                Stackable = true,
                Disposable = false
            });

            Config.Add(ItemNames.Detector, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("w_am_digiscanner"),
                Type = ItemTypes.Other,
                Weight = 1000,
                DisplayName = "item_detector",
                Image = "detector",
                CanUse = true,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                DropRotation = new Vector3(0, -90, 0),
                SceneName = Scenes.Configs.SceneNames.ScannerUse,
                Stackable = false,
                Disposable = false
            });

            Config.Add(ItemNames.Stetoskop, new OtherConfig
            {
                ModelHash = NAPI.Util.GetHashKey("prop_ear_defenders_01"),
                Type = ItemTypes.Other,
                Weight = 800,
                DisplayName = "item_stetoskop",
                Image = "stetoskop",
                CanUse = false,
                DropOffsetPosition = new Vector3(0, 0, 0.99),
                DropRotation = new Vector3(0, -90, 0),
                Stackable = false,
                Disposable = false
            });
        }
    }
}
