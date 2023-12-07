using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Whistler.Common;
using Whistler.Core;
using Whistler.Entities;
using Whistler.Families;
using Whistler.Fractions.AttackArmy.Models;
using Whistler.Helpers;
using Whistler.SDK;

namespace Whistler.Fractions.AttackArmy
{
    class AttackManager : Script
    {
        private static List<PowerStation> _stations = new List<PowerStation>();
        private static PowerStationStatus _globalStatus = PowerStationStatus.Work;
        private static string _timer = null;
        private static DateTime _lastBroken = DateTime.Now.AddDays(-1);
        private const int _intervalAttackArmyMinutes = 60;
        private const int _intervaRepairMinutes = 5;
        private static AttackMembers _moneyMembers = null;
        public AttackManager()
        {
            _stations.Add(new PowerStation(new Vector3(2107.3, 2338.3, 93.3), true));
            _stations.Add(new PowerStation(new Vector3(2091.1, 2330.4, 93.3), false));
            _stations.Add(new PowerStation(new Vector3(2094.1, 2320.0, 93.3), false));
        }

        public static bool StartTimer(PowerStationStatus checkStatus, PowerStationStatus newStatus, string message)
        {
            if (CheckStations(checkStatus))
            {
                foreach (var station in _stations)
                {
                    if (station.InteractedPlayer.IsLogged())
                    {
                        Notify.SendInfo(station.InteractedPlayer, message);
                        if (newStatus == PowerStationStatus.Work)
                            station.InteractedPlayer.CreatePlayerAction(PersonalEvents.PlayerActions.RepairNGStation, 1);
                    }
                }
                Timers.StartOnce(30000, () =>
                {
                    SetGlobalStatus(newStatus);
                    _timer = null;
                });
                return true;
            }
            return false;
        }

        public static bool CheckLoadItems(ExtPlayer player)
        {
            if (_globalStatus == PowerStationStatus.Work || _globalStatus == PowerStationStatus.Breaking)
                return false;
            return CheckAccess(player, true, false, false);
        }

        public static void StopTimer()
        {
            if (_timer != null)
            {
                Timers.Stop(_timer);
                _timer = null;
            }
        }
        private static bool CheckStations(PowerStationStatus checkStatus)
        {
            foreach (var station in _stations)
            {
                if (station.Status != checkStatus)
                    return false;
            }
            return true;
        }
        private static void SetGlobalStatus(PowerStationStatus status)
        {
            _globalStatus = status;
            List<int> interactedPlayers = new List<int>();
            foreach (var station in _stations)
            {
                station.SetStatus(status);
                if (station.InteractedPlayer.IsLogged())
                {
                    int member = station.InteractedPlayer?.Character?.UUID ?? 0;
                    if (member > 0)
                        interactedPlayers.Add(member);
                }
            }
            if (status == PowerStationStatus.Broken)
            {
                _lastBroken = DateTime.Now;
                _moneyMembers = new AttackMembers();
                _moneyMembers.SetCrimeBrokenStation(interactedPlayers);
                Chat.SendFractionMessage(14, "elstation:warn", false);
                Chat.SendFractionMessage(14, "elstation:warn", true);
            }
            else
            {
                _moneyMembers?.SetArmyRepairStation(interactedPlayers);
                _moneyMembers?.Payment();
                Chat.SendFractionMessage(14, "elstation:rest", false);
                Chat.SendFractionMessage(14, "elstation:rest", true);
            }
        }

        public static void AddLoaderItems(ExtPlayer player)
        {
            if (!player.IsLogged())
                return;
            int member = player?.Character?.UUID ?? 0;
            if (member > 0)
                _moneyMembers?.AddCrimeLoadItem(member);
        }

        public static bool CanBreakingStation(PowerStationStatus status)
        {
            if (status == PowerStationStatus.Work)
                return _lastBroken.AddMinutes(_intervalAttackArmyMinutes) < DateTime.Now;
            else if (status == PowerStationStatus.Broken)
                return _lastBroken.AddMinutes(_intervaRepairMinutes) < DateTime.Now;
            return false;
        }
        public static bool CheckAccess(ExtPlayer player, bool isGang, bool onlyLeader, bool notify = true)
        {
            if (isGang)
            {
                if (onlyLeader)
                {
                    if (Manager.IsGang(player) && Manager.IsLeaderOrSub(player) || (player.GetFamily()?.OrgActiveType ?? OrgActivityType.Unknown) == OrgActivityType.Crime && (player.GetFamily()?.CanAccessToBizWar(player) ?? false))
                        return true;
                    else if (notify)
                        Notify.SendError(player, "elstation:leader");
                }
                else
                {
                    if (Manager.IsGang(player) || (player.GetFamily()?.OrgActiveType ?? OrgActivityType.Unknown) == OrgActivityType.Crime)
                        return true;
                    else if (notify)
                        Notify.SendError(player, "elstation:leader");
                }
            }
            else
            {
                if (Manager.inFraction(player, 14))
                    return true;
                else if (notify)
                    Notify.SendError(player, "can:army");
            }
            return false;
        }
    }
}
