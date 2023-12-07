using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Whistler.ClothesCustom
{
    internal static class WhistlerClothes
    {
        public static void SetWhistlerClothes(this Player player, int slot, int drawable, int texture)
        {
            if (slot > 11 || slot < 1) return;
            player.SetSharedData($"{Constants.PrefixCloth}{slot}", new List<int>{ drawable, texture});
        }
    }
}
