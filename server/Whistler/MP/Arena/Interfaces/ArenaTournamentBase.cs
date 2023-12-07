using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Core.QuestPeds;

namespace Whistler.MP.Arena.Interfaces
{
    internal abstract class ArenaTournamentBase
    {
        protected List<QuestPedParamModel> PedParams { get; }
        protected QuestPed StartPed { get; }
        
        protected ArenaTournamentBase(List<QuestPedParamModel> pedParams)
        {
            PedParams = pedParams;
            StartPed = ConfigureQuestPed();
        }

        protected abstract QuestPed ConfigureQuestPed();
    }
}