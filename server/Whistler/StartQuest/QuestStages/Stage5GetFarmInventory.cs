using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.Fractions.Models;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Jobs.Farm;
using Whistler.Jobs.Farm.Configs;
using Whistler.MoneySystem;

namespace Whistler.StartQuest.QuestStages
{
    class Stage5GetFarmInventory : BaseStage
    {
        int _minApples = 10;

        public Stage5GetFarmInventory()
        {
            StageName = StartQuestNames.Stage5GetFarmInventory;
            var pedFarm = new QuestPed(StartQuestSettings.PedsFarm);
            pedFarm.PlayerInteracted += PedFarm_PlayerInteracted;
        }

        private void PedFarm_PlayerInteracted(PlayerGo player, QuestPed ped)
        {
            var level = player.Character.QuestStage;
            var faqSeed = new DialogPage("startQuest_27",
                ped.Name,
                ped.Role)
                .AddCloseAnswer("startQuest_10");
            DialogPage startPage;
            switch (level)
            {
                case StartQuestNames.Stage5GetFarmInventory:
                    startPage = new DialogPage("startQuest_23",
                        ped.Name,
                        ped.Role)
                        .AddAnswer("startQuest_10", PedFarm_CallBack);
                    break;
                case StartQuestNames.Stage6WorkInFarm:
                    var inventory = player.GetInventory();
                    var plant = FarmConfigs.PlantConfigsList[Inventory.Enums.ItemNames.AppleSeed];
                    var countApple = inventory.Items.Where(item => item.Name == Inventory.Enums.ItemNames.Apple).Sum(item => item.Count);
                    if (countApple >= _minApples)
                    {
                        startPage = new DialogPage("startQuest_25",
                            ped.Name,
                            ped.Role)
                            .AddAnswer("startQuest_10", PedFarm_SellApples);
                    }
                    else
                    {
                        startPage = new DialogPage("startQuest_24",
                            ped.Name,
                            ped.Role)
                            .AddAnswer("startQuest_28", faqSeed)
                            .AddCloseAnswer("startQuest_10");
                    }
                    break;
                case StartQuestNames.Stage7AutoSchool:
                    startPage = new DialogPage("startQuest_26",
                        ped.Name,
                        ped.Role)
                        .AddCloseAnswer("startQuest_10");
                    break;
                default:
                    startPage = new DialogPage("startQuest_12",
                        ped.Name,
                        ped.Role)
                        .AddCloseAnswer("startQuest_13");
                    break;
            }
            startPage.OpenForPlayer(player);
        }
        private void PedFarm_CallBack(PlayerGo player)
        {
            var inventory = player.GetInventory();
            var itemSeed = ItemsFabric.CreateSeed(Inventory.Enums.ItemNames.AppleSeed, 10, false);
            var itemWatering = ItemsFabric.CreateWatering(Inventory.Enums.ItemNames.WateringLow, false);
            var itemBox = ItemsFabric.CreateOther(Inventory.Enums.ItemNames.FoodBox, 1, false);
            inventory.AddItem(itemSeed);
            inventory.AddItem(itemWatering);
            inventory.AddItem(itemBox);
            player.TriggerEvent("biginfo:show", "startQuest_48");
            QuestFinish(player);
        }
        private void PedFarm_SellApples(PlayerGo player)
        {
            var inventory = player.GetInventory();
            var plant = FarmConfigs.PlantConfigsList[Inventory.Enums.ItemNames.AppleSeed];
            var countApple = inventory.Items.Where(item => item.Name == Inventory.Enums.ItemNames.Apple).Sum(item => item.Count);
            if (countApple > 2)
            {
                inventory.RemoveItems(item => item.Name == Inventory.Enums.ItemNames.Apple);
                Wallet.MoneyAdd(player.Character, (int)((countApple - 2) * plant.Price * FarmConfigs.CellCoef), "Money_SellHarvest");
                var item = ItemsFabric.CreateFood(Inventory.Enums.ItemNames.Apple, 2, false);
                inventory.AddItem(item);
                player.TriggerEvent("biginfo:show", "startQuest_59");
                StartQuestManager.EndQuest(player, StartQuestNames.Stage6WorkInFarm);
            }
        }
        protected override void StartStage(PlayerGo player)
        {
            player.SendTip("tip_input_promo");
            player.CreateClientBlip(780, 1, "Target", StartQuestSettings.PedsFarm.Position, 1, 46, NAPI.GlobalDimension);
            player.CreateWaypoint(StartQuestSettings.PedsFarm.Position);
            QuestInformation.Show(player, "startQuest_46", "startQuest_47");
        }
        protected override void FinishStage(PlayerGo player)
        {
            player.DeleteClientBlip(780);
        }
        protected override void StopStage(PlayerGo player)
        {
            player.DeleteClientBlip(780);
        }
    }
}
