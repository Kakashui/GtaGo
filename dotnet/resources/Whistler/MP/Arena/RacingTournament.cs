using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.MP.Arena.Interfaces;
using Whistler.MP.Arena.Racing;
using Whistler.MP.Arena.UI;
using Whistler.MP.Arena.UI.DTO;
using Whistler.SDK;

namespace Whistler.MP.Arena
{
    internal class RacingTournament : ArenaTournamentBase
    {
        public RacingTournament(List<QuestPedParamModel> pedParams) : base(pedParams)
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
            DialogPage mainPage;
            if (RacingManager.CurrentMap != null && !RacingManager.CurrentMap.Started)
            {
                mainPage = new DialogPage("racing:3".Translate(RacingManager.CurrentMap.Name), questPed.Name, questPed.Role);
                mainPage.AddAnswer("racing:4", OpenEventsMenu);
            }
            else
            {
                mainPage = new DialogPage("racing:5", questPed.Name, questPed.Role);
            }
            
            mainPage.AddCloseAnswer();
            mainPage.OpenForPlayer(player);
        }

        private static void OpenEventsMenu(ExtPlayer player)
        {
            if (!GameEventsEvents.Subscribers.Contains(player)) GameEventsEvents.Subscribers.Add(player);
            
            var mapper = MapperManager.Get();
            if (RacingManager.CurrentMap != null)
                player.TriggerCefEvent("events/setCurrentEventId", (int) RacingManager.CurrentMap.RacingName);
                
            player.TriggerCefEvent("events/setCurrentEventsState", 
                JsonConvert.SerializeObject(mapper.Map<IEnumerable<CurrentRacingDto>>(RacingManager.Events.Events.ToList())));
                
            SafeTrigger.ClientEvent(player,"race:openMenu");
        }
    }
}