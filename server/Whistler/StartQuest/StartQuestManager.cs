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
using Whistler.Entities;
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

        public static void TryingStartNextQuest(PlayerGo player)
        {
            if (!player.IsLogged())
                return;
            if (!player.GetCharacter().IsSpawned)
                return;
            WhistlerTask.Run(() =>
            {
                StartQuest(player);
            }, 1000);
        }
        public static void StartQuest(PlayerGo player)
        {
            var stage = GetPlayerStageQuest(player);
            stage?.QuestStart(player);      
        }

        public static void EndQuest(PlayerGo player, StartQuestNames stageName)
        {
            if (stageName != player.Character.QuestStage)
                return;
            _questions.GetValueOrDefault(stageName)?.QuestFinish(player);
        }

        private static BaseStage GetPlayerStageQuest(PlayerGo player)
        {
            if (_questions.ContainsKey(player.GetCharacter().QuestStage))
                return _questions[player.GetCharacter().QuestStage];
            return null;
        }

        public static void DeletePlayerVehicle(PlayerGo player)
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
        public static void CMD_setstage(PlayerGo player, int id, int stage)
        {
            if (!player.IsLogged())
                return;
            if (!Group.CanUseAdminCommand(player, "setstage"))
                return;
            var target = Main.GetPlayerByID(id);
            if (target == null)
                return;
            var currStage = GetPlayerStageQuest(player);
            target.Character.QuestStage = (StartQuestNames)stage;
            if (currStage != null)
                currStage.BreakStage(player);
            else
                TryingStartNextQuest(target);
        }


        public static Action<PlayerGo> EnterSchoolBlip;
        [RemoteEvent("startquest:enterSchoolBlip")]
        public static void Event_EnterSchoolBlip(PlayerGo player)
        {
            EnterSchoolBlip?.Invoke(player);
        }



    }
}
