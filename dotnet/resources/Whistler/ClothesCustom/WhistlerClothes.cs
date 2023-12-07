using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.ClothesCustom
{
    internal static class WhistlerClothes
    {
        public static void SetWhistlerClothes(this ExtPlayer player, int slot, int drawable, int texture)
        {
            if (slot > 11 || slot < 1) return;
            SafeTrigger.SetSharedData(player, $"{Constants.PrefixCloth}{slot}", new List<int>{ drawable, texture});
        }
    }
}
