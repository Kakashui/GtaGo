using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Whistler.ClothesCustom
{
    internal static class WhistlerCostumes
    {
        public static void SetWhistlerCostume(this Player player, Inventory.Enums.CostumeNames costume)
        {
            player.SetSharedData(Constants.Costume, (int)costume);
        }
    }
}
