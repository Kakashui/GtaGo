using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.Fractions.Models;
using Whistler.GUI;
using Whistler.Helpers;

namespace Whistler.StartQuest.QuestStages
{
    class Stage7AutoSchool : BaseStage
    {
        private static Vector3 _scoolPoint = new Vector3(-814.8553, -1347.102, 5.203162);

        public Stage7AutoSchool()
        {
            StageName = StartQuestNames.Stage7AutoSchool;
            StartQuestManager.EnterSchoolBlip += IncomingToSchool;
        }

        protected override void StartStage(PlayerGo player)
        {
            player.SendTip("tip_input_promo");
            player.CreateClientBlip(781, 1, "Target", _scoolPoint, 1, 46, NAPI.GlobalDimension);
            QuestInformation.Show(player, "startQuest_60", "startQuest_61");
            player.CreateWaypoint(_scoolPoint);
            Trigger.ClientEvent(player, "startquest:Stage7AutoSchool", _scoolPoint);
        }


        public static void IncomingToSchool(PlayerGo player)
        {
            player.DeleteClientBlip(781);
            player.SendTip("tip_autoschool_help");
            StartQuestManager.DeletePlayerVehicle(player);
            QuestInformation.Hide(player);
        }
        protected override void FinishStage(PlayerGo player)
        {
            player.DeleteClientBlip(781);
            QuestInformation.Hide(player);
        }
        protected override void StopStage(PlayerGo player)
        {
            player.DeleteClientBlip(781);
            Trigger.ClientEvent(player, "startquest:Stage7AutoSchool:stop");
            QuestInformation.Hide(player);
        }
    }
}
