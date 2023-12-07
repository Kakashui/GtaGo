using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Core;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.Fractions.GOV.Config;
using Whistler.Fractions.Models;
using Whistler.Helpers;

namespace Whistler.StartQuest.QuestStages
{
    class Stage4InspectTheDisplay : BaseStage
    {
        public Stage4InspectTheDisplay()
        {
            StageName = StartQuestNames.Stage4InspectTheDisplay;
        }


        protected override void StartStage(PlayerGo player)
        {
            player.StartQuestTempParam = 0;
            player.CreateClientMarker(780, 0, Consignments.VotePositions[0] + new Vector3(0, 0, 2), 1, NAPI.GlobalDimension, new Color(50, 200, 100), new Vector3());
            player.CreateClientMarker(781, 27, Consignments.VotePositions[0], 1, NAPI.GlobalDimension, InteractShape.DefaultMarkerColor, new Vector3());
        }

        protected override void FinishStage(PlayerGo player)
        {
            player.StartQuestTempParam = 0;
            player.DeleteClientMarker(781);
            player.DeleteClientMarker(780);
        }

        protected override void StopStage(PlayerGo player)
        {
            player.StartQuestTempParam = 0;
            player.DeleteClientMarker(781);
            player.DeleteClientMarker(780);
        }
    }
}
