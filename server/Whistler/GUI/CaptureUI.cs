using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Whistler.Helpers;

namespace Whistler.GUI
{
    public static class CaptureUI
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(CaptureUI));
        public static void EnableCaptureUI(Player player, int firstTeam, int secondTeam, int currentTime, int firstTeamPlayers = 0, int secondTeamPlayers = 0, bool isGangCapture = true)
        {
            Trigger.ClientEvent(player, "captureUI:enable", firstTeam, secondTeam, currentTime, firstTeamPlayers, secondTeamPlayers, isGangCapture);
        }

        public static void DisableCaptureUI(Player player)
        {
            Trigger.ClientEvent(player, "captureUI:disable");
        }

        public static void SetCaptureStats(Player player, int firstTeamPlayers, int secondTeamPlayers, int currentTime)
        {
            Trigger.ClientEvent(player, "captureUI:setStats", firstTeamPlayers, secondTeamPlayers, currentTime);
        }

        public static void EnableKillLog(Player player)
        {
            Trigger.ClientEvent(player, "captureUI:log:enable");
            player.SetData("kl:enabled", true);
        }

        public static void DisableKillog(Player player, bool reset = false)
        {
            Trigger.ClientEvent(player, "captureUI:log:disable", reset);
            player.SetData("kl:enabled", false);
        }

        public static void AddKillogItem(Player player, Player killer, Player victim, uint reason)
        {
            var weaponId = 99;

            try
            {
                var weaponHash = (Weapons.Hash) reason;
                var weaponItemType = Enum.Parse<ItemType>(weaponHash.ToString(), true);
                weaponId = (int)weaponItemType;
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }

            var killerName = killer?.Name ?? "World";
            var killerFraction = killer?.GetCharacter()?.FractionID ?? 0;

            Trigger.ClientEvent(player, "captureUI:log:append", killerName, killerFraction, victim.Name, victim.GetCharacter().FractionID, weaponId);
        }

        public static void AddKillogItem(Player player, Player killer, int killerFraction, Player victim, int victimFraction, uint reason)
        {
            var weaponId = 99;

            try
            {
                var weaponHash = (Weapons.Hash) reason;
                var weaponItemType = Enum.Parse<ItemType>(weaponHash.ToString(), true);
                weaponId = (int)weaponItemType;
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }

            if ((int) reason == -842959696 || (int) reason == 539292904)
                weaponId = 99;
            var killerName = killer?.Name ?? "World";
            //var killerFraction = (killer.IsNull) ? 0 : Main.Players[killer].FractionID;

            Trigger.ClientEvent(player, "captureUI:log:append", killerName, killerFraction, victim.Name, victimFraction, weaponId);
        }
        
        public static void AddKillogEmptyItem(Player player, Player killer, Player victim, uint reason)
        {
            var weaponId = 99;

            try
            {
                var weaponHash = (Weapons.Hash) reason;
                var weaponItemType = Enum.Parse<ItemType>(weaponHash.ToString(), true);
                weaponId = (int)weaponItemType;
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex.ToString());
            }

            var killerName = killer?.Name ?? "World";

            Trigger.ClientEvent(player, "captureUI:log:append", killerName, 0, victim.Name, 0, weaponId);
        }

        public static void SendUntilCaptureTimer(Player player, int maxTime, int currentTime, string message)
        {
            player.TriggerEvent("captureUI:untilCapt:send", maxTime, currentTime, message);
        }

        public static void DisableUntilCaptureTimer(Player player)
        {
            player.TriggerEvent("captureUI:untilCapt:disable");
        }
    }
}
