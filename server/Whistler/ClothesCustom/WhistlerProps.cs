using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whistler.ClothesCustom
{
    static class WhistlerProps
    {
        public static void SetWhistlerProps(this Player player, int slot, int drawable, int texture)
        {
            if (slot > 12 || slot < 0) return;
            player.SetSharedData($"{Constants.PrefixProp}{slot}", new List<int> { drawable, texture });
        }
    }
}
