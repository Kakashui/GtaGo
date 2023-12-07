using GTANetworkAPI;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.SDK;

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
        private void PedStartAutoScool_PlayerInteracted(ExtPlayer player, QuestPed ped)
        {
            QuestInformation.Hide(player);
            var level = player.Character.QuestStage;
            DialogPage startPage;
            switch (level)
            {
                case StartQuestNames.Stage8LastInstructions:
                    startPage = new DialogPage("Отлично! Ты получил права и теперь можешь приступить к работам водителя. Более подробно с работами можно ознакомиться в мэрии. Чтобы добраться до туда, воспользуйся арендой, которая находится на территории автошколы или вызови такси. Удачи!",
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
        private void PedStartAutoScool_CallBack(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"biginfo:show", "startQuest_62");
            QuestFinish(player);
        }
        protected override void StartStage(ExtPlayer player)
        {
            QuestInformation.Show(player, "Поговорите с Paper", "Найдите Paper и поговорите с ним. Он находится на территории автошколы.");
            player.CreateClientBlip(782, 1, "Paper", StartQuestSettings.PedAutoScool.Position, 1, 46, NAPI.GlobalDimension);
            player.CreateClientMarker(782, 0, StartQuestSettings.PedAutoScool.Position + new Vector3(0, 0, 2), 1, NAPI.GlobalDimension, new Color(50, 200, 100), new Vector3());
            player.CreateWaypoint(StartQuestSettings.PedAutoScool.Position);
        }
        protected override void StopStage(ExtPlayer player)
        {
            player.DeleteClientBlip(782);
            player.DeleteClientMarker(782);
            QuestInformation.Hide(player);
        }
        protected override void FinishStage(ExtPlayer player)
        {
            player.DeleteClientBlip(782);
            player.DeleteClientMarker(782);
            QuestInformation.Hide(player);
        }
    }
}
