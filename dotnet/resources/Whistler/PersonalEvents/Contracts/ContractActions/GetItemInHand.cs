using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core.CustomSync.Attachments;
using Whistler.Entities;
using Whistler.PersonalEvents.Contracts.Models;
using Whistler.SDK;

namespace Whistler.PersonalEvents.Contracts.ContractActions
{
    class GetItemInHand : ActionBase
    {
        public AbstractItemNames AbstractItem { get; set; }
        public GetItemInHand(AbstractItemNames abstractItem)
        {
            AbstractItem = abstractItem;
        }
        public override void PlayerInteract(ExtPlayer player, ContractModel contract, GTANetworkAPI.Vector3 point)
        {
            if (player.IsPlayerHaveContainer())
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.Bottom, "medsupply_2".Translate(), 3000);
                return;
            }
            var contractProgress = contract.GetContractProgress(player);
            if (contractProgress == null || !contractProgress.CheckProgress())
            {
                Notify.SendError(player, "options_program_28");
                return;
            }
            if (point != null)
            {
                if (contractProgress.PointsVisited.Contains(point))
                {
                    Notify.SendError(player, "options_program_36");
                    return;
                }
            }
            ContractItem contractItem = new ContractItem(contractProgress, AbstractItem, contractProgress.ExpirationDate, 1);
            player.GiveContainerToPlayer(contractItem, AttachId.SupplyBox);
            if (point != null)
            {
                contractProgress.PointsVisited.Add(point);
            }
        }
    }
}