using System.Collections.Generic;
using GTANetworkAPI;
using Whistler.Core.QuestPeds;
using Whistler.MP.Arena.Interfaces;

namespace Whistler.MP.Arena
{
    internal class ArenaTournamentManager : Script
    {
        private static List<ArenaTournamentBase> _tournaments = new List<ArenaTournamentBase>();

        private static List<QuestPedParamModel> questStrikePedParams = new List<QuestPedParamModel>
        {
            new QuestPedParamModel(PedHash.CaseyCutscene, new Vector3(-272.555, -2032.3927, 30.1456), "Bob Wayne", "qp:strike:1", -69, 0, 7),
            // new QuestPedParamModel(PedHash.CaseyCutscene, new Vector3(-833.3248, -2351.214, 14.68466), "Bob Wayne", "qp:strike:1", 271, 0, 5),
            // new QuestPedParamModel(PedHash.CaseyCutscene, new Vector3(-1929.05, 3323.448, 32.96026), "Bob Wayne", "qp:strike:1", 76, 0, 5),
            // new QuestPedParamModel(PedHash.CaseyCutscene, new Vector3(826.997, -2157.789, 29.61902), "Bob Wayne", "qp:strike:1", 115, 0, 2),
            // new QuestPedParamModel(PedHash.CaseyCutscene, new Vector3(6.118212, -1100.9338, 29.797007), "Bob Wayne", "qp:strike:1", -92, 0, 2),
            // new QuestPedParamModel(PedHash.CaseyCutscene, new Vector3(-1119.3938, 2695.01, 18.554153), "Bob Wayne", "qp:strike:1", -26, 0, 2),
            // new QuestPedParamModel(PedHash.CaseyCutscene, new Vector3(2571.294, 295.7627, 108.73487), "Bob Wayne", "qp:strike:1", 105, 0, 2),
            // new QuestPedParamModel(PedHash.CaseyCutscene, new Vector3(-3172.0579, 1084.0016, 20.83875), "Bob Wayne", "qp:strike:1", -18.5F, 0, 2),
            // new QuestPedParamModel(PedHash.CaseyCutscene, new Vector3(-665.75085, -936.6858, 21.82923), "Bob Wayne", "qp:strike:1", -86, 0, 2),
            // new QuestPedParamModel(PedHash.CaseyCutscene, new Vector3(252.00024, -46.142723, 69.94108), "Bob Wayne", "qp:strike:1", 163.5F, 0, 2),
            // new QuestPedParamModel(PedHash.CaseyCutscene, new Vector3(1692.2294, 3756.3123, 34.705364), "Bob Wayne", "qp:strike:1", -37.5F, 0, 2),
            // new QuestPedParamModel(PedHash.CaseyCutscene, new Vector3(-331.7368, 6080.303, 31.454763), "Bob Wayne", "qp:strike:1", -34.5F, 0, 2),
            // new QuestPedParamModel(PedHash.CaseyCutscene, new Vector3(-1306.3943, -390.67804, 36.695766), "Bob Wayne", "qp:strike:1", 169.5F, 0, 2),
        };

        private static List<QuestPedParamModel> questRacingPedParams = new List<QuestPedParamModel>
        {
            new QuestPedParamModel(PedHash.Claypain, new Vector3(-245.632, -2005.314, 30.14558), "Red Valdez", "racing:6", 157, 0),
        };
        public ArenaTournamentManager()
        {
            _tournaments.Add(new StrikeTournament(questStrikePedParams));
            _tournaments.Add(new RacingTournament(questRacingPedParams));
            
            NAPI.Blip.CreateBlip(546, new Vector3(-254, -2019, 30), 1f, 2, "Арена", shortRange: true);
        }
    }
}