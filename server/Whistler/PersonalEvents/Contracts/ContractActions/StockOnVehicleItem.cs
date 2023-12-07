using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.PersonalEvents.Contracts.Models;
using Whistler.SDK;

namespace Whistler.PersonalEvents.Contracts.ContractActions
{
    class StockOnVehicleItem : ActionBase
    {
        public AbstractItemNames AbstractItem { get; set; }
        public StockOnVehicleItem(AbstractItemNames abstractItem)
        {
            AbstractItem = abstractItem;
        }
        public override void PlayerInteract(PlayerGo player, ContractModel contract, GTANetworkAPI.Vector3 point)
        {
            if (!player.IsInVehicle)
            {
                Notify.SendError(player, "options_program_27");
                return;
            }
            var vehGo = player.Vehicle.GetVehicleGo();
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
                    Notify.SendError(player, "options_program_35");
                    return;
                }
            }

            int itemCount = vehGo.Data.GetAbstractItem(contractProgress, AbstractItem, contractProgress.ExpirationDate, point != null ? 1 : int.MaxValue);
            if (itemCount <= 0)
            {
                Notify.SendError(player, "В машине нет нужных ящиков");
                return;
            }
            var result = contract.ContractProgressed(player, itemCount);
            if (result > 0)
            {
                Notify.SendSuccess(player, "options_program_29".Translate(result));
                if (point != null)
                {
                    contractProgress.PointsVisited.Add(point);
                }
            }
            else
            {
                Notify.SendSuccess(player, "options_program_30");
            }
        }
    }
}
