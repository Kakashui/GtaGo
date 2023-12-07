using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.SDK;
using Whistler.Inventory.Enums;
using Whistler.Inventory;
using Whistler.Helpers;
using Whistler.Jobs.SteelMaking.Models;
using Whistler.Inventory.Models;
using Whistler.Entities;
using System.Text;
using System.IO;
using Whistler.MiniGames.MetalPlant;

namespace Whistler.Jobs.SteelMaking
{
    class OreMining : Script
    {
        /// <summary>
        /// Рудные жилы
        /// </summary>
        private static Dictionary<int, OreVein> _oreVein = new Dictionary<int, OreVein>();

        private static int _id = 1;
        private static MetalPlantPoint _metallPlantPoint = null;

        public OreMining()
        {
            /*
            _metallPlantPoint = new MetalPlantPoint(OreMiningSettings.PointMetal);
            var veins = OreMiningSettings.Veins.GetElementsWithRandomProbability(1);
            var offset = new Vector3(0, 0, -3);
            foreach (var veinBlip in OreMiningSettings.OreVeinBlips)
            {
                NAPI.Blip.CreateBlip(654, veinBlip, 1.2F, 47, "Залежи руды", 255, 0, true, 0, NAPI.GlobalDimension);
            }
            foreach (var vein in veins)
            {
                var oreVein = new OreVein(_id++, vein + offset);
                _oreVein.Add(oreVein.Point, oreVein);
            }
            */
        }


        //[Command("giveother")]
        //public static void CMD_GiveDynamit(ExtPlayer player, int id)
        //{
        //    var inventory = player.GetInventory();
        //    var item = ItemsFabric.CreateOther((ItemNames)id, 1, false);
        //    if (item != null)
        //        inventory.AddItem(item);
        //}

        public static bool OnUseDynamite(ExtPlayer player, Other dynamite)
        {
            if (player.OreVeinID < 0)
                return false;
            if (!(_oreVein.GetValueOrDefault(player.OreVeinID)?.Status ?? false))
            {
                return false;
            }
            player.GetInventory().SubItemByName(dynamite.Name, 1, LogAction.Use);
            Vector3 pos = player.Position.Copy();
            SafeTrigger.ClientEventInRange(pos, 200, "OreVein:explosionDynamitTimer", player.Position, 10, player.OreVeinID);
            _oreVein[player.OreVeinID]?.ExplodeDynamite(pos);
            player.CreatePlayerAction(PersonalEvents.PlayerActions.ExplodeDynamite, 1);
            return true;
        }
        public static void OnUseDetector(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player, "OreVein:UseDetector", true);
        }
        public static void OnStopUseDetector(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player, "OreVein:UseDetector", false);
        }

        [RemoteEvent("OreVeid:CreateDrops")]
        public static void RemoteEvent_CreateDrops(ExtPlayer player, int point, string dataPos)
        {
            _oreVein.GetValueOrDefault(point)?.DropOresAndCoals(dataPos);
        }

        //[RemoteEvent("OreVeid:savePoint")]
        //public static void savePoint(ExtPlayer player)
        //{
        //    StreamWriter saveCoords = new StreamWriter("coords.txt", true, Encoding.UTF8);
        //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        //    saveCoords.Write($"new Vector3({player.Position.X}, {player.Position.Y}, {player.Position.Z}), \r\n");
        //    saveCoords.Close();
        //    var oreVein = new OreVein(_id++, player.Position + new Vector3(0, 0, -3));
        //    _oreVein.Add(oreVein.Point, oreVein);
        //}
    }
}
