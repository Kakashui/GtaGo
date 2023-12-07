using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Jobs.ImpovableJobs;
using Whistler.SDK;

namespace Whistler.NewDonateShop.Models
{
    class TruckExpDonateItem : BaseDonateItem
    {
        public TruckExpDonateItem(int amount)
        {
            Amount = amount;
        }
        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var character = player.Character;
            if (!character.ImprovableJobs.ContainsKey(ImprovableJobType.ProductsLoader))
            {
                Notify.Send(player, NotifyType.Error, NotifyPosition.BottomCenter, "dshop:item:truck:err".Translate(Amount), 3000);
                return false;
            }
                
            character.ImprovableJobs[ImprovableJobType.ProductsLoader].StagesPassed += Amount;
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:item:truck:ok".Translate(Amount), 3000);
            return true;
        }
    }
}
