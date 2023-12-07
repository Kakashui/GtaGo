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

        protected override void StartStage(PlayerGo player)
        {
            player.StartQuestTempParam = 1;
            player.CreateWaypoint(FarmConfigs.WaterPoints[0]);
            player.CreateClientMarker(783, 0, FarmConfigs.WaterPoints[0] + new Vector3(0, 0, 2), 1, NAPI.GlobalDimension, new Color(50, 200, 100), new Vector3());
            QuestInformation.Show(player, "startQuest_49", "startQuest_50");
        }
        protected override void FinishStage(PlayerGo player)
        {
            QuestInformation.Hide(player);
            player.StartQuestTempParam = 0;
            player.DeleteClientMarker(783);
        }
        protected override void StopStage(PlayerGo player)
        {
            player.TriggerEvent("farm:deleteHelpMarkers");
            QuestInformation.Hide(player);
            player.StartQuestTempParam = 0;
        }


        public void FilledTheWateringCan(PlayerGo player, int count)
        {
            if (player.Character.QuestStage == StartQuestNames.Stage6WorkInFarm && player.StartQuestTempParam == 1)
            {
                QuestInformation.Show(player, "startQuest_51", "startQuest_52");
                player.TriggerEvent("farm:createHelpMarkers", 0, 0);
                player.StartQuestTempParam++;
                player.DeleteClientMarker(783);
            }
        }


        public void PlantingSeed(PlayerGo player, int count)
        {
            if (player.Character.QuestStage == StartQuestNames.Stage6WorkInFarm && player.StartQuestTempParam == 2)
            {
                QuestInformation.Show(player, "startQuest_53", "startQuest_54");
                player.TriggerEvent("farm:deleteHelpMarkers");
                player.StartQuestTempParam++;
            }
        }


        public void WateringPlant(PlayerGo player, int count)
        {
            if (player.Character.QuestStage == StartQuestNames.Stage6WorkInFarm && player.StartQuestTempParam == 3)
            {
                QuestInformation.Show(player, "startQuest_55", "startQuest_56");
                player.StartQuestTempParam++;
            }
        }

        public void HarvestPlant(PlayerGo player, int count)
        {
            if (player.Character.QuestStage == StartQuestNames.Stage6WorkInFarm && player.StartQuestTempParam == 4)
            {
                QuestInformation.Show(player, "startQuest_57", "startQuest_58");
                player.StartQuestTempParam++;
                player.CreateWaypoint(StartQuestSettings.PedsFarm.Position);
            }
        }
    }
}
