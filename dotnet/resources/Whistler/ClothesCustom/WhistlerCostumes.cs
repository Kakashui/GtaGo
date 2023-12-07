using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.ClothesCustom
{
    internal static class WhistlerCostumes
    {
        public static void SetWhistlerCostume(this ExtPlayer player, Inventory.Enums.CostumeNames costume)
        {
            SafeTrigger.SetSharedData(player, Constants.Costume, (int)costume);
        }
    }
}
