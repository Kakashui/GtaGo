using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Phone.Calls;
using Whistler.Helpers;
using Whistler.StartQuest.QuestStages;
using System.Linq.Expressions;
using Whistler.VehicleSystem.Models.VehiclesData;
using Whistler.VehicleSystem;
using Whistler.GUI;
using Whistler.Entities;
using Whistler.Core.QuestPeds;
using Whistler.Fractions.Models;

namespace Whistler.StartQuest
{
    internal class StartQuestManager : Script
    {
        protected static WhistlerLogger _logger = new WhistlerLogger(typeof(StartQuestManager));

        /// <summary>
        /// Стадии квеста
        /// </summary>
        private static Dictionary<StartQuestNames, BaseStage> _questions;

        public StartQuestManager()
        {
            _questions = new Dictionary<StartQuestNames, BaseStage>()
            {
                { StartQuestNames.Stage1IncomingToState,   new Stage1IncomingToState() },
                { StartQuestNames.Stage2GetRentVehicle,    new Stage2ArrivalInTheCity() },
                { StartQuestNames.Stage3DeliveryOfMail,    new Stage3GotoBurger() },
                { StartQuestNames.Stage4InspectTheDisplay, new Stage4InspectTheDisplay() },
                { StartQuestNames.Stage5GetFarmInventory,  new Stage5GetFarmInventory() },
                { StartQuestNames.Stage6WorkInFarm,        new Stage6WorkInFarm() },
                { StartQuestNames.Stage7AutoSchool,        new Stage7AutoSchool() },
                { StartQuestNames.Stage8LastInstructions,  new Stage8LastInstructions() },
            };
            Main.OnPlayerReady += TryingStartNextQuest;
        }

        public static void TryingStartNextQuest(ExtPlayer player)
        {
            if (!player.IsLogged()) return;
            if (!player.Character.IsSpawned) return;
            NAPI.Task.Run(() =>
            {
                StartQuest(player);
            }, 1000);
        }
        public static void StartQuest(ExtPlayer player)
        {
            BaseStage stage = GetPlayerStageQuest(player);
            if (stage == null) return;

            stage.QuestStart(player);      
        }

        public static void EndQuest(ExtPlayer player, StartQuestNames stageName)
        {
            if (stageName != player.Character.QuestStage)
                return;
            _questions.GetValueOrDefault(stageName)?.QuestFinish(player);
        }

        private static BaseStage GetPlayerStageQuest(ExtPlayer player)
        {
            if (_questions.ContainsKey(player.Character.QuestStage))
                return _questions[player.Character.QuestStage];
            return null;
        }

        public static void DeletePlayerVehicle(ExtPlayer player)
        {
            try
            {
                player.RemoveTempVehicle(VehicleAccess.StartQuest)?.CustomDelete();
            }
            catch (Exception e)
            {
                _logger.WriteError($"OnPlayerEnterVehicleHandler:\n{e}");
            }
        }

        [Command("setstage")]
        public static void CMD_setstage(ExtPlayer player, int id, int stage)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "setstage")) return;

            ExtPlayer target = Trigger.GetPlayerByUuid(id);
            if (target == null)
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, $"Игрок не найден", 3000);
                return;
            }

            BaseStage currStage = GetPlayerStageQuest(player);
            target.Character.QuestStage = (StartQuestNames)stage;
            if (currStage != null) currStage.BreakStage(player);
            else TryingStartNextQuest(target);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, $"Вы успешно установили {target.Name} квест #{stage}", 3000);
        }


        public static Action<ExtPlayer> EnterSchoolBlip;
        [RemoteEvent("startquest:enterSchoolBlip")]
        public static void Event_EnterSchoolBlip(ExtPlayer player)
        {
            EnterSchoolBlip?.Invoke(player);
        }



    }
}
