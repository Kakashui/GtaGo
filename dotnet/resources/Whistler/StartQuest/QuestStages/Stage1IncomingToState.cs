using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.Fractions.Models;
using Whistler.GUI;
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

        private void PedStartRailwayStation_PlayerInteracted(ExtPlayer player, QuestPed ped)
        {
            var level = player.Character.QuestStage;
            DialogPage startPage;
            switch (level)
            {
                case StartQuestNames.Stage1IncomingToState:
                    var giveLicAndTask = new DialogPage("Держи еще пару долларов, они тебе понадобятся. Удачи!",
                        ped.Name,
                        ped.Role)
                        .AddAnswer("startQuest_10", PedStartRailwayStation_CallBack);

                    var startPage3 = new DialogPage("Тогда давай по порядку, сначала тебе нужно поехать и заработать денег для восстановления прав, я тебе дам немного на ход ноги. Этого должно хватить на аренду скутера. Человек у которого ты сможешь взять напрокат транспорт, находится с обратной стороны этого здания. Езжай!",
                        ped.Name,
                        ped.Role)
                        .AddAnswer("Хорошо, спасибо тебе, Джо.", giveLicAndTask);
                    
                    var startPage2 = new DialogPage("Вот это да… тебе нужна помощь?",
                        ped.Name,
                        ped.Role)
                        .AddAnswer("Уже чувствую себя лучше, но у меня украли все деньги и документы.", startPage3);
                    
                    var startPage1 = new DialogPage("Что с тобой случилось?",
                        ped.Name,
                        ped.Role)
                        .AddAnswer("Последнее, что я помню плыл на лайнере и как меня начали грабить в моей каюте, а потом ударили битой по голове.", startPage2);
                    
                    
                    startPage = new DialogPage("Здарова, как поживаешь?",
                        ped.Name,
                        ped.Role)
                        .AddAnswer("Не может быть. Как мы давно не виделись. Привет, Джо.", startPage1);
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
        private void PedStartRailwayStation_CallBack(ExtPlayer player)
        {
            MoneySystem.Wallet.MoneyAdd(player.Character, 300, "Помощь Joe");
            QuestFinish(player);
        }
        protected override void StartStage(ExtPlayer player)
        {
            QuestInformation.Show(player, "Поговорите с Joe", "Найдите Joe и пообщайтесь с ним.");
            player.CreateClientBlip(778, 1, "Joe", StartQuestSettings.PedRailwayStation.Position, 1, 46, NAPI.GlobalDimension);
            player.CreateWaypoint(StartQuestSettings.PedRailwayStation.Position);
            player.CreateClientMarker(778, 0, StartQuestSettings.PedRailwayStation.Position + new Vector3(0, 0, 2), 1, NAPI.GlobalDimension, new Color(50, 200, 100), new Vector3());
            player.SendTip("tip_6");
            player.SendTip("tip_5");
        }
        protected override void StopStage(ExtPlayer player)
        {
            QuestInformation.Hide(player);
            player.DeleteClientBlip(778);
            player.DeleteClientBlip(778);
            player.DeleteClientMarker(778);
        }
        protected override void FinishStage(ExtPlayer player)
        {
            QuestInformation.Hide(player);
            player.DeleteClientBlip(778);
            player.DeleteClientBlip(778);
            player.DeleteClientMarker(778);
        }
    }
}