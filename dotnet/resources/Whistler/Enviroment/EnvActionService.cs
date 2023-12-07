using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Whistler.Enviroment.Models;
using System.Linq;
using Whistler.SDK;
using Whistler.Helpers;
using Whistler.Entities;

namespace Whistler.Enviroment
{
    public static class EnvActionService
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(EnvActionService));
        private static Dictionary<int, SitConfig> _sitConfig = new Dictionary<int, SitConfig>();
        public static void TakeSitPlace(ExtPlayer player, int model,  Vector3 position, Vector3 rotation)
        {
            var hash = NAPI.Util.GetHashKey($"{model}{Math.Floor(position.X)}{Math.Floor(position.Y)}");
            if (player.HasSharedData("env:data:action:sit") && player.GetSharedData<SitDTO>("env:data:action:sit") != null)
            {
                FreeSitPlace(player);
                return;
            }
            var placePosition = _sitConfig[model].TakePlace(hash);
            if (placePosition < 0)
                Notify.Send(player, NotifyType.Error, NotifyPosition.Center, "Здесь уже кто-то сидит.", 3000);
            else
            {
                SafeTrigger.SetSharedData(player, "env:data:action:sit", new SitDTO(model, position, rotation, placePosition));
                SafeTrigger.SetData(player, "env:sit:place", new SitData(model, hash, placePosition));
                //SafeTrigger.UpdatePosition(player,  position;
                //player.Rotation = rotation;
            }
        }
        public static void FreeSitPlace(ExtPlayer player)
        {
            try
            {
                SafeTrigger.ResetSharedData(player, "env:data:action:sit");
                if (!player.HasData("env:sit:place")) return;
                SitData place = player.GetData<SitData>("env:sit:place");
                if(_sitConfig.ContainsKey(place.Model))
                    _sitConfig[place.Model].FreePlace(place.Hash, place.Place);
                player.ResetData("env:sit:place");
            }
            catch (Exception e)
            {
                _logger.WriteError($"FreeSitPlace:\n{e}");
            }
            
        }
        public static void ParseConfig()
        {
            if (File.Exists("sitpositions.json"))
            {
                using var r = new StreamReader("sitpositions.json");
                _sitConfig = JsonConvert.DeserializeObject<List<SitConfig>>(r.ReadToEnd()).ToDictionary(c => c.Model, c => c);
                if (Directory.Exists("client/configs"))
                {
                    using var w = new StreamWriter("client/configs/seats.js");
                    w.Write($"module.exports = {JsonConvert.SerializeObject(_sitConfig, Formatting.Indented)}");
                }
            }
        }
    }
}
