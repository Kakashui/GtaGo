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
using Whistler.SDK;

namespace Whistler.StartQuest.QuestStages
{
    class Stage7AutoSchool : BaseStage
    {
        private static Vector3 _scoolPoint = new Vector3(-927.4753, -2038.4761, 9.386556);

        public Stage7AutoSchool()
        {
            StageName = StartQuestNames.Stage7AutoSchool;
            StartQuestManager.EnterSchoolBlip += IncomingToSchool;
        }

        protected override void StartStage(ExtPlayer player)
        {
            GUI.Documents.Models.License license = player.Character.Licenses.FirstOrDefault(x => x.Name == GUI.Documents.Enums.LicenseName.Auto);
            if (license != null && license.IsActive)
            {
                QuestFinish(player);
                return;
            }
            QuestInformation.Show(player, "Езжайте в автошколу", "Отправляйтесь в автошколу и получите водительское удостоверение");
            player.CreateWaypoint(_scoolPoint);
            SafeTrigger.ClientEvent(player, "startquest:Stage7AutoSchool", _scoolPoint);
        }


        public static void IncomingToSchool(ExtPlayer player)
        {
            player.DeleteClientBlip(781);
            player.SendTip("tip_autoschool_help");
            StartQuestManager.DeletePlayerVehicle(player);
            QuestInformation.Hide(player);
        }
        protected override void FinishStage(ExtPlayer player)
        {
            player.DeleteClientBlip(781);
            QuestInformation.Hide(player);
        }
        protected override void StopStage(ExtPlayer player)
        {
            player.DeleteClientBlip(781);
            SafeTrigger.ClientEvent(player, "startquest:Stage7AutoSchool:stop");
            QuestInformation.Hide(player);
        }
    }
}
