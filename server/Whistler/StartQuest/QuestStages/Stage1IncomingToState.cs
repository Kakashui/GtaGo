using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.Fractions.Models;
using Whistler.Helpers;

namespace Whistler.StartQuest.QuestStages
{
    class Stage1IncomingToState : BaseStage
    {
        public Stage1IncomingToState()
        {
            StageName = StartQuestNames.Stage1IncomingToState;
            var ped = new QuestPed(StartQuestSettings.PedRailwayStation);
            ped.PlayerInteracted += PedStartRailwayStation_PlayerInteracted;
        }
        private void PedStartRailwayStation_PlayerInteracted(PlayerGo player, QuestPed ped)
        {
            var level = player.Character.QuestStage;
            DialogPage startPage;
            switch (level)
            {
                case StartQuestNames.Stage1IncomingToState:
                    var giveLicAndTask = new DialogPage("startQuest_9",
                        ped.Name,
                        ped.Role)
                        .AddAnswer("startQuest_10", PedStartRailwayStation_CallBack);
                    startPage = new DialogPage("startQuest_7",
                        ped.Name,
                        ped.Role)
                        .AddAnswer("startQuest_8", giveLicAndTask);
                    break;
                case StartQuestNames.Stage2GetRentVehicle:
                    startPage = new DialogPage("startQuest_11",
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
        private void PedStartRailwayStation_CallBack(PlayerGo player)
        {
            player.GiveLic(GUI.Documents.Enums.LicenseName.Auto, 7);
            player.TriggerEvent("biginfo:show", "startQuest_35");
            QuestFinish(player);
        }
        protected override void StartStage(PlayerGo player)
        {
            player.CreateClientBlip(778, 1, "Target", StartQuestSettings.PedRailwayStation.Position, 1, 46, NAPI.GlobalDimension);
            player.CreateClientMarker(778, 0, StartQuestSettings.PedRailwayStation.Position + new Vector3(0, 0, 2), 1, NAPI.GlobalDimension, new Color(50, 200, 100), new Vector3());
            player.SendTip("tip_6");
            player.SendTip("tip_5");
        }
        protected override void StopStage(PlayerGo player)
        {
            player.DeleteClientBlip(778);
            player.DeleteClientMarker(778);
        }
        protected override void FinishStage(PlayerGo player)
        {
            player.DeleteClientBlip(778);
            player.DeleteClientMarker(778);
        }
    }
}