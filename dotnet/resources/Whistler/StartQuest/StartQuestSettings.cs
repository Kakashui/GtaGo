using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Core.QuestPeds;
using Whistler.Fractions.Models;

namespace Whistler.StartQuest
{
    class StartQuestSettings
    {
        public static QuestPedParamModel PedRailwayStation = new QuestPedParamModel(PedHash.Chip, new Vector3(-3038.705, 62.912197, 11.603134), "Joe", "startQuest_30", -48, 0, 2);
        public static QuestPedParamModel PedRent = new QuestPedParamModel(PedHash.ChiBoss01GMM, new Vector3(-3006.052, 100.24009, 11.871033), "Bill", "startQuest_34", 124, 0, 2);
        public static QuestPedParamModel PedGov = new QuestPedParamModel(PedHash.Bankman, new Vector3(-533.96014, -187.05411, 38.21968), "Frank", "startQuest_29", 153, 0, 2);
        public static QuestPedParamModel PedsFarm = new QuestPedParamModel(PedHash.Trucker01SMM, new Vector3(1869.3287, 4848.3584, 44.32099), "Salton", "startQuest_31", 45, 0, 2);
        public static QuestPedParamModel PedAutoScool = new QuestPedParamModel(PedHash.Paper, new Vector3(-919.61536, -2034.0065, 9.402867), "Paper", "startQuest_32", -137, 0, 2);
    }
}
