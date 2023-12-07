using GTANetworkAPI;
using Whistler.Core;
using Whistler.SDK;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Whistler.Helpers;
using Whistler.Entities;

namespace Whistler.GUI
{
    public static class CaptureUI
    {
        private static WhistlerLogger _logger = new WhistlerLogger(typeof(CaptureUI));
        public static void EnableCaptureUI(ExtPlayer player, int firstTeam, int secondTeam, int currentTime, int firstTeamPlayers = 0, int secondTeamPlayers = 0, bool isGangCapture = true)
        {
            SafeTrigger.ClientEvent(player, "captureUI:enable", firstTeam, secondTeam, currentTime, firstTeamPlayers, secondTeamPlayers, isGangCapture);
        }

        public static void DisableCaptureUI(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player, "captureUI:disable");
        }

        public static void SetCaptureStats(ExtPlayer player, int firstTeamPlayers, int secondTeamPlayers, int currentTime)
        {
            SafeTrigger.ClientEvent(player, "captureUI:setStats", firstTeamPlayers, secondTeamPlayers, currentTime);
        }

        public static void EnableKillLog(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player, "captureUI:log:enable");
            SafeTrigger.SetData(player, "kl:enabled", true);
        }

        public static void DisableKillog(ExtPlayer player, bool reset = false)
        {
            SafeTrigger.ClientEvent(player, "captureUI:log:disable", reset);
            SafeTrigger.SetData(player, "kl:enabled", false);
        }

        public static void AddKillogItem(ExtPlayer player, ExtPlayer killer, ExtPlayer victim, uint reason)
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
            var killerFraction = killer?.Character?.FractionID ?? 0;

            SafeTrigger.ClientEvent(player, "captureUI:log:append", killerName, killerFraction, victim.Name, victim.Character.FractionID, weaponId);
        }

        public static void AddKillogItem(ExtPlayer player, ExtPlayer killer, int killerFraction, ExtPlayer victim, int victimFraction, uint reason)
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

            SafeTrigger.ClientEvent(player, "captureUI:log:append", killerName, killerFraction, victim.Name, victimFraction, weaponId);
        }
        
        public static void AddKillogEmptyItem(ExtPlayer player, ExtPlayer killer, ExtPlayer victim, uint reason)
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

            SafeTrigger.ClientEvent(player, "captureUI:log:append", killerName, 0, victim.Name, 0, weaponId);
        }

        public static void SendUntilCaptureTimer(ExtPlayer player, int maxTime, int currentTime, string message)
        {
            SafeTrigger.ClientEvent(player,"captureUI:untilCapt:send", maxTime, currentTime, message);
        }

        public static void DisableUntilCaptureTimer(ExtPlayer player)
        {
            SafeTrigger.ClientEvent(player,"captureUI:untilCapt:disable");
        }
    }
}
