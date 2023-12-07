using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Core.QuestPeds;
using Whistler.Entities;
using Whistler.Fractions.Models;
using Whistler.Helpers;

namespace Whistler.MiniGames.MetalPlant
{
    class MetalPlantPoint
    {
        public QuestPed Ped;
        public Blip Blip;
        public MetalPlantPoint(QuestPedParamModel pedParams)
        {
            Ped = new QuestPed(pedParams.Hash, pedParams.Position, pedParams.Name, pedParams.Role, pedParams.Heading, pedParams.Dimension, 2);
            Ped.PlayerInteracted += Ped_PlayerInteracted;
            Blip = NAPI.Blip.CreateBlip(648, pedParams.Position, 1, 49, "Metal Make", 255, 0, true, 0, NAPI.GlobalDimension);
        }
        private void Ped_PlayerInteracted(ExtPlayer player, QuestPed ped)
        {
            DialogPage startPage;
            if (!player.CheckLic(GUI.Documents.Enums.LicenseName.MetalPlantWorker))
            {
                startPage = new DialogPage("mg:mp:job:noLic",
                    ped.Name,
                    ped.Role)
                    .AddCloseAnswer("mg:mp:job:cancel");
            }
            else
            {
                var giveLicAndTask = new DialogPage("mg:mp:job:descWork",
                    ped.Name,
                    ped.Role)
                    .AddAnswer("mg:mp:job:startWork", MetalPlantService.WorkBegine)
                    .AddCloseAnswer("mg:mp:job:cancel");
                startPage = new DialogPage("mg:mp:job:hi",
                    ped.Name,
                    ped.Role)
                    .AddAnswer("mg:mp:job:tallAboutWork", giveLicAndTask)
                    .AddAnswer("mg:mp:job:startWork", MetalPlantService.WorkBegine)
                    .AddCloseAnswer("mg:mp:job:cancel");
            }
            startPage.OpenForPlayer(player);
        }
    }
}
