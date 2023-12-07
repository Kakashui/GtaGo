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
    public class UnderTorsos
    {
        private class UnderTors
        {
            public UnderTors(int torso, Dictionary<int, int> glovesTorso)
            {
                Torso = torso;
                GlovesTorso = glovesTorso;
            }
            public UnderTors() { }

            public int Torso { get; set; }
            public Dictionary<int, int> GlovesTorso { get; set; }
        }

        private static string _fileName = "UnderTorsos";
        static UnderTorsos() {
            if (File.Exists($"Configs/Clothes/Male/{_fileName}.json")) {
                using (var r = new StreamReader($"Configs/Clothes/Male/{_fileName}.json"))
                {
                    _maleConfig = JsonConvert.DeserializeObject<Dictionary<int, UnderTors>>(r.ReadToEnd());
                }
                using (var r = new StreamReader($"Configs/Clothes/Female/{_fileName}.json"))
                {
                    _femaleConfig = JsonConvert.DeserializeObject<Dictionary<int, UnderTors>>(r.ReadToEnd());
                }
            }
            else
            {
                _maleConfig = new Dictionary<int, UnderTors>();
                _femaleConfig = new Dictionary<int, UnderTors>();
            }
            
        }  

        private static Dictionary<int, UnderTors> _maleConfig;
        private static Dictionary<int, UnderTors> _femaleConfig;

        public static int GetTorso(ExtPlayer player, int topDrawable)
        {
            var gender = player.GetGender();
            var equip = player.GetEquip();
            var config = gender ? _maleConfig : _femaleConfig;
            if (!config.ContainsKey(topDrawable)) return -1;

            var torso = config[topDrawable].Torso;
            if (equip.Clothes[ClothesSlots.Gloves] != null && config[topDrawable].GlovesTorso.ContainsKey(equip.Clothes[ClothesSlots.Gloves].Drawable))
                torso = config[topDrawable].GlovesTorso[equip.Clothes[ClothesSlots.Gloves].Drawable];            
            return torso;
        }

        public static void Add(bool gender, int top, int tors, Dictionary<int, int> gloves)
        {
            if (_maleConfig == null) _maleConfig = new Dictionary<int, UnderTors>();
            if (_femaleConfig == null) _femaleConfig = new Dictionary<int, UnderTors>();
           
         
            if (gender)
                _maleConfig.Add(top, new UnderTors(tors, gloves));
            else
                _femaleConfig.Add(top, new UnderTors(tors, gloves));
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
