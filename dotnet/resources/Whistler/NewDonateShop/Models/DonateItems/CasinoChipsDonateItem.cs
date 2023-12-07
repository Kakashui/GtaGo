using GTANetworkAPI;
using ServerGo.Casino.Business;
using ServerGo.Casino.ChipModels;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.NewDonateShop.Interfaces;
using Whistler.SDK;

namespace Whistler.NewDonateShop.Models
{
    class CasinoChipsDonateItem: BaseDonateItem
    {
        public CasinoChipsDonateItem(int amount)
        {
            Amount = amount;
        }      

        public override bool TryUse(ExtPlayer player, int count, bool sell)
        {
            var character = player.Character;
            var total = Amount;
            var chips = new List<Chip>();
            for (var type = ChipType.Yellow; type > ChipType.Undefined; type--)
            {
                Chip chip = ChipFactory.Create(type);
                while (total >= chip.Value)
                {
                    total -= chip.Value;
                    chips.Add(chip);
                }
                if (total == 0) break;
            }
            CasinoManager.FindFirstCasino()?.AddChips(player, chips);
            CasinoManager.FindFirstCasino()?.CashBox.Charge(Amount);
            Notify.Send(player, NotifyType.Success, NotifyPosition.BottomCenter, "dshop:item:chips:ok".Translate( Amount), 3000);
            return true;
        }
    }
}
