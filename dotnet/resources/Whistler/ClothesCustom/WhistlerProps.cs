using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Entities;
using Whistler.SDK;

namespace Whistler.ClothesCustom
{
    static class WhistlerProps
    {
        public static void SetWhistlerProps(this ExtPlayer player, int slot, int drawable, int texture)
        {
            if (slot > 12 || slot < 0) return;
            SafeTrigger.SetSharedData(player, $"{Constants.PrefixProp}{slot}", new List<int> { drawable, texture });
        }
    }
}
