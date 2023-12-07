using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.Fractions.Models;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Jobs.Farm.Configs;
using Whistler.PersonalEvents;
using Whistler.SDK;

namespace Whistler.StartQuest.QuestStages
{
    class Stage6WorkInFarm : BaseStage
    {

        public Stage6WorkInFarm()
        {
            StageName = StartQuestNames.Stage6WorkInFarm;
            EventManager.AddEvent(PlayerActions.FilledTheWateringCan, FilledTheWateringCan);
            EventManager.AddEvent(PlayerActions.PlantingSeed, PlantingSeed);
            EventManager.AddEvent(PlayerActions.WateringPlant, WateringPlant);
            EventManager.AddEvent(PlayerActions.HarvestPlant, HarvestPlant);
        }

        protected override void StartStage(ExtPlayer player)
        {
            player.StartQuestTempParam = 1;
            player.CreateWaypoint(FarmConfigs.WaterPoints[0]);
            player.CreateClientBlip(783, 1, "Target", FarmConfigs.WaterPoints[0], 1, 46, NAPI.GlobalDimension);
            player.CreateClientMarker(783, 0, FarmConfigs.WaterPoints[0] + new Vector3(0, 0, 2), 1, NAPI.GlobalDimension, new Color(50, 200, 100), new Vector3());
            QuestInformation.Show(player, "Наберите воды", "Найдите бочки с водой и наполните лейку");
        }
        protected override void FinishStage(ExtPlayer player)
        {
            QuestInformation.Hide(player);
            player.StartQuestTempParam = 0;
            player.DeleteClientMarker(783);
            player.DeleteClientBlip(783);
        }
        protected override void StopStage(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"farm:deleteHelpMarkers");
            QuestInformation.Hide(player);
            player.StartQuestTempParam = 0;
            player.DeleteClientMarker(783);
            player.DeleteClientBlip(783);
        }


        public void FilledTheWateringCan(ExtPlayer player, int count)
        {
            if (player.Character.QuestStage == StartQuestNames.Stage6WorkInFarm && player.StartQuestTempParam == 1)
            {
                QuestInformation.Show(player, "startQuest_51", "startQuest_52");
                SafeTrigger.ClientEvent(player,"farm:createHelpMarkers", 0, 0);
                player.StartQuestTempParam++;
                player.DeleteClientMarker(783);
                player.DeleteClientBlip(783);
            }
        }


        public void PlantingSeed(ExtPlayer player, int count)
        {
            if (player.Character.QuestStage == StartQuestNames.Stage6WorkInFarm && player.StartQuestTempParam == 2)
            {
                QuestInformation.Show(player, "startQuest_53", "startQuest_54");
                SafeTrigger.ClientEvent(player,"farm:deleteHelpMarkers");
                player.StartQuestTempParam++;
            }
        }


        public void WateringPlant(ExtPlayer player, int count)
        {
            if (player.Character.QuestStage == StartQuestNames.Stage6WorkInFarm && player.StartQuestTempParam == 3)
            {
                QuestInformation.Show(player, "startQuest_55", "startQuest_56");
                player.StartQuestTempParam++;
            }
        }

        public void HarvestPlant(ExtPlayer player, int count)
        {
            if (player.Character.QuestStage == StartQuestNames.Stage6WorkInFarm && player.StartQuestTempParam == 4)
            {
                QuestInformation.Show(player, "startQuest_57", "startQuest_58");
                player.StartQuestTempParam++;
                player.CreateWaypoint(StartQuestSettings.PedsFarm.Position);
                player.CreateClientBlip(783, 1, "Target", StartQuestSettings.PedsFarm.Position, 1, 46, NAPI.GlobalDimension);
            }
        }
    }
}
