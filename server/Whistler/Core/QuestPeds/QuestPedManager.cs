using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.Fractions;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Core.QuestPeds
{
    internal class QuestPedManager : Script 
    {
        public static List<QuestPed> QuestPeds { get; } = new List<QuestPed>();
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(QuestPedManager));

        public QuestPedManager()
        {
            var questPed = new QuestPed(PedHash.FbiSuit01, new Vector3(1853, 2598, 45.67), "Karim_Denz", "");
            questPed.PlayerInteracted += (player, ped) =>
            {
                var descPage =
                    new DialogPage("questpeds_3", ped.Name, ped.Role)
                        .AddCloseAnswer();
                var workDescriptionPage = new DialogPage("questpeds_4",
                        ped.Name, questPed.Role)
                    .AddAnswer("questpeds_5", descPage)
                    .AddCloseAnswer("questpeds_6");
                workDescriptionPage.OpenForPlayer(player);
            };
            
            var medicPed = new QuestPed(PedHash.Paramedic01SMM, new Vector3(304.5436, -588.2626, 43.25), "Jock Cranley", "qp:med:1", interactionRange: 2, heading: 68);
            medicPed.PlayerInteracted += (player, ped) =>
            {
                var descPage = new DialogPage("questpeds_7".Translate(Ems.HealByBotPrice), ped.Name, ped.Role)
                    .AddAnswer("questpeds_8", Ems.HealPlayerByPed)
                    .AddCloseAnswer("questpeds_9");
                var introPage =
                    new DialogPage("questpeds_10", ped.Name, ped.Role)
                        .AddAnswer("questpeds_11", Ems.HealPlayerByPed)
                        .AddAnswer("questpeds_12", descPage)
                        .AddCloseAnswer("questpeds_13");
                
                introPage.OpenForPlayer(player);
            };
        }
        
        [ServerEvent(Event.PlayerConnected)]
        public static void OnPlayerConnected(PlayerGo player)
        {
            try
            {
                player.TriggerEvent("questPeds:load", JsonConvert.SerializeObject(QuestPeds));
            }
            catch (Exception e) { _logger.WriteError("QuestPeds: " + e.ToString()); }
        }
        
        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(PlayerGo player, DisconnectionType type, string reason)
        {
            try
            {
                if (DialogPage.OpenedPages.ContainsKey(player)) DialogPage.OpenedPages.Remove(player);
            }
            catch (Exception e) { _logger.WriteError("QuestPeds: " + e.ToString()); }
        }
        
        [RemoteEvent("dialogWindow:playerSelectedAnswer")]
        public static void OnPlayerSelectedAnswer(PlayerGo player, int answerId)
        {
            try
            {
                if (DialogPage.OpenedPages.ContainsKey(player)) 
                    DialogPage.OpenedPages[player].OnPlayerSelectedAnswer(player, answerId);                
            }
            catch (Exception e) { _logger.WriteError("QuestPeds: " + e.ToString()); }
        }

        [RemoteEvent("dialogWindow:playerClosedDialog")]
        public static void OnPlayerClosedDialog(PlayerGo player)
        {
            try
            {
                if (DialogPage.OpenedPages.ContainsKey(player))
                    DialogPage.OpenedPages[player].OnPlayerClosedDialog(player);
            }
            catch (Exception e) { _logger.WriteError("QuestPeds: " + e.ToString()); }
        }
    }
}