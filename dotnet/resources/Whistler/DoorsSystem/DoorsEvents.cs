using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.DoorsSystem
{
    class DoorsEvent: Script
    {
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            NAPI.World.DeleteWorldProp(-1148826190, new Vector3(82.38156, -1390.476, 29.52609), 30f);
            NAPI.World.DeleteWorldProp(868499217, new Vector3(82.38156, -1390.752, 29.52609), 30f);
            NAPI.World.DeleteWorldProp(-8873588, new Vector3(842.7685, -1024.539, 28.34478), 30f);
            NAPI.World.DeleteWorldProp(97297972, new Vector3(845.3694, -1024.539, 28.34478), 30f);
            NAPI.World.DeleteWorldProp(-8873588, new Vector3(-662.6415, -944.3256, 21.97915), 30f);
            NAPI.World.DeleteWorldProp(97297972, new Vector3(-665.2424, -944.3256, 21.97915), 30f);
            NAPI.World.DeleteWorldProp(-8873588, new Vector3(810.5769, -2148.27, 29.76892), 30f);
            NAPI.World.DeleteWorldProp(97297972, new Vector3(813.1779, -2148.27, 29.76892), 30f);
            NAPI.World.DeleteWorldProp(-8873588, new Vector3(18.572, -1115.495, 29.94694), 30f);
            NAPI.World.DeleteWorldProp(97297972, new Vector3(16.12787, -1114.606, 29.94694), 30f);
            NAPI.World.DeleteWorldProp(-8873588, new Vector3(243.8379, -46.52324, 70.09098), 30f);
            NAPI.World.DeleteWorldProp(97297972, new Vector3(244.7275, -44.07911, 70.09098), 30f);
            NAPI.World.DeleteWorldProp(-1922281023, new Vector3(-715.6154, -157.2561, 37.67493), 30f);
            NAPI.World.DeleteWorldProp(-1922281023, new Vector3(-716.6755, -155.42, 37.67493), 30f);
            NAPI.World.DeleteWorldProp(-1922281023, new Vector3(-1456.201, -233.3682, 50.05648), 30f);
            NAPI.World.DeleteWorldProp(-1922281023, new Vector3(-1454.782, -231.7927, 50.05649), 30f);
            NAPI.World.DeleteWorldProp(-1922281023, new Vector3(-156.439, -304.4294, 39.99308), 30f);
            NAPI.World.DeleteWorldProp(-1922281023, new Vector3(-157.1293, -306.4341, 39.99308), 30f);
            NAPI.World.DeleteWorldProp(1780022985, new Vector3(-1201.435, -776.8566, 17.99184), 30f);
            NAPI.World.DeleteWorldProp(1780022985, new Vector3(127.8201, -211.8274, 55.22751), 30f);
            NAPI.World.DeleteWorldProp(1780022985, new Vector3(617.2458, 2751.022, 42.75777), 30f);
            NAPI.World.DeleteWorldProp(1780022985, new Vector3(-3167.75, 1055.536, 21.53288), 30f); //<-
            NAPI.World.DeleteWorldProp(145369505, new Vector3(-822.4442, -188.3924, 37.81895), 30f);
            NAPI.World.DeleteWorldProp(-1663512092, new Vector3(-823.2001, -187.0831, 37.81895), 30f);
            NAPI.World.DeleteWorldProp(-1844444717, new Vector3(-29.86917, -148.1571, 57.22648), 30f);
            NAPI.World.DeleteWorldProp(-1844444717, new Vector3(1932.952, 3725.154, 32.9944), 30f);
            NAPI.World.DeleteWorldProp(1417577297, new Vector3(-59.89302, -1092.952, 26.88362), 30f);//autosalon ->
            NAPI.World.DeleteWorldProp(2059227086, new Vector3(-39.13366, -1108.218, 26.7198), 30f);
            NAPI.World.DeleteWorldProp(1417577297, new Vector3(-60.54582, -1094.749, 26.88872), 30f);
            NAPI.World.DeleteWorldProp(2059227086, new Vector3(-59.89302, -1092.952, 26.88362), 30f);//<-
            NAPI.World.DeleteWorldProp(1765048490, new Vector3(1855.685, 3683.93, 34.59282), 30f);
            NAPI.World.DeleteWorldProp(543652229, new Vector3(321.8085, 178.3599, 103.6782), 30f); //vinewood tattoo
            NAPI.World.DeleteWorldProp(868499217, new Vector3(-818.7643, -1079.545, 11.47806), 30f);
            NAPI.World.DeleteWorldProp(3146141106, new Vector3(-816.7932, -1078.406, 11.47806), 30f);
            NAPI.World.DeleteWorldProp(543652229, new Vector3(-1155.454, -1424.008, 5.046147), 30f); //vespuchi tattoo
            NAPI.World.DeleteWorldProp(543652229, new Vector3(1321.286, -1650.597, 52.36629), 30f); //elburro tattoo
            NAPI.World.DeleteWorldProp(NAPI.Util.GetHashKey("prop_lrggate_02"), new Vector3(-857.4845, 18.12612, 44.4434), 30f); //elburro tattoo
            NAPI.World.DeleteWorldProp(NAPI.Util.GetHashKey("tr_prop_tr_gate_r_01a"), new Vector3(-2148.653, 1110.646, -23.5492), 30f); //elburro tattoo
            NAPI.World.DeleteWorldProp(NAPI.Util.GetHashKey("tr_prop_tr_gate_r_01a"), new Vector3(-2148.653, 1110.646, 29.48058), 30f); //elburro tattoo
            NAPI.World.DeleteWorldProp(NAPI.Util.GetHashKey("tr_prop_tr_gate_l_01a"), new Vector3(-2148.653, 1101.464, -23.5492), 30f); //elburro tattoo
            NAPI.World.DeleteWorldProp(NAPI.Util.GetHashKey("tr_prop_tr_gate_l_01a"), new Vector3(-2148.653, 1101.464, 29.48058), 30f); //elburro tattoo

            DoorsService.LoadAccess();
        }

        [Command("parsedoors")]
        public void ParseDoor(ExtPlayer player)
        {
            if (!Group.CanUseAdminCommand(player, "parsedoors")) return;
            DoorsService.ParseDoorsConfigs();
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "Parcing compleete", 3000);
        }

        [RemoteEvent("doors:action:state")]
        public void SetDoorState(ExtPlayer player, int uuid, bool state)
        {
            if (!player.IsLogged()) return;
            DoorsService.SetDoorState(player, uuid, state);
        }

        [RemoteEvent("doors:access:add")]
        public void AddDoorAccess(ExtPlayer player, int uuid, string access)
        {
            if (!Group.CanUseAdminCommand(player, "dooraccess")) return;
            DoorsService.AddDoorAccess(player, uuid, access);
        }

        [RemoteEvent("doors:access:check")]
        public void CheckDoorAccess(ExtPlayer player, int uuid)
        {
            if (!Group.CanUseAdminCommand(player, "dooraccess")) return;
            DoorsService.CheckDoorAccess(player, uuid);
        }
    }
}
