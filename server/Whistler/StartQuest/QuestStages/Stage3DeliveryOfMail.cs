using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.Fractions.Models;
using Whistler.GUI;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.MoneySystem;

namespace Whistler.StartQuest.QuestStages
{
    class Stage3GotoBurger : BaseStage
    {
        private List<AutoRoomPoint> _autoroomPositions = new List<AutoRoomPoint> //доставка почти в автосалонах
        {
            new AutoRoomPoint(new Vector3(-790.4354, -208.20871, 37.252777), 1),
            //new AutoRoomPoint(new Vector3(-805.0755, -222.98671, 37.252777), 1),
            new AutoRoomPoint(new Vector3(-122.82194, -586.0485, 37.75329), 2),
            //new AutoRoomPoint(new Vector3(-117.20474, -608.2347, 37.75329), 2),
            new AutoRoomPoint(new Vector3(273.38412, -1162.9672, 29.272997), 4),
            new AutoRoomPoint(new Vector3(75.75917, -1747.0688, 29.629784), 8),
        };
        Vector3 _mailPos = new Vector3(-535.79486, -170.78406, 38.219646);

        int _moneyForMail = 15000;
        public Stage3GotoBurger()
        {
            StageName = StartQuestNames.Stage3DeliveryOfMail;
            var ped = new QuestPed(StartQuestSettings.PedGov);
            ped.PlayerInteracted += PedStartGov_PlayerInteracted;
            var mailShape = Core.InteractShape.Create(_mailPos, 2, 2, 0)
                .AddInteraction(MailInteract, "startQuest_16")
                .AddEnterPredicate(EnterMailShape);
            foreach (var autoroomPos in _autoroomPositions)
            {
                var shape = Core.InteractShape.Create(autoroomPos.Position, 2, 2, 0)
                    .AddInteraction(p => AutoRoomInteract(p, autoroomPos.ID), "startQuest_63")
                    .AddEnterPredicate((c, p) => EnterAutoRoomShape(c, p, autoroomPos.ID));
            }
        }
        private void PedStartGov_PlayerInteracted(PlayerGo player, QuestPed ped)
        {
            var fraction = player.GetFraction();
            DialogPage startPage;
            var level = player.Character.QuestStage;
            if (level == StartQuestNames.Stage2GetRentVehicle)
            {
                StartQuestManager.EndQuest(player, StartQuestNames.Stage2GetRentVehicle);
                level = player.Character.QuestStage;
            }
            switch (level)
            {
                case StartQuestNames.Stage3DeliveryOfMail:
                    if (player.StartQuestTempParam == 0)
                    {
                        var sendMail = new DialogPage("startQuest_15",
                            ped.Name,
                            ped.Role)
                            .AddAnswer("startQuest_10", Ped_GetMail);
                        startPage = new DialogPage("startQuest_14",
                            ped.Name,
                            ped.Role)
                            .AddAnswer("startQuest_8", sendMail);
                    }
                    else if (player.StartQuestTempParam == 16)
                    {
                        Wallet.MoneyAdd(player.Character, _moneyForMail, "Money_StartQuestMail");
                        Fractions.SkinManager.TakePlayerCostumes(player.Character.UUID, ClothesOwn.Work);
                        QuestFinish(player);
                        startPage = new DialogPage("startQuest_20",
                            ped.Name,
                            ped.Role)
                            .AddCloseAnswer("startQuest_10");
                    }
                    else
                    {
                        startPage = new DialogPage("startQuest_15",
                            ped.Name,
                            ped.Role)
                            .AddCloseAnswer("startQuest_10");
                    }
                    break;
                case StartQuestNames.Stage4InspectTheDisplay:
                    if (player.StartQuestTempParam == 0)
                    {
                        startPage = new DialogPage("startQuest_21",
                            ped.Name,
                            ped.Role)
                            .AddCloseAnswer("startQuest_10");
                    }
                    else
                    {
                        StartQuestManager.EndQuest(player, StartQuestNames.Stage4InspectTheDisplay);
                        startPage = new DialogPage("startQuest_22",
                            ped.Name,
                            ped.Role)
                            .AddCloseAnswer("startQuest_10");
                    }
                    break;
                case StartQuestNames.Stage5GetFarmInventory:
                    startPage = new DialogPage("startQuest_22",
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
        private void Ped_GetMail(PlayerGo player)
        {
            QuestInformation.Show(player, "startQuest_40", "startQuest_41");
            player.CreateWaypoint(_mailPos);
            player.CreateClientBlip(777, 1, "Target", _mailPos, 1, 46, NAPI.GlobalDimension);
            player.CreateClientMarker(777, 0, _mailPos + new Vector3(0, 0, 1), 1, NAPI.GlobalDimension, new Color(50, 200, 100), new Vector3());
        }
        protected void MailInteract(PlayerGo player)
        {
            if (player.Character.QuestStage == StartQuestNames.Stage3DeliveryOfMail)
            {
                if (player.StartQuestTempParam == 0)
                {
                    player.StartQuestTempParam = 1 | 2 | 4 | 8;
                    Whistler.SDK.Notify.SendInfo(player, "startQuest_17");
                    player.DeleteClientBlip(777);
                    player.DeleteClientMarker(777);
                    Trigger.ClientEvent(player, "startquest:Stage3DeliveryOfMail");
                    QuestInformation.Show(player, "startQuest_42", "startQuest_43");
                    player.CreateWaypoint(_autoroomPositions[0].Position);
                    bool gender = player.GetGender();
                    var cloth = ItemsFabric.CreateCostume(ItemNames.StandartCostume, gender ? CostumeNames.MWorkMail1 : CostumeNames.FWorkMail1, ClothesOwn.Work, gender, true);
                    BaseItem oldItem = null;
                    player.GetEquip()?.EquipItem(player, cloth, ClothesSlots.Costume, ref oldItem, LogAction.None);
                    if (oldItem != null)
                        player.GetInventory()?.AddItem(oldItem);
                }
            }
        }
        protected bool EnterMailShape(ColShape shape, PlayerGo player)
        {
            if (player.Character.QuestStage == StartQuestNames.Stage3DeliveryOfMail)
            {
                if (player.StartQuestTempParam == 0)
                    return true;
            }
            return false;
        }
        protected void AutoRoomInteract(PlayerGo player, int id)
        {
            if (player.Character.QuestStage == StartQuestNames.Stage3DeliveryOfMail)
            {
                if ((player.StartQuestTempParam & id) == id)
                {
                    player.StartQuestTempParam &= ~id;
                    Trigger.ClientEvent(player, "startquest:Stage3DeliveryOfMail:delBlip", id);
                    if (player.StartQuestTempParam == 0)
                    {
                        player.StartQuestTempParam = 16;
                        SDK.Notify.SendSuccess(player, "startQuest_19");
                        QuestInformation.Show(player, "startQuest_44", "startQuest_45");
                        player.CreateWaypoint(StartQuestSettings.PedGov.Position);
                        player.CreateClientBlip(779, 1, "Target", StartQuestSettings.PedGov.Position, 1, 46, NAPI.GlobalDimension);
                        player.CreateClientMarker(779, 0, StartQuestSettings.PedGov.Position + new Vector3(0, 0, 2), 1, NAPI.GlobalDimension, new Color(50, 200, 100), new Vector3());
                    }
                    else
                    {
                        SDK.Notify.SendInfo(player, "startQuest_18");
                        foreach (var autoRoom in _autoroomPositions)
                        {
                            if ((player.StartQuestTempParam & autoRoom.ID) == autoRoom.ID)
                            {
                                player.CreateWaypoint(autoRoom.Position);
                                break;
                            }
                        }
                    }

                }
            }
        }
        protected bool EnterAutoRoomShape(ColShape shape, PlayerGo player, int id)
        {
            if (player.Character.QuestStage == StartQuestNames.Stage3DeliveryOfMail)
            {
                if ((player.StartQuestTempParam & id) == id)
                    return true;
            }
            return false;
        }

        protected override void StartStage(PlayerGo player)
        {
            player.SendTip("tip_2");
        }
        protected override void FinishStage(PlayerGo player)
        {
            player.StartQuestTempParam = 0;
            Trigger.ClientEvent(player, "startquest:Stage3DeliveryOfMail:close");
            QuestInformation.Hide(player);
            player.DeleteClientBlip(777);
            player.DeleteClientBlip(779);
            player.DeleteClientMarker(779);
            player.SendTip("tip_3");
        }
        protected override void StopStage(PlayerGo player)
        {
            player.StartQuestTempParam = 0;
            Trigger.ClientEvent(player, "startquest:Stage3DeliveryOfMail:stop");
            player.DeleteClientBlip(777);
            QuestInformation.Hide(player);
            player.DeleteClientBlip(779);
            player.DeleteClientMarker(779);
        }

    }

    class AutoRoomPoint
    {
        public Vector3 Position { get; set; }
        public int ID { get; set; }
        public AutoRoomPoint(Vector3 position, int iD)
        {
            Position = position;
            ID = iD;
        }
    }
}
