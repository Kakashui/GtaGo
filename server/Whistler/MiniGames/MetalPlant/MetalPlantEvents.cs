using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Inventory.Enums;
using Whistler.NewJobs.Models;
using Whistler.SDK;

namespace Whistler.MiniGames.MetalPlant
{
    public class MetalPlantEvents: Script
    {

        [ServerEvent(Event.ResourceStart)]
        public void OnStart()
        {
            MetalPlantService.Init();
        }

        [RemoteEvent("game:mplant:start")]
        public void StartGame(PlayerGo player, int ore, int fuel)
        {
            if (!player.Logged()) return;
            var oreExists = player.Character.Inventory.SubItemByName((ItemNames)ore, 1, LogAction.Delete);
            var fuelExists = player.Character.Inventory.SubItemByName((ItemNames)fuel, 1, LogAction.Delete);
            if (oreExists == null || fuelExists == null)
            {
                player.SendError("game:mplant:error:noitem");
                return;
            }
            player.SetData("mg:metalplant:ore", oreExists);
            player.MetallPlantOre = (ItemNames)ore;
            player.TriggerEvent("mg:metalplant:start");
        }
        [RemoteEvent("game:mplant:win")]
        public void WinGame(PlayerGo player, int percent)
        {
            if (!player.Logged()) return;
            MetalPlantService.CalculateGameResult(player, percent);
        }
    }
}
