using GTANetworkAPI;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.Helpers;

namespace Whistler.StartQuest.QuestStages
{
    class Stage8LastInstructions : BaseStage
    {
        public Stage8LastInstructions()
        {
            StageName = StartQuestNames.Stage8LastInstructions;
            var ped = new QuestPed(StartQuestSettings.PedAutoScool);
            ped.PlayerInteracted += PedStartAutoScool_PlayerInteracted;
        }
        private void PedStartAutoScool_PlayerInteracted(PlayerGo player, QuestPed ped)
        {
            var level = player.Character.QuestStage;
            DialogPage startPage;
            switch (level)
            {
                case StartQuestNames.Stage8LastInstructions:
                    startPage = new DialogPage("startQuest_33",
                        ped.Name,
                        ped.Role)
                        .AddAnswer("startQuest_8", PedStartAutoScool_CallBack);
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
        private void PedStartAutoScool_CallBack(PlayerGo player)
        {
            player.TriggerEvent("biginfo:show", "startQuest_62");
            QuestFinish(player);
        }
        protected override void StartStage(PlayerGo player)
        {
            player.CreateClientBlip(782, 1, "Target", StartQuestSettings.PedAutoScool.Position, 1, 46, NAPI.GlobalDimension);
            player.CreateClientMarker(782, 0, StartQuestSettings.PedAutoScool.Position + new Vector3(0, 0, 2), 1, NAPI.GlobalDimension, new Color(50, 200, 100), new Vector3());
            player.CreateWaypoint(StartQuestSettings.PedAutoScool.Position);
        }
        protected override void StopStage(PlayerGo player)
        {
            player.DeleteClientBlip(782);
            player.DeleteClientMarker(782);
        }
        protected override void FinishStage(PlayerGo player)
        {
            player.DeleteClientBlip(782);
            player.DeleteClientMarker(782);
        }
    }
}
