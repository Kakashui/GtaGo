using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Core.QuestPeds;
using Whistler.Fractions.Models;

namespace Whistler.StartQuest
{
    class StartQuestSettings //взять квест у нпс
    {
        public static QuestPedParamModel PedRailwayStation = new QuestPedParamModel(PedHash.Chip, new Vector3(1220.1044, -2999.3096, 5.8653526), "Smith", "startQuest_30", 31, 0, 2); //в проту
        public static QuestPedParamModel PedRent = new QuestPedParamModel(PedHash.ChiBoss01GMM, new Vector3(1224.02, -2976.618, 5.9253845), "Bill", "startQuest_34", 124, 0, 2); //в проту
        public static QuestPedParamModel PedGov = new QuestPedParamModel(PedHash.Bankman, new Vector3(-533.96014, -187.05411, 38.21968), "Frank", "startQuest_29", 153, 0, 2);
        public static QuestPedParamModel PedsFarm = new QuestPedParamModel(PedHash.Trucker01SMM, new Vector3(1869.3287, 4848.3584, 44.32099), "Salton", "startQuest_31", 45, 0, 2);
        public static QuestPedParamModel PedAutoScool = new QuestPedParamModel(PedHash.Paper, new Vector3(-808.84686, -1342.6624, 5.1753764), "Paper", "startQuest_32", -150, 0, 2);
    }
}
