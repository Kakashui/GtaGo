using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.Enviroment
{
    public class EnvActionEvents//: Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            EnvActionService.ParseConfig();
        }

        [RemoteEvent("env:action:sit:take")]
        public void TakePlace(ExtPlayer player, int model, float posX, float posY, float posZ, float rotZ)
        {
            if (!player.IsLogged()) return;           
            EnvActionService.TakeSitPlace(player, model, new Vector3(posX, posY, posZ), new Vector3(0, 0, rotZ));
        }

        [RemoteEvent("env:action:sit:free")]
        public void FreePlace(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            EnvActionService.FreeSitPlace(player);
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(ExtPlayer player, DisconnectionType type, string reason)
        {
            EnvActionService.FreeSitPlace(player);
        }
    }
}
