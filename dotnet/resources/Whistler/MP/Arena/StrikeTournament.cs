using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.MP.Arena.Interfaces;
using Whistler.MP.Arena.UI;

namespace Whistler.MP.Arena
{
    internal class StrikeTournament : ArenaTournamentBase
    {

        public StrikeTournament(List<QuestPedParamModel> pedParams) : base(pedParams)
        {
        }
        
        protected override QuestPed ConfigureQuestPed()
        {
            QuestPed questPed = null;
            foreach (var pedParam in PedParams)
            {
                questPed = new QuestPed(pedParam);
                questPed.PlayerInteracted += (p, ped) => OpenDialog(p, ped);
            }
            return questPed;
        }
        
        private static void OpenDialog(ExtPlayer player, QuestPed questPed)
        {
            var mainPage = new DialogPage("qp:strike:2", questPed.Name, questPed.Role);
            var ratingDesriptionPage = new DialogPage("qp:strike:3", questPed.Name, questPed.Role);
            
            var howToEarnPointsPage = new DialogPage("qp:strike:4", questPed.Name, questPed.Role);
            ratingDesriptionPage.AddAnswer("qp:strike:5", mainPage);
            ratingDesriptionPage.AddAnswer("qp:strike:6", howToEarnPointsPage);
            ratingDesriptionPage.AddCloseAnswer();
            howToEarnPointsPage.AddAnswer("qp:strike:7", mainPage);
            howToEarnPointsPage.AddCloseAnswer();
            
            var tfDescriptionPage = new DialogPage("qp:strike:8", questPed.Name, questPed.Role);
            var ggDescriptionPage = new DialogPage("qp:strike:9", questPed.Name, questPed.Role);
            var dmDescriptionPage = new DialogPage("qp:strike:10", questPed.Name, questPed.Role);
            var battlesDescriptionPage = new DialogPage("qp:strike:11", questPed.Name, questPed.Role);
            battlesDescriptionPage.AddAnswer("qp:strike:12", tfDescriptionPage);
            battlesDescriptionPage.AddAnswer("qp:strike:13", ggDescriptionPage);
            battlesDescriptionPage.AddAnswer("qp:strike:14", dmDescriptionPage);
            battlesDescriptionPage.AddCloseAnswer();
            
            tfDescriptionPage.AddAnswer("qp:strike:15", mainPage);
            tfDescriptionPage.AddCloseAnswer();
            ggDescriptionPage.AddAnswer("qp:strike:15", mainPage);
            ggDescriptionPage.AddCloseAnswer();
            dmDescriptionPage.AddAnswer("qp:strike:15", mainPage);
            dmDescriptionPage.AddCloseAnswer();

            mainPage.AddAnswer("qp:strike:16", ArenaUiUpdateHandler.OpenMenuForPlayer);
            mainPage.AddAnswer("qp:strike:17", battlesDescriptionPage);
            mainPage.AddAnswer("qp:strike:18", ratingDesriptionPage);
            
            mainPage.AddCloseAnswer();
            mainPage.OpenForPlayer(player);
        }
    }
}