using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core;
using Whistler.Entities;
using Whistler.MP.RoyalBattle.Configs;
using Whistler.MP.RoyalBattle.Models;

namespace Whistler.MP.RoyalBattle
{
    class RoyalBattleManager : Script
    {
        [Command("createbattle")]
        public static void CreateBattle(PlayerGo player)
        {
            if (!Group.CanUseAdminCommand(player, "createbattle"))
                return;
            RoyalBattleService.CreateBattle();
        }  
        [Command("stopbattle")]
        public static void StopBattle(PlayerGo player, int minutes)
        {
            if (!Group.CanUseAdminCommand(player, "stopbattle")) //
                return;
            RoyalBattleService.StopBattle(player, minutes);
        }        


        [RemoteEvent("royalBattle:registerForBattle")]
        public static void RegisterBattle(PlayerGo player)
        {
            RoyalBattleService.RegisterForBattle(player);
        }     
        [RemoteEvent("royalBattle:searchStats")]
        public static void SearchStats(PlayerGo player, string name)
        {
            RoyalBattleService.SearchPlayerInStats(player, name);
        }

        private static Blip _blip;
        public RoyalBattleManager()
        {
            foreach (var point in Configurations.EnterPosition)
            {
                InteractShape.Create(point, 1, 2, 0)
                    .AddInteraction(RoyalBattleService.InterractOpenBattleMenu, "interact_40")
                    .AddInteraction(RoyalBattleService.InterractOpenBattleStatsMenu, "interact_39", Key.VK_I);
            }
            ColShape shape = NAPI.ColShape.CreateSphereColShape(Configurations.CenterRegisterZone, Configurations.RadiusRegisterZone, 0);
            shape.OnEntityExitColShape += (c,p) => RoyalBattleService.OnExitRegisterZone(p as PlayerGo);
            _blip = NAPI.Blip.CreateBlip(94, Configurations.ExitPosition, 1, 52, "Headhunting", 255, 0, true, 0, 0);
        }
    }
}
