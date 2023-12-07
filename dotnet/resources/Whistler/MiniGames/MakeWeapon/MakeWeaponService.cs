using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Core;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.MoneySystem;
using Whistler.NewJobs;
using Whistler.NewJobs.Models;
using Whistler.SDK;

namespace Whistler.MiniGames.MakeWeapon
{
    public static class MakeWeaponService
    {
        public static Job Job { get; set; }        
        public static WeaponsConfig Cofig = new WeaponsConfig();
        public static int GameCost { get; } = 20;

        private static List<Vector3> _detailPoints = new List<Vector3>
        {
            new Vector3(-273.6051, -2662.953, 5.449992),
            new Vector3(-275.4868, -2681.05, 5.449992),
            new Vector3(-282.9623, -2673.147, 5.449993),
            new Vector3(-286.6974, -2676.656, 5.449994),
            new Vector3(-280.5571, -2682.662, 5.449991),
            new Vector3(-1818.811, 2749.557, -4.270128),
            new Vector3(-1826.185, 2757.297, -4.270129),
            new Vector3(-1817.001, 2767.394, -4.270129),
            new Vector3(-1830.12, 2753.73, -4.270129),
            new Vector3(-1823.883, 2747.564, -4.270128)
        };
        private static List<MakeWeaponPoint> _points = new List<MakeWeaponPoint> {
            //ARMY
            new MakeWeaponPoint(new Vector3(-1822.25, 2773.897, -3.27013),new Vector3(0, 0, 45.03246)),
            new MakeWeaponPoint(new Vector3(-1819.069, 2776.967, -3.27013),new Vector3(0, 0, 51.17525)),
            new MakeWeaponPoint(new Vector3(-1815.842, 2780.298, -3.27013),new Vector3(0, 0, 43.19193)),
            new MakeWeaponPoint(new Vector3(-1814.505, 2779.146, -3.270128),new Vector3(0, 0, 232.3449)),
            new MakeWeaponPoint(new Vector3(-1817.733, 2776.009, -3.270128),new Vector3(0, 0, 226.8274)),
            new MakeWeaponPoint(new Vector3(-1816.202, 2774.224, -3.27013),new Vector3(0, 0, 52.33448)),
            new MakeWeaponPoint(new Vector3(-1812.946, 2777.526, -3.270129),new Vector3(0, 0, 44.39213)),
            new MakeWeaponPoint(new Vector3(-1811.827, 2776.491, -3.270128),new Vector3(0, 0, 222.748)),
            new MakeWeaponPoint(new Vector3(-1815.026, 2773.337, -3.27013),new Vector3(0, 0, 222.6923)),
            new MakeWeaponPoint(new Vector3(-1813.583, 2771.528, -3.270129),new Vector3(0, 0, 47.62411)),
            new MakeWeaponPoint(new Vector3(-1810.3, 2774.763, -3.270129),new Vector3(0, 0, 42.68639)),
            new MakeWeaponPoint(new Vector3(-1809.257, 2773.917, -3.270128),new Vector3(0, 0, 222.9556)),
            new MakeWeaponPoint(new Vector3(-1812.496, 2770.703, -3.270128),new Vector3(0, 0, 224.6057)),
            new MakeWeaponPoint(new Vector3(-1813.445, 2744.901, -3.270129),new Vector3(0, 0, 232.253)),
            new MakeWeaponPoint(new Vector3(-1809.795, 2748.24, -3.27013),new Vector3(0, 0, 229.1471)),
            new MakeWeaponPoint(new Vector3(-1806.337, 2751.739, -3.270129),new Vector3(0, 0, 225.925)),
            new MakeWeaponPoint(new Vector3(-1807.293, 2752.337, -3.27013),new Vector3(0, 0, 47.27987)),
            new MakeWeaponPoint(new Vector3(-1810.786, 2748.94, -3.270128),new Vector3(0, 0, 51.2005)),
            new MakeWeaponPoint(new Vector3(-1812.328, 2750.651, -3.270128),new Vector3(0, 0, 229.8306)),
            new MakeWeaponPoint(new Vector3(-1808.773, 2754.198, -3.270129),new Vector3(0, 0, 231.3692)),
            new MakeWeaponPoint(new Vector3(-1810.144, 2755.172, -3.27013),new Vector3(0, 0, 44.32594)),
            new MakeWeaponPoint(new Vector3(-1813.641, 2751.693, -3.270129),new Vector3(0, 0, 48.61074)),
            new MakeWeaponPoint(new Vector3(-1815.224, 2753.445, -3.270128),new Vector3(0, 0, 224.3909)),
            new MakeWeaponPoint(new Vector3(-1811.617, 2757.001, -3.270129),new Vector3(0, 0, 225.728)),

            //COMMON
            new MakeWeaponPoint(new Vector3(-268.9918, -2659.681, 6.449988),new Vector3(0, 0, 223.1562)),
            new MakeWeaponPoint(new Vector3(-265.7703, -2656.437, 6.449989),new Vector3(0, 0, 231.8988)),
            new MakeWeaponPoint(new Vector3(-266.937, -2655.603, 6.449992),new Vector3(0, 0, 50.48519)),
            new MakeWeaponPoint(new Vector3(-270.1088, -2658.758, 6.449992),new Vector3(0, 0, 52.42001)),
            new MakeWeaponPoint(new Vector3(-271.7038, -2657.16, 6.449991),new Vector3(0, 0, 222.6722)),
            new MakeWeaponPoint(new Vector3(-268.4362, -2653.868, 6.449992),new Vector3(0, 0, 225.651)),
            new MakeWeaponPoint(new Vector3(-269.5333, -2652.871, 6.449992),new Vector3(0, 0, 54.05857)),
            new MakeWeaponPoint(new Vector3(-272.7672, -2656.089, 6.449995),new Vector3(0, 0, 45.60471)),
            new MakeWeaponPoint(new Vector3(-274.5078, -2654.539, 6.449991),new Vector3(0, 0, 239.097)),
            new MakeWeaponPoint(new Vector3(-271.0728, -2651.215, 6.449991),new Vector3(0, 0, 228.8764)),
            new MakeWeaponPoint(new Vector3(-272.3667, -2650.133, 6.449991),new Vector3(0, 0, 49.46016)),
            new MakeWeaponPoint(new Vector3(-275.6678, -2653.292, 6.449991),new Vector3(0, 0, 47.51171)),
            new MakeWeaponPoint(new Vector3(-278.7645, -2656.452, 6.449993),new Vector3(0, 0, 40.26815)),
            new MakeWeaponPoint(new Vector3(-288.9283, -2666.997, 6.449991),new Vector3(0, 0, 44.12748)),
            new MakeWeaponPoint(new Vector3(-268.1941, -2673.376, 6.449992),new Vector3(0, 0, 225.1478)),
            new MakeWeaponPoint(new Vector3(-271.7127, -2676.902, 6.449992),new Vector3(0, 0, 222.2761)),
            new MakeWeaponPoint(new Vector3(-270.2124, -2678.639, 6.449992), new Vector3(0, 0, 48.99709)),
            new MakeWeaponPoint(new Vector3(-266.6305, -2675.054, 6.44999),new Vector3(0, 0, 46.84859)),
            new MakeWeaponPoint(new Vector3(-265.3049, -2676.118, 6.449993),new Vector3(0, 0, 216.7008)),
            new MakeWeaponPoint(new Vector3(-269.0096, -2679.76, 6.449993),new Vector3(0, 0, 227.3602)),
            new MakeWeaponPoint(new Vector3(-267.3838, -2681.397, 6.449992),new Vector3(0, 0, 50.78462)),
            new MakeWeaponPoint(new Vector3(-263.8868, -2677.901, 6.449992),new Vector3(0, 0, 43.58507)),
            new MakeWeaponPoint(new Vector3(-262.9365, -2678.674, 6.449994),new Vector3(0, 0, 228.8264)),
            new MakeWeaponPoint(new Vector3(-266.4433, -2682.167, 6.449992),new Vector3(0, 0, 227.5098)),
            new MakeWeaponPoint(new Vector3(-270.062, -2685.591, 6.44999),new Vector3(0, 0, 226.8829)),
            new MakeWeaponPoint(new Vector3(-279.9843, -2668.598, 6.449994),new Vector3(0, 0, 44.97728)),
            new MakeWeaponPoint(new Vector3(-276.5423, -2665.202, 6.44999),new Vector3(0, 0, 47.30485)),
            new MakeWeaponPoint(new Vector3(-275.5649, -2665.992, 6.449989),new Vector3(0, 0, 229.5356)),
            new MakeWeaponPoint(new Vector3(-278.1043, -2663.531, 6.449991),new Vector3(0, 0, 224.5991)),
            new MakeWeaponPoint(new Vector3(-281.5169, -2666.961, 6.449992),new Vector3(0, 0, 226.3482)),
            new MakeWeaponPoint(new Vector3(-286.153, -2669.578, 6.449993),new Vector3(0, 0, 43.89157)),
            new MakeWeaponPoint(new Vector3(-282.6625, -2666.101, 6.44999),new Vector3(0, 0, 39.24925)),
            new MakeWeaponPoint(new Vector3(-279.1187, -2662.66, 6.44999),new Vector3(0, 0, 54.23692)),
            new MakeWeaponPoint(new Vector3(-280.5818, -2660.862, 6.449992),new Vector3(0, 0, 222.552)),
            new MakeWeaponPoint(new Vector3(-284.0948, -2664.319, 6.449992),new Vector3(0, 0, 223.0404)),
            new MakeWeaponPoint(new Vector3(-287.705, -2667.942, 6.449993),new Vector3(0, 0, 228.9038)),
            new MakeWeaponPoint(new Vector3(-288.9229, -2667.008, 6.449993),new Vector3(0, 0, 41.89435)),
            new MakeWeaponPoint(new Vector3(-285.3195, -2663.442, 6.44999),new Vector3(0, 0, 37.25936)),
            new MakeWeaponPoint(new Vector3(-281.7429, -2660.019, 6.44999),new Vector3(0, 0, 45.17112)),
            new MakeWeaponPoint(new Vector3(-293.4823, -2684.037, 6.449992),new Vector3(0, 0, 225.037)),
            new MakeWeaponPoint(new Vector3(-294.6765, -2683.189, 6.449991),new Vector3(0, 0, 46.29343)),
            new MakeWeaponPoint(new Vector3(-290.9806, -2679.362, 6.44999),new Vector3(0, 0, 47.36541)),
            new MakeWeaponPoint(new Vector3(-292.4825, -2677.628, 6.449991),new Vector3(0, 0, 228.5832)),
            new MakeWeaponPoint(new Vector3(-296.1851, -2681.383, 6.449993),new Vector3(0, 0, 226.8049)),
            new MakeWeaponPoint(new Vector3(-297.1902, -2680.592, 6.44999),new Vector3(0, 0, 43.15269)),
            new MakeWeaponPoint(new Vector3(-293.4711, -2676.862, 6.44999),new Vector3(0, 0, 46.64545)),
            new MakeWeaponPoint(new Vector3(-289.781, -2673.188, 6.449989),new Vector3(0, 0, 47.49586)),
            new MakeWeaponPoint(new Vector3(-291.2447, -2671.365, 6.449988),new Vector3(0, 0, 225.0721)),
            new MakeWeaponPoint(new Vector3(-294.9717, -2675.136, 6.449993),new Vector3(0, 0, 230.4317)),
            new MakeWeaponPoint(new Vector3(-298.6909, -2678.868, 6.449992),new Vector3(0, 0, 224.7815)),
            new MakeWeaponPoint(new Vector3(-299.9137, -2677.984, 6.449991),new Vector3(0, 0, 51.38364)),
            new MakeWeaponPoint(new Vector3(-296.1285, -2674.246, 6.449989),new Vector3(0, 0, 49.05798)),
            new MakeWeaponPoint(new Vector3(-292.3743, -2670.448, 6.449991),new Vector3(0, 0, 46.90242)),
            new MakeWeaponPoint(new Vector3(-287.768, -2690.114, 6.449994),new Vector3(0, 0, 53.76018)),
            new MakeWeaponPoint(new Vector3(-286.8699, -2691.083, 6.449993),new Vector3(0, 0, 229.0693)),
            new MakeWeaponPoint(new Vector3(-283.1309, -2687.333, 6.449991),new Vector3(0, 0, 228.5458)),
            new MakeWeaponPoint(new Vector3(-281.5447, -2689.056, 6.449992),new Vector3(0, 0, 44.29179)),
            new MakeWeaponPoint(new Vector3(-285.219, -2692.696, 6.449992),new Vector3(0, 0, 40.57556)),
            new MakeWeaponPoint(new Vector3(-284.0129, -2693.844, 6.449992),new Vector3(0, 0, 230.2039)),
            new MakeWeaponPoint(new Vector3(-280.1934, -2690.237, 6.449991),new Vector3(0, 0, 225.985)),
            new MakeWeaponPoint(new Vector3(-276.3623, -2686.261, 6.44999),new Vector3(0, 0, 228.1553)),
            new MakeWeaponPoint(new Vector3(-273.4403, -2689.148, 6.449992),new Vector3(0, 0, 229.4991)),
            new MakeWeaponPoint(new Vector3(-274.7357, -2688.085, 6.449992),new Vector3(0, 0, 57.75735)),
            new MakeWeaponPoint(new Vector3(-277.3316, -2693.043, 6.449992),new Vector3(0, 0, 222.679)),
            new MakeWeaponPoint(new Vector3(-278.6414, -2691.903, 6.449992),new Vector3(0, 0, 51.4502)),
            new MakeWeaponPoint(new Vector3(-282.4371, -2695.615, 6.449991),new Vector3(0, 0, 43.53841)),
            new MakeWeaponPoint(new Vector3(-281.1608, -2696.75, 6.44999),new Vector3(0, 0, 228.2905)),
            //new MakeWeaponPoint(,),
            //new MakeWeaponPoint(,),
            //new MakeWeaponPoint(,),
            //new MakeWeaponPoint(,),
            //new MakeWeaponPoint(,),
            //new MakeWeaponPoint(,),
            //new MakeWeaponPoint(,),
            //new MakeWeaponPoint(,),
            //new MakeWeaponPoint(,),
            //new MakeWeaponPoint(,),
            //new MakeWeaponPoint(,),
            //new MakeWeaponPoint(,)
        };

        internal static void GetDetail(ExtPlayer player)
        {
            if (Job.IsOnJub(player))
            {
                SafeTrigger.ClientEvent(player,"mw:game:detail:get");
            }
        }

        internal static void SetLimit(ExtPlayer player, int limit)
        {
            var worker = player.GetWorker();
            worker.TotalInPayday = limit;
            Notify.Send(player, NotifyType.Info, NotifyPosition.Center, "limit:set".Translate(limit), 3000);
        }

        private static List<QuestPed> _peds = new List<QuestPed>();
        private static Blip _blip;


        public static void HandleGameResult(ExtPlayer player, int id, int result, int bonus)
        {
            var worker = player.GetWorker();
            if (result < 1)
            {
                if (Wallet.MoneySub(player.Character, GameCost, "Сборка оружия"))
                    SafeTrigger.ClientEvent(player,"mg:makeweapon:stage:next", worker.Expiriance, 0);
                else
                    QuitJob(player);
            }
            else
            {
                switch (Job.GetLvlLvl(player))
                {
                    case 1:
                        result = Math.Min(result, 2);
                        break;
                    case 2:
                        result = Math.Min(result, 3);
                        break;
                    case 3:
                        result = Math.Min(result, 4);
                        break;
                    default:
                        result = Math.Min(result, 2);
                        break;
                }
                bonus = Math.Min(result, bonus);
                var total = (int)Math.Floor(Cofig[id].Price * result * .25);
                if (player.Character.FractionID == 14)
                    bonus += 3;

                if (bonus > 0)
                    total += (int)Math.Floor(total * bonus * .1);
                total -= GameCost;
                total = NewDonateShop.DonateService.UseJobCoef(player, total, player.Character.IsPrimeActive());
                Wallet.MoneyAdd(player.Character, total, "Money_MakeWeaponGame");
                worker.TotalInPayday += total;
                
                //if (Job.IsLimit(player, worker))
                //{
                //    SafeTrigger.ClientEvent(player,"mg:makeweapon:quit");
                //    Notify.Send(player, NotifyType.Info, NotifyPosition.Center, "qp:mg:mw:17", 3000);
                //}                    
                //else
                SafeTrigger.ClientEvent(player,"mg:makeweapon:stage:next", worker.AddExp(Job), total);
            }
        }

        public static void Init()
        {
            Cofig.Parse();
            foreach (var point in _detailPoints)
            {
                var shape = InteractShape.Create(point, 1, 2);
                shape.AddDefaultMarker();
                shape.AddInteraction(MakeWeaponService.GetDetail, "interact_46");
            }
            Job = new Job
            {
                Name = "weaponmaker",
                Levels = new List<JobLevel>
                {
                    new JobLevel(1, 0, "loh"),
                    new JobLevel(2, 2000, "normal"),
                    new JobLevel(3, 6000, "profi")
                },
                Condition = (ExtPlayer player) => player.Character.FractionID == 14 || player.CheckLic(GUI.Documents.Enums.LicenseName.MakingWeapon),
                Limit = 50000
            };
            Job.ParseLevels("interfaces/gui/src/configs/gameMakeWeapon", "/levels.js");

            _peds.Add(new QuestPed(PedHash.Blackops01SMY, new Vector3(-1808.241, 2767.128, -3.270129), "Alan Drochello", "qp:mweapon:1", 33.65149f, 0, 2));            
            _peds.Add(new QuestPed(PedHash.Blackops03SMY, new Vector3(-264.8663, -2663.261, 6.449992), "Sauka Konchet", "qp:mweapon:1", 31.91754f, 0, 2));
            _peds.ForEach(p=>p.PlayerInteracted += GetOnJub);
            _blip = NAPI.Blip.CreateBlip(150, new Vector3(-265.8626, -2661.387, 6.449992), 1, 2, "Оружейный завод", shortRange: true);
        }

        private static void GetOnJub(ExtPlayer player, QuestPed ped)
        {
            var result = Job.CanGetOnJob(player);
            DialogPage startPage;

            var info = new DialogPage("qp:mg:mw:1", 
                ped.Name, 
                ped.Role)
                .AddCloseAnswer("qp:mg:mw:2");

            switch (result)
            {
                case NewJobs.Enums.TryGetJobResults.OnOtherJob:
                    startPage = new DialogPage("qp:mg:mw:3", ped.Name, ped.Role)
                        .AddCloseAnswer("qp:mg:mw:4");
                    break;
                case NewJobs.Enums.TryGetJobResults.Limit:
                    startPage = new DialogPage("qp:mg:mw:16", ped.Name, ped.Role)
                        .AddCloseAnswer("qp:mg:mw:4");
                    break;
                case NewJobs.Enums.TryGetJobResults.BadCondition: 
                    startPage = new DialogPage("qp:mg:mw:5", ped.Name, ped.Role)
                        .AddCloseAnswer("qp:mg:mw:6");
                    break;
                case NewJobs.Enums.TryGetJobResults.Success:
                    startPage = new DialogPage("qp:mg:mw:7", ped.Name, ped.Role)
                        .AddAnswer("qp:mg:mw:8", Job.Invite)
                        .AddAnswer("qp:mg:mw:9", info)
                        .AddCloseAnswer("qp:mg:mw:10");
                    break;
                case NewJobs.Enums.TryGetJobResults.Already: 
                    startPage = new DialogPage("qp:mg:mw:11", ped.Name, ped.Role)
                        .AddAnswer("qp:mg:mw:12", Job.Leave)
                        .AddCloseAnswer("qp:mg:mw:13");
                    break;
                default:
                    startPage = new DialogPage("qp:mg:mw:14", ped.Name, ped.Role)
                        .AddCloseAnswer("qp:mg:mw:15");
                    break;
            }
            startPage.OpenForPlayer(player);          
        }

        public static void QuitJob(ExtPlayer player)
        {
            foreach (var point in _points.Where(p => p.CurrentWorker == player))
                point.CurrentWorker = null;

            SafeTrigger.ClientEvent(player,"mg:makeweapon:quit");
        }
    }
}
