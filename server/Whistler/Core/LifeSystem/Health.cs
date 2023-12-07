using GTANetworkAPI;
using System;
using System.Collections.Generic;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Configs.Models;
using Whistler.Inventory.Enums;
using Whistler.Inventory.Models;
using Whistler.SDK;

namespace Whistler.Core.LifeSystem
{
    public class Health : Script
    {
        public Health()
        {
            InventoryService.OnUseLifeActivityItem += OnUseHealthItem;
        }

        public void OnUseHealthItem(Player player, LifeActivityData data)
        {
            if (data.Hp < 1) return;

            player.Health = Math.Min(100, Math.Max(0, player.Health + data.Hp));
        }
    }
}
