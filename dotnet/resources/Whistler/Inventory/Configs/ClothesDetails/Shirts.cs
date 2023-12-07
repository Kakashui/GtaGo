using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Whistler.Inventory.Enums;
using Newtonsoft.Json;
using Whistler.ClothesCustom;
using GTANetworkAPI;
using Whistler.Helpers;
using Whistler.Entities;

namespace Whistler.Inventory.Configs.ClothesDetails
{
    public class Shirts
    {
        private class ShirtConfig
        {
            public ShirtConfig(Dictionary<ClothesTopTypes, int> undershits, List<int> colors, int armor)
            {
                Undershits = undershits;
                Colors = colors;
                Armor = armor;
            }
            public ShirtConfig(){}

            public Dictionary<ClothesTopTypes, int> Undershits { get; set; }
            public List<int> Colors { get; set; }
            public int Armor { get; set; }
        }

        private static string _fileName = "Shirts";
        static Shirts()
        {
            if (File.Exists($"Configs/Clothes/Male/{_fileName}.json")) {
                using (var r = new StreamReader($"Configs/Clothes/Male/{_fileName}.json"))
                {
                    _maleConfig = JsonConvert.DeserializeObject<Dictionary<int, ShirtConfig>>(r.ReadToEnd());
                }
                using (var r = new StreamReader($"Configs/Clothes/Female/{_fileName}.json"))
                {
                    _femaleConfig = JsonConvert.DeserializeObject<Dictionary<int, ShirtConfig>>(r.ReadToEnd());
                }
            }
            else
            {
                _maleConfig = new Dictionary<int, ShirtConfig>();
                _femaleConfig = new Dictionary<int, ShirtConfig>();
            }
            
        }

        //drawable : {type / correctedDrawable}
        #region male undershits
        private static Dictionary<int, ShirtConfig> _maleConfig;
        #endregion

        #region female undershits
        private static Dictionary<int, ShirtConfig> _femaleConfig;
        #endregion

        public static int GetFixedDrawable(bool gender, int topDrawable, ClothesTopTypes topType)
        {
            var config = gender ? _maleConfig : _femaleConfig;
            if (!config.ContainsKey(topDrawable)) 
                return InventoryService.DefaultClothes[ClothesSlots.Shirt].GetDrawable(gender);
            if(topType == ClothesTopTypes.Invalid || !config[topDrawable].Undershits.ContainsKey(topType)) 
                return InventoryService.DefaultClothes[ClothesSlots.Shirt].GetDrawable(gender);
            return config[topDrawable].Undershits[topType];
        }

        public static int GetColor(bool gender, int drawable, int colorIndex)
        {
            var config = gender ? _maleConfig : _femaleConfig;
            if (colorIndex < 0 || !config.ContainsKey(drawable)) return 0;
            if (config[drawable].Colors.Count <= colorIndex) return 0;
            return config[drawable].Colors[colorIndex];
        }

        public static int GetColor(ExtPlayer player, int drawable, int colorIndex)
        {
            var gender = player.GetGender();
            var config = gender ? _maleConfig : _femaleConfig;
            if (colorIndex < 0 || !config.ContainsKey(drawable)) return 0;
            if (config[drawable].Colors.Count <= colorIndex) return 0;
            return config[drawable].Colors[colorIndex];
        }

        public static void Add(bool gender, int id, Dictionary<int, int> undershirt, List<int> colors, int armor)
        {
            if (_maleConfig == null) _maleConfig = new Dictionary<int, ShirtConfig>();
            if (_femaleConfig == null) _femaleConfig = new Dictionary<int, ShirtConfig>();
            var convert = new Dictionary<ClothesTopTypes, int>();
            foreach (var item in undershirt)
            {
                convert.Add((ClothesTopTypes)(item.Key + 1), item.Value);
            }
            if (gender)
                _maleConfig.Add(id, new ShirtConfig(convert, colors, armor));
            else
                _femaleConfig.Add(id, new ShirtConfig(convert, colors, armor));
        }

        public static void Parse()
        {
            using (var w = new StreamWriter($"Configs/Clothes/Male/{_fileName}.json"))
            {
                w.Write(JsonConvert.SerializeObject(_maleConfig, Formatting.Indented));
            }
            using (var w = new StreamWriter($"Configs/Clothes/Female/{_fileName}.json"))
            {
                w.Write(JsonConvert.SerializeObject(_femaleConfig, Formatting.Indented));
            }
        }
    }
}
