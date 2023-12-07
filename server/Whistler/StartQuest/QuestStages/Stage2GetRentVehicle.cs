using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Whistler.Common.CommonClasses;
using Whistler.Core;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.GUI.Tips;
using Whistler.Helpers;
using Whistler.SDK;
using Whistler.VehicleSystem;
using Whistler.VehicleSystem.Models.VehiclesData;

namespace Whistler.StartQuest.QuestStages
{
    internal class Stage2ArrivalInTheCity : BaseStage
    {

        private static Random _rnd = new Random();

        private static PositionWithHeading _carPosition = new PositionWithHeading(new Vector3(1203.4094, -2970.7153, 5.6799936), new Vector3(-0.38828555, 0.008736938, -87.96143)); //спавн тачки
        private List<string> _listCars = new List<string>
        {
            "tornado2",
            "picador",
            "virgo",
            "pigalle",
            "lurcher",
            "vigero",
        };
        public Stage2ArrivalInTheCity()
        {
            StageName = StartQuestNames.Stage2GetRentVehicle;
            var ped = new QuestPed(StartQuestSettings.PedRent);
            ped.PlayerInteracted += Ped_PlayerInteracted;
        }

        private void Ped_PlayerInteracted(PlayerGo player, QuestPed ped)
        {

            var level = player.Character.QuestStage;
            DialogPage startPage;
            switch (level)
            {
                case StartQuestNames.Stage2GetRentVehicle:
                    startPage = new DialogPage("startQuest_2",
                        ped.Name,
                        ped.Role)
                        .AddAnswer("startQuest_3", SpawnCars);
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

        public void SpawnCars(Player player)
        {
            var car = _listCars.GetRandomElement();
            var veh = VehicleManager.CreateTemporaryVehicle(car, _carPosition.Position, _carPosition.Rotation, player.Name, VehicleAccess.StartQuest, player);
            player.AddTempVehicle(veh, VehicleAccess.StartQuest);
            player.CustomSetIntoVehicle(veh, VehicleConstants.DriverSeatClientSideBroken);
            veh.SetSharedData("HOLDERNAME", veh.GetVehicleGo().Data.GetHolderName());
            veh.Dimension = 0;
            VehicleCustomization.SetColor(veh, new Color(_rnd.Next(0, 256), _rnd.Next(0, 256), _rnd.Next(0, 256)), 1, true);
            VehicleCustomization.SetColor(veh, new Color(_rnd.Next(0, 256), _rnd.Next(0, 256), _rnd.Next(0, 256)), 1, false);
            VehicleStreaming.SetEngineState(veh, false);
            player.DeleteClientBlip(776);
            player.DeleteClientMarker(776);
            QuestInformation.Show(player, "startQuest_38", "startQuest_39");
            player.CreateClientBlip(779, 1, "Target", StartQuestSettings.PedGov.Position, 1, 46, NAPI.GlobalDimension);
            player.CreateClientMarker(779, 0, StartQuestSettings.PedGov.Position + new Vector3(0, 0, 2), 1, NAPI.GlobalDimension, new Color(50, 200, 100), new Vector3());
            player.CreateWaypoint(StartQuestSettings.PedGov.Position);
        }
        protected override void StartStage(PlayerGo player)
        {
            QuestInformation.Show(player, "startQuest_36", "startQuest_37");
            player.CreateClientBlip(776, 1, "Target", StartQuestSettings.PedRent.Position, 1, 46, NAPI.GlobalDimension);
            player.CreateClientMarker(776, 0, StartQuestSettings.PedRent.Position + new Vector3(0, 0, 2), 1, NAPI.GlobalDimension, new Color(50, 200, 100), new Vector3());
            player.CreateWaypoint(StartQuestSettings.PedRent.Position);
            player.SendTip("tip_6");
            player.SendTip("tip_5");
        }
        protected override void StopStage(PlayerGo player)
        {
            player.DeleteClientBlip(776);
            player.DeleteClientMarker(776);
            player.DeleteClientBlip(779);
            player.DeleteClientMarker(779);
            QuestInformation.Hide(player);
        }
        protected override void FinishStage(PlayerGo player)
        {
            player.DeleteClientBlip(779);
            player.DeleteClientMarker(779);
            QuestInformation.Hide(player);
        }
    }
}
