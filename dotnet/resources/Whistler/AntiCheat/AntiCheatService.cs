using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Helpers;
using Whistler.Inventory;
using Whistler.Inventory.Enums;
using Whistler.SDK;

namespace Whistler.AntiCheatServices
{
    internal class AntiCheatService : Script
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(AntiCheatService));
        private static List<uint> forbiddenWeapon = new List<uint>
        {
            1119849093,
            2982836145,
            1752584910,
            1834241177,
            3056410471,
            125959754,
            1672152130,
            1305664598,
            2726580491,
            2138347493,
            3125143736,
            2481070269,
            741814745,
            615608432,
            2874559379
        };

        [RemoteEvent("ac:w:fb")]
        public void OnSpeedLimitExceeded(ExtPlayer player, uint weapon)
        {
            if (!player.IsLogged() || player.Character.AdminLVL > 0) return;
            Chat.SendToAdmins(1, $"[AntiCheat]: [{player.Character.UUID}]{player.Name} banned: forbidden weapon");
            _logger.WriteWarning($"forbidden weapon (${player.Name}) : {weapon}");
            Admin.hardbanPlayer(player, 9999, "cheat w1");
        }

        [RemoteEvent("ac:teleport")]
        public static void OnTeleportCheat(ExtPlayer player, string positionPrev, string positionNew, int speed, int dist)
        {
            if (!player.IsLogged()) return;
            if (player.IsAdmin()) return;
            if (player.HasData("lastTeleport"))
            {
                DateTime dateTime = player.GetData<DateTime>("lastTeleport");
                if(dateTime < DateTime.Now)
                    Chat.SendToAdmins(2, $"[AntiCheat]: [{player.Character.UUID}]{player.Name} (teleport). TP from {positionPrev} to {positionNew}. Speed {speed}, Distance: {dist}");
            }
            else
                Chat.SendToAdmins(2, $"[AntiCheat]: [{player.Character.UUID}]{player.Name} (teleport). TP from {positionPrev} to {positionNew}. Speed {speed}, Distance: {dist}");
        }

        public static void ArmorHack_BadArmor(ExtPlayer player, int armor) {
            _logger.WriteWarning($"ArmorHack:\n{player.Name} BodyArmour:{armor} / player:{player.Armor}");
        }

        public static void ArmorHack_NoArmor(ExtPlayer player, int clientArmour)
        {
            _logger.WriteWarning($"ArmorHack_NoArmor:\n{player.Name} not have BodyArmour but  client armour = {clientArmour}");
        }

        public static void BadClientWeaponHandle(ExtPlayer player, string msg)
        {
            _logger.WriteWarning($"bad client weapon (${player.Name}): {msg}");
            var message = $"Несоответсвие оружия у игрока с id {player.Character.UUID}. Необходимо проверить.";
            SendToAdminChat(message);
        }

        public static void KillPlayerHandle(ExtPlayer player, ExtPlayer killer, uint weapon)
        {
            if (killer.IsLogged())
            {
                var eq = killer.GetEquip();
                var killWeapon = eq.CurrentWeapon == WeaponSlots.Invalid ? null : eq.Weapons[eq.CurrentWeapon];
                var killWeponHash = killWeapon == null ? 0 : killWeapon.Config.WeaponHash;
                var distance = killer == player ? 0 : (int)player.Position.DistanceTo2D(killer.Position);
                GameLog.Kill(player.Name, killer.Name, weapon, killWeponHash, distance);
                if (forbiddenWeapon.Contains(weapon)/* && Main.Players[killer].AdminLVL < 1*/ && !eq.Weapons.Values.Any(w=>w.Config.WeaponHash == weapon))
                {
                    _logger.WriteWarning($"forbidden weapon (${killer.Name}) : {weapon}");
                    Chat.SendToAdmins(1, $"forbidden weapon ${killer.Name}({killer.Value}): {weapon}");
                    Admin.hardbanPlayer(player, 9999, "cheat w2");
                }
                //else if (killWeponHash != weapon && weapon != 2725352035)
                //{
                //    _logger.WriteWarning($"mismatch weapon (${player.Name}): {killWeponHash}/{weapon}");
                //    Chat.SendToAdmins(1, $"mismatch weapon ${killer.Name}({killer.Value}): {weapon}");
                //}
            }
        }

        private static void SendToAdminChat(string msg)
        {
            foreach (ExtPlayer player in SafeTrigger.GetAllPlayers())
            {
                if (player.Character == null) continue;
                if (player.Character.AdminLVL < 2) continue;

                SafeTrigger.ClientEvent(player, "chat:api:action", ChatType.AdminChat, msg, -1);
            }
        }

        [RemoteEvent("__ragemp_cheat_detected")]
        public void CheatDetect1(ExtPlayer player, string cheatCode)
        {
            _logger.WriteWarning($"ragemp_cheat_detected1: {cheatCode}");
        }

        [RemoteEvent("_ragemp_cheat_detected")]
        public void CheatDetect2(ExtPlayer player, string cheatCode)
        {
            _logger.WriteWarning($"ragemp_cheat_detected2: {cheatCode}");
        }

        [RemoteEvent("ragemp_cheat_detected")]
        public void CheatDetect3(ExtPlayer player, string cheatCode)
        {
            _logger.WriteWarning($"ragemp_cheat_detected3: {cheatCode}");
        }
    }
}