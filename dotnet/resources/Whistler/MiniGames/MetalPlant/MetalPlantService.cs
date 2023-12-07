using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Enums;
using Whistler.Jobs.SteelMaking;
using Whistler.NewJobs;
using Whistler.NewJobs.Models;
using Whistler.SDK;

namespace Whistler.MiniGames.MetalPlant
{
    public static class MetalPlantService
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(MetalPlantService));
        public static Job Job { get; private set; }
        private static Random _rnd = new Random();
        private static int _diamondProbability = 1000;
        public static void Init()
        {
            Job = new Job
            {
                Name = "metalplantworker",
                Levels = new List<JobLevel>
                {
                    new JobLevel(1, 0, "loh"),
                    new JobLevel(2, 2000, "normal"),
                    new JobLevel(3, 6000, "profi")
                },
                Condition = (ExtPlayer player) => player.CheckLic(GUI.Documents.Enums.LicenseName.MetalPlantWorker),
                Limit = 50000
            };
            Job.ParseLevels("interfaces/gui/src/configs/gameMetalPlant", "/levels.js");
        }
        public static void WorkBegine(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            if (!player.CheckLic(GUI.Documents.Enums.LicenseName.MetalPlantWorker))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "mg:mp:job:noLic", 3000);
                return;
            }
            //if (!Job.IsOnJub(player))
            //{
            //    Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "mg:mw:job:no", 3000);
            //    return;
            //}
            var allOre = player.Character.Inventory?.GetItemLinksByCondition(i => i.Type == Inventory.Enums.ItemTypes.Ore).Select(i => new { Name = (int)i.Name, Count = i.Count });
            var allCoal = player.Character.Inventory?.GetItemLinksByCondition(i => i.Type == Inventory.Enums.ItemTypes.Coals).Select(i => new { Name = (int)i.Name, Count = i.Count });
            SafeTrigger.ClientEvent(player,"mg:metalplant:game:open", JsonConvert.SerializeObject(allOre), JsonConvert.SerializeObject(allCoal), Job.GetLvlLvl(player));
        }

        internal static void CalculateGameResult(ExtPlayer player, int percent)
        {
            var metals = new List<bool>();
            var lvl = Job.GetLvlLvl(player);
            var metalOnLvl = lvl + 1;
            percent = Math.Min(100, percent);
            var common = 0;
            var perfect = 0;
            bool diamond = false;
            for (int i = 0; i < metalOnLvl; i++)
            {
                if (!diamond && _rnd.Next(0, _diamondProbability) < 1 && percent > 80)
                {
                    diamond = true;
                }
                else if (percent > 99)
                {
                    metals.Add(true);
                    perfect++;
                }
                else
                {
                    percent -= 10;
                    var isPerfect = _rnd.Next(100) < percent;
                    metals.Add(isPerfect);
                    if (isPerfect)
                        perfect++;
                    else
                        common++;
                }
            }
            Job.AddExpiriance(player, common + perfect * 2 + (diamond ? 5 : 0)  );
            SafeTrigger.ClientEvent(player,"mg:metalplant:result", diamond, metals);
            GiveResources(player, perfect, common, diamond);
            if (diamond)
                player.CreatePlayerAction(PersonalEvents.PlayerActions.MakeDiamond, 1);
        }
        private static void GiveResources(ExtPlayer player, int perfect, int common, bool diamond)
        {
            if (player.MetallPlantOre == ItemNames.Invalid)
                return;
            var ore = player.MetallPlantOre;
            player.MetallPlantOre = ItemNames.Invalid;
            var resource = OreMiningSettings.OreResources.GetValueOrDefault(ore);
            if (resource == null)
                return;
            if (perfect > 0)
            {
                var perfItem = Inventory.ItemsFabric.CreateResources(resource.PerfectResource, perfect, false);
                if (!player.Character.Inventory.AddItem(perfItem))
                    Inventory.DropSystem.DropItem(perfItem, player.Position, player.Dimension);
            }
            if (common > 0)
            {
                var commonItem = Inventory.ItemsFabric.CreateResources(resource.CommonResource, common, false);
                if (!player.Character.Inventory.AddItem(commonItem))
                    Inventory.DropSystem.DropItem(commonItem, player.Position, player.Dimension);
            }
            if (diamond)
            {
                var diamondItem = Inventory.ItemsFabric.CreateResources(ItemNames.Diamond, 1, false);
                if (!player.Character.Inventory.AddItem(diamondItem))
                    Inventory.DropSystem.DropItem(diamondItem, player.Position, player.Dimension);
            }
        }
    }
}
