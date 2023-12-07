using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs
{
    static class ClothesConfigs
    {
        public static Dictionary<ItemNames, ClothesConfig> Config;
        static ClothesConfigs()
        {
            /*
                     Mask,
        Gloves,
        Ear,
        Leg,
        BackpackLight,
        BackpackMedium,
        BackpackLarge,
        Feet,
        Accessories,
        Jewelry,
        Shirt,
        BodyArmor,
        Top,
        Hat,
        Glasses,
        Watches,
        Bracelets,
             */
            Config = new Dictionary<ItemNames, ClothesConfig>();
            Config.Add(ItemNames.Mask, new ClothesConfig
            {
                Type = ItemTypes.Clothes,
                ComponentId = 1,
                ModelHash = NAPI.Util.GetHashKey("prop_michael_balaclava"),
                Weight = 500,
                Slots = new List<ClothesSlots> { ClothesSlots.Mask },
                DisplayName = "item_mask",
                Image = "mask",
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
            }); 

            Config.Add(ItemNames.Hat, new ClothesConfig
            {
                Type = ItemTypes.Props,
                ComponentId = 0,
                ModelHash = NAPI.Util.GetHashKey("prop_cap_01b"),
                Weight = 300,
                Slots = new List<ClothesSlots> { ClothesSlots.Hat },
                DisplayName = "item_hat",
                Image = "hat",
                DropOffsetPosition = new Vector3(0, 0, 0.94), 
                DropRotation = new Vector3(-5, 0, 0), 
            });

            Config.Add(ItemNames.Glasses, new ClothesConfig
            {
                Type = ItemTypes.Props,
                ComponentId = 1,
                ModelHash = NAPI.Util.GetHashKey("prop_cs_sol_glasses"),
                Weight = 100,
                Slots = new List<ClothesSlots> { ClothesSlots.Glasses },
                DisplayName = "item_glasses",
                Image = "glasses",
                DropOffsetPosition = new Vector3(0, 0, 0.99), 
                DropRotation = new Vector3(-10, 0, 0), 
            });

            Config.Add(ItemNames.Ear, new ClothesConfig
            {
                Type = ItemTypes.Props,
                ComponentId = 2,
                ModelHash = NAPI.Util.GetHashKey("prop_box_ammo07a"),
                Weight = 50,
                Slots = new List<ClothesSlots> { ClothesSlots.Ear },
                DisplayName = "item_ear",
                Image = "ears",
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
            });

            Config.Add(ItemNames.Top, new ClothesConfig
            {
                Type = ItemTypes.Clothes,
                ComponentId = 11,
                ModelHash = NAPI.Util.GetHashKey("prop_cs_tshirt_ball_01"),
                Weight = 1100,
                Slots = new List<ClothesSlots> { ClothesSlots.Top },
                DisplayName = "item_top",
                Image = "top",
                DropOffsetPosition = new Vector3(0, 0, 0.97), 
                
            });

            Config.Add(ItemNames.Shirt, new ClothesConfig
            {
                Type = ItemTypes.Clothes,
                ComponentId = 8,
                ModelHash = NAPI.Util.GetHashKey("prop_ld_tshirt_02"),
                Weight = 900,
                Slots = new List<ClothesSlots> { ClothesSlots.Shirt },
                DisplayName = "item_undershit",
                Image = "shirt",
                DropOffsetPosition = new Vector3(0, 0, 0.98), 
                
            });
            Config.Add(ItemNames.BodyArmor, new ClothesConfig
            {
                Type = ItemTypes.Clothes,
                ComponentId = 9,
                ModelHash = NAPI.Util.GetHashKey("prop_armour_pickup"),
                Weight = 5000,
                Slots = new List<ClothesSlots> { ClothesSlots.BodyArmor },
                DisplayName = "item_armor",
                Image = "bodyarmor",
                DropOffsetPosition = new Vector3(0, 0, 0.91), 
                DropRotation = new Vector3(-90, 0, 0),
                ShopType = 8
            });
           
            Config.Add(ItemNames.Gloves, new ClothesConfig
            {
                Type = ItemTypes.Clothes,
                ComponentId = 3,
                ModelHash = NAPI.Util.GetHashKey("prop_boxing_glove_01"),
                Weight = 200,
                Slots = new List<ClothesSlots> { ClothesSlots.Gloves },
                DisplayName = "item_gloves",
                Image = "gloves",
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                DropRotation = new Vector3(-90, 0, 0), 
            });

            Config.Add(ItemNames.Watches, new ClothesConfig
            {
                Type = ItemTypes.Props,
                ComponentId = 6,
                ModelHash = NAPI.Util.GetHashKey("p_watch_04"),
                Weight = 400,
                Slots = new List<ClothesSlots> { ClothesSlots.Watches },
                DisplayName = "item_watches",
                Image = "watch",
                DropOffsetPosition = new Vector3(0, 0, 0.99), 
                
            });

            Config.Add(ItemNames.Bracelets, new ClothesConfig
            {
                Type = ItemTypes.Props,
                ComponentId = 7,
                ModelHash = NAPI.Util.GetHashKey("p_watch_04"),
                Weight = 150,
                Slots = new List<ClothesSlots> { ClothesSlots.Bracelets },
                DisplayName = "item_bracelets",
                Image = "bracelets",
                DropOffsetPosition = new Vector3(0, 0, 0.99), 
                
            });

            Config.Add(ItemNames.Leg, new ClothesConfig
            {
                Type = ItemTypes.Clothes,
                ComponentId = 4,
                ModelHash = NAPI.Util.GetHashKey("p_laz_j02_s"),
                Weight = 800,
                Slots = new List<ClothesSlots> { ClothesSlots.Leg },
                DisplayName = "item_leg",
                Image = "leg",
                DropOffsetPosition = new Vector3(0, 0, 0.9), 
                
            });

            Config.Add(ItemNames.Feet, new ClothesConfig
            {
                Type = ItemTypes.Clothes,
                ComponentId = 6,
                ModelHash = NAPI.Util.GetHashKey("prop_ld_shoe_01"),
                Weight = 2000,
                Slots = new List<ClothesSlots> { ClothesSlots.Feet },
                DisplayName = "item_feet",
                Image = "feet",
                DropOffsetPosition = new Vector3(0, 0, 0.94), 
                
            });

            Config.Add(ItemNames.Accessories, new ClothesConfig
            {
                Type = ItemTypes.Clothes,
                ComponentId = 7,
                ModelHash = NAPI.Util.GetHashKey("prop_box_ammo07a"),
                Weight = 300,
                Slots = new List<ClothesSlots> { ClothesSlots.Accessories },
                DisplayName = "item_accessories",
                Image = "accessories",
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
            });

            Config.Add(ItemNames.Jewelry, new ClothesConfig
            {
                Type = ItemTypes.Clothes,
                ComponentId = 7,
                ModelHash = NAPI.Util.GetHashKey("prop_box_ammo07a"),
                Weight = 400,
                Slots = new List<ClothesSlots> { ClothesSlots.Accessories },
                DisplayName = "item_Jewelry",
                Image = "accessories",
                DropOffsetPosition = new Vector3(0, 0, 1), 
                
            });
        }
    }
}
