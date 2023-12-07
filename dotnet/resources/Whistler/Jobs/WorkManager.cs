using GTANetworkAPI;
using System;
using System.Linq;
using System.Collections.Generic;
using Whistler.Businesses;
using Whistler.Core;
using Whistler.SDK;
using Whistler.GUI;
using Newtonsoft.Json;
using Whistler.ClothesCustom;
using Whistler.Docks;
using Whistler.Helpers;
using Whistler.Entities;

namespace Whistler.Jobs
{
    class WorkManager : Script
    {
        private static List<Vector3> JCheckpoints = new List<Vector3>()
        {
            new Vector3(895.0821, -179.3907, 74.68025),//t1
            new Vector3(1967.24, 3780.375, 32.16825),//t2
            new Vector3(1785.574, 4593.743, 37.66298),//ta3
            new Vector3(913.8586, -1273.308, 27.07613),//1
            new Vector3(-1478.16, -519.0229, 34.71669),//
            new Vector3(-137.6891, 6362.365, 31.47063), //
            new Vector3(435.3048, -645.8337, 28.71639), //avtobusi
            new Vector3(632.851, -3014.975, 7.316179), //trai1
            new Vector3(346.3057, 3406.874, 36.50902), //trai2
            new Vector3(-2203.86, 4244.335, 48.23107), //trai3
            new Vector3(-1332.75, 65.47161, 53.48625),//gamkreci
            new Vector3(486.7936, -1295.927, 29.53489), //meaiki
        };
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(WorkManager));
        public static Random rnd = new Random();

        [ServerEvent(Event.ResourceStart)]
        public void onResourceStart()
        {
            try
            {
                NAPI.Blip.CreateBlip(513, JCheckpoints[6], 1, 30, Main.StringToU16("Автобус"), 255, 0, true, 0, 0);
            } 
            catch (Exception e) 
            {
                _logger.WriteError("ResourceStart: " + e.ToString()); 
            }
        }

        // 
        //
        public static List<string> JobStats = new List<string>
        {
            "Электрик",
            "Почтальон",
            "Таксист",
            "Водитель автобуса",
            "Газонокосильщик",
            "Дальнобойщик",
            "Грузчик",
            "Автомеханик",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Электрик",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный",
            "Безработный"
        };

        // REQUIRED REFACTOR //
        public static void load(ExtPlayer player)
        { 
            SafeTrigger.SetData(player, "BUS_ONSTOP", false);
            SafeTrigger.SetData(player, "IS_REQUESTED", false);
            SafeTrigger.SetData(player, "WORKWAY", -1);
            SafeTrigger.SetData(player, "REQUEST", "null");
            SafeTrigger.SetData(player, "IS_IN_ARREST_AREA", false);
            SafeTrigger.SetData(player, "IN_HOSPITAL", false);
            SafeTrigger.SetData(player, "GANGPOINT", -1);
        }
    }
}