using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory.Enums;
using Whistler.SDK;

namespace Whistler.Inventory
{
    public class WeaponEvents : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(WeaponEvents));

        [RemoteEvent("weapon:cheat")]
        public void WeaponCheat(PlayerGo player, string clientWeapon, string factWeapon)
        {
            try
            {
                AntiCheatServices.AntiCheatService.BadClientWeaponHandle(player, $"{clientWeapon}/{factWeapon}");
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }
        }

        [RemoteEvent("weapon:reload")]
        public void WeaponReloadWeapon(PlayerGo player, int ammo)
        {
            //Console.WriteLine($"ammo {ammo}");
            try
            {
                if (!player.IsLogged()) return;
                var equip = player.GetEquip();
                if (equip == null) return;
                equip.ReloadWeapon(player,  ammo);
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }
        }

        [RemoteEvent("weapon:activate")]
        public void WeaponActivate(PlayerGo player, int slotId, int ammo)
        {
            try
            {
                if (!player.IsLogged()) return;
                if (player.HasData("item:action")) return;
                var equip = player.GetEquip();
                if (equip == null) return;
                equip.SetActiveItem(player, (WeaponSlots)slotId, ammo);
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }
        }

        [RemoteEvent("weapon:settings:update")]
        public void UpdateWeaponSettings(PlayerGo player, string config, string coef)
        {
            if (!player.IsLogged()) return;
            if (!Group.CanUseAdminCommand(player, "startwfbattle")) return;
            InventoryService.UpdateWeaponsSetting(config, coef);
        }

        [Command("damagesettings")]
        public void ChangeDamageSettings(PlayerGo player)
        {
            if (!Group.CanUseAdminCommand(player, "damagesettings")) return;
            player.TriggerEvent("weapon:settings:open");
        }
    }
}
