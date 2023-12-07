using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.Inventory.Configs.ClothesDetails
{
    internal class Bags
    {
        private static string _fileName = "Bags";
        private static Dictionary<int, List<int>> _maleConfig;
        private static Dictionary<int, List<int>> _femaleConfig;

        static Bags()
        {
            if (File.Exists($"Configs/Clothes/Male/{_fileName}.json"))
            {
                using (var r = new StreamReader($"Configs/Clothes/Male/{_fileName}.json"))
                {
                    _maleConfig = JsonConvert.DeserializeObject<Dictionary<int, List<int>>>(r.ReadToEnd());
                }
                using (var r = new StreamReader($"Configs/Clothes/Female/{_fileName}.json"))
                {
                    _femaleConfig = JsonConvert.DeserializeObject<Dictionary<int, List<int>>>(r.ReadToEnd());
                }
            }
            else
            {
                _maleConfig = new Dictionary<int, List<int>>();
                _femaleConfig = new Dictionary<int, List<int>>();
            }
        }

        public static int GetColor(bool gender, int drawable, int colorIndex)
        {
            var config = gender ? _maleConfig : _femaleConfig;
            if (colorIndex < 0 || !config.ContainsKey(drawable)) return 0;
            if (config[drawable].Count <= colorIndex) return 0;
            return config[drawable][colorIndex];
        }

        public static int GetColor(ExtPlayer player, int drawable, int colorIndex)
        {
            var gender = player.GetGender();
            var config = gender ? _maleConfig : _femaleConfig;
            if (colorIndex < 0 || !config.ContainsKey(drawable)) return 0;
            if (config[drawable].Count <= colorIndex) return 0;
            return config[drawable][colorIndex];
        }

        public static void Add(bool gender, int id, List<int> colors)
        {
            if (_maleConfig == null) _maleConfig = new Dictionary<int, List<int>>();
            if (_femaleConfig == null) _femaleConfig = new Dictionary<int, List<int>>();
            if (gender)
                _maleConfig.Add(id, colors);
            else
                _femaleConfig.Add(id, colors);
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
