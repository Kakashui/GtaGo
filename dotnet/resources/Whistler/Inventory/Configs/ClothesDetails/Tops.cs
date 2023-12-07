using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Enums;

namespace Whistler.Inventory.Configs.ClothesDetails
{
    public class Tops
    {
        private class Top
        {
            public Top(ClothesTopTypes type, List<int> colors, int armor)
            {
                Type = type;
                Colors = colors;
                Armor = armor;
            }
            public Top() { }

            public ClothesTopTypes Type { get; set; }
            public List<int> Colors { get; set; }
            public int Armor { get; set; }
        }
        private static string _fileName = "Tops";
        static Tops() {
            if (File.Exists($"Configs/Clothes/Male/{_fileName}.json")) {
                using (var r = new StreamReader($"Configs/Clothes/Male/{_fileName}.json"))
                {
                    _maleConfig = JsonConvert.DeserializeObject<Dictionary<int, Top>>(r.ReadToEnd());
                }
                using (var r = new StreamReader($"Configs/Clothes/Female/{_fileName}.json"))
                {
                    _femaleConfig = JsonConvert.DeserializeObject<Dictionary<int, Top>>(r.ReadToEnd());
                }
            }
            else
            {
                _maleConfig = new Dictionary<int, Top>();
                _femaleConfig = new Dictionary<int, Top>();
            }
            
        }  

        private static Dictionary<int, Top> _maleConfig;
        private static Dictionary<int, Top> _femaleConfig;

        public static ClothesTopTypes GetType(ExtPlayer player, int drawableID)
        {
            var gender = player.GetGender();
            var config = gender ? _maleConfig : _femaleConfig;
            return config[drawableID].Type;
        }

        public static ClothesTopTypes GetType(bool gender, int drawable)
        {
            var config = gender ? _maleConfig : _femaleConfig; 
            if(!config.ContainsKey(drawable)) return ClothesTopTypes.Invalid;
            return config[drawable].Type;
        }

        public static int GetColor(bool gender, int drawable, int colorIndex)
        {
            var config = gender ? _maleConfig : _femaleConfig;
            if(colorIndex < 0 || !config.ContainsKey(drawable)) return 0;
            if(config[drawable].Colors.Count <= colorIndex) return 0;
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

        public static void Add(bool gender, int id, int type, List<int> colors, int armor)
        {
            if (_maleConfig == null) _maleConfig = new Dictionary<int, Top>();
            if (_femaleConfig == null) _femaleConfig = new Dictionary<int, Top>();
            var convert = (ClothesTopTypes)(type + 1);
         
            if (gender)
                _maleConfig.Add(id, new Top(convert, colors, armor));
            else
                _femaleConfig.Add(id, new Top(convert, colors, armor));
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
