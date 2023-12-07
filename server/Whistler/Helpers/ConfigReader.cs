using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace Whistler.Helpers
{
    static class ConfigReader
    {
        public static void CreateDirectories()
        {
            if (!Directory.Exists("ParsedConfigs")) Directory.CreateDirectory("ParsedConfigs");
            if (!Directory.Exists("Configs")) Directory.CreateDirectory("Configs");
            if (!Directory.Exists("Configs/Clothes")) Directory.CreateDirectory("Configs/Clothes");
            if (!Directory.Exists("Configs/Clothes/Male")) Directory.CreateDirectory("Configs/Clothes/Male");
            if (!Directory.Exists("Configs/Clothes/Female")) Directory.CreateDirectory("Configs/Clothes/Female");
        }
        public static T GetConfigFromFile<T>(string fileName)
        {
            var file = $"Configs/{fileName}.json";
            if (File.Exists(file))
            {
                using (var r = new StreamReader(file))
                {
                    var result = JsonConvert.DeserializeObject<T>(r.ReadToEnd());
                    return result;
                }
            }
            else return default;
        }
    }
}
