using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Gangs.WeedFarm
{
    class WeedFarmEvents: Script
    {

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            WeedFarmService.Init();
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnect(ExtPlayer player, DisconnectionType type, string reason)
        {
            WeedFarmService.DisconnectHandle(player);
        }

        [RemoteEvent("weedfarm:comp:path:set")]
        public void SetPathComponent(ExtPlayer player, double x, double y)
        {
            SafeTrigger.SetData(player, "weedfarm:comp:path", new Vector3(x, y, 0));
        }

        [RemoteEvent("weedfarm:sort:action:request")]
        public void RequestPackAction(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            WeedFarmService.RequestPackAction(player);
        }

        [RemoteEvent("weedfarm:instance:check")]
        public void BadFarmDimension(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            WeedFarmService.LoadFarmOnStart(player);
        }

        [RemoteEvent("weedfarm:sort:cancel")]
        public void CancelSortJob(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            WeedFarmService.CancelSortJob(player);
        }

        [RemoteEvent("weedfarm:delivery:begine")]
        public void DeliveryBegine(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            WeedFarmService.BegineWeedDeliveryJob(player);
        }

        [RemoteEvent("weedfarm:delivery:end")]
        public void DeliveryEnd(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            WeedFarmService.EndWeedDeliveryJob(player);
        }

        [RemoteEvent("weedfarm:delivery:action")]
        public void DeliveryAction(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            WeedFarmService.ActionWeedDeliveryJob(player);
        }

        [Command("setwfowner")]
        public void SetWeedFarmOwner(ExtPlayer player, int farmId, int gangId)
        {
            if (!Group.CanUseAdminCommand(player, "setwfowner")) return;
            WeedFarmService.SetNewFarmOwner(player, farmId, gangId);
        }

        [Command("startwfbattle")]
        public void StartWeedFarmBattle(ExtPlayer player, int farmId)
        {
            if (!Group.CanUseAdminCommand(player, "startwfbattle")) return;
            WeedFarmService.StartFarmBattleByAdmin(player, farmId);
        }

        [Command("respfarmcar")]
        public void RespawnFarmCar(ExtPlayer player,  int farmId)
        {
            if (!Manager.CanUseCommand(player, "respfarmcar")) return;

            WeedFarmService.RespawnFarmCar(player, farmId);
        }
    }
}